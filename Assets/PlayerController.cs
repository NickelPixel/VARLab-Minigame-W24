using UnityEngine.InputSystem;
using UnityEngine;
using TMPro;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.Rendering.Universal;
using System.Collections;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using Unity.VisualScripting;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    [SerializeField]
    private float playerSpeed = 2.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    [SerializeField]
    private Vector2 movementInput = Vector2.zero;
    private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;

    [SerializeField]
    private float smoothInputSpeed;

    public bool nearCampfire;
    public bool nearTable;

    public bool nearSteed;
    public bool carrying;
    public bool feeding;
    public bool hasPlayed;
    public bool canMove;

    public float roastTime = 5;
    public bool roasting;
    public float feedTime;

    public float roastPercentage;
    public TextMeshProUGUI roastPercentageDisplay;

    public GameObject fire;
    public GameObject steed;
    public GameObject bag;

    private Vector3 _direction;
    private Quaternion _lookRotation;

    public GameObject marshmallow1;
    public GameObject marshmallow2;
    public GameObject marshmallow3;

    public RoastingReticle reticle;
    public RoastingMiniGame target;
    public GameObject bg;

    public GameObject resultText;
    public Animator resultTextAnim;
    public bool showedResult;
    public bool hasRoastedAndFed;

    private Animator playerAnim;
    public Animator horseAnim;

    public Color orange;
    public Color red;
    public Color green;
    public Color halfGreen;
    public Color halfOrange;
    public Color halfRed;
    public Color baseColor;
    public Color good;
    public Color perfect;
    public Color burnt;

    public TimerScript ts;

    public GameObject Abutton;
    public GameObject RTrigger; 


    private void Start()
    {
        Abutton.SetActive(false);
        RTrigger.SetActive(false);
        canMove = true;
        bag = GameObject.Find("Bag of Marshmallows ripped");
        hasPlayed = false;
        ts = GameObject.FindObjectOfType<TimerScript>();
        hasRoastedAndFed = false;
        marshmallow1.GetComponent<MeshRenderer>().material.color = baseColor;
        marshmallow2.GetComponent<MeshRenderer>().material.color = baseColor;
        marshmallow3.GetComponent<MeshRenderer>().material.color = baseColor;
        marshmallow1.SetActive(false);
        marshmallow2.SetActive(false);
        marshmallow3.SetActive(false);
        bg.GetComponent<Image>().color = red;
        playerAnim = GetComponent<Animator>();
        showedResult = false;
        resultText.SetActive(false);
        bg.SetActive(false);
        target = GetComponentInChildren<RoastingMiniGame>();
        target.gameObject.SetActive(false);
        reticle = GetComponentInChildren<RoastingReticle>();
        reticle.score = 50;
        reticle.gameObject.SetActive(false);
        fire = GameObject.FindGameObjectWithTag("Fire");
        //steed = GameObject.FindGameObjectWithTag("SteedTarget");
        //roastPercentageDisplay = GetComponentInChildren<TextMeshProUGUI>();
        roastPercentage = 50;
        
        roastPercentageDisplay.enabled = false;
        roastPercentageDisplay.gameObject.SetActive(false);
        roasting = false;
        nearCampfire = false;
        controller = gameObject.GetComponent<CharacterController>();
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if(ts.players.Length == 2 && canMove)
        {
            movementInput = context.ReadValue<Vector2>();
        }
    }

    public void OnRoast(InputAction.CallbackContext context)
    {
        
        target.start = target.beginning;
        if (nearCampfire && carrying && !hasRoastedAndFed)
        {
            
            resultText.SetActive(true);
            roasting = true;
            //roastPercentageDisplay.enabled = true;
            Debug.Log("Roasting...");
            //roastPercentage = 50;
            //reticle.score = 50;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (nearTable)
        {
            if (!carrying)
            {
                Abutton.SetActive(false);
                playerAnim.SetBool("Walking", false);
                canMove = false;
                movementInput = Vector2.zero;
                hasPlayed = false;
                playerAnim.SetTrigger("GetMarsh");
                target.gameObject.GetComponent<Image>().color = halfOrange;
                bg.GetComponent<Image>().color = red;
                StartCoroutine(TurnOnMellows());
                carrying = true;
                showedResult = false;
                roastTime = 5;
                feedTime = 3;
                roastPercentage = 50;
                reticle.score = 50;
                hasRoastedAndFed = false;
            }
        }
        if (nearSteed)
        {
            if (carrying)
            {
                Abutton.SetActive(false);
                if (reticle.rectTrans != null)
                {
                    reticle.rectTrans.anchoredPosition = reticle.beginningPos.anchoredPosition;
                }
                carrying = false;
                //roastPercentageDisplay.enabled = true;
                feeding = true;
                feedTime = 3;
                
                reticle.scoreCalculated = false; //mark score as uncalculated when starting feeding
            }
        }
    }

    public IEnumerator TurnOnMellows()
    {
        yield return new WaitForSeconds(1f);
        marshmallow1.GetComponent<MeshRenderer>().material.color = baseColor;
        marshmallow2.GetComponent<MeshRenderer>().material.color = baseColor;
        marshmallow3.GetComponent<MeshRenderer>().material.color = baseColor;
        marshmallow1.SetActive(true);
        marshmallow2.SetActive(true);
        marshmallow3.SetActive(true);
        canMove = true;
    }

    void Update()
    {
        
        //if (gameObject.name == "Sheriff(Clone)")
        //{
        //    steed = GameObject.FindGameObjectWithTag("SteedTarget");
        //}
        //if(gameObject.name == "Bandit(Clone)")
        //{
        //    steed = GameObject.FindGameObjectWithTag("SteedTarget2");
        //}
        _lookRotation = Quaternion.LookRotation(_direction);
        //roastPercentageDisplay.text = roastPercentage.ToString("F0") + "%";
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        currentInputVector = Vector2.SmoothDamp(currentInputVector, movementInput, ref smoothInputVelocity, smoothInputSpeed);
        Vector3 move = new Vector3(currentInputVector.x, 0, currentInputVector.y);
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero && !roasting && !feeding)
        {
            playerAnim.SetBool("Roasting", false);
            playerAnim.SetBool("Walking", true);
            gameObject.transform.forward += move;
        }
        if (move == Vector3.zero) 
        {
            playerAnim.SetBool("Walking", false);
        }


        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        if (roastPercentage > 80 && roasting)
        {
            marshmallow1.GetComponent<MeshRenderer>().material.color = Color.Lerp(marshmallow1.GetComponent<MeshRenderer>().material.color, perfect, 0 + Time.deltaTime);
            marshmallow2.GetComponent<MeshRenderer>().material.color = Color.Lerp(marshmallow1.GetComponent<MeshRenderer>().material.color, perfect, 0 + Time.deltaTime);
            marshmallow3.GetComponent<MeshRenderer>().material.color = Color.Lerp(marshmallow1.GetComponent<MeshRenderer>().material.color, perfect, 0 + Time.deltaTime);
            target.gameObject.GetComponent<Image>().color = Color.Lerp(target.gameObject.GetComponent<Image>().color, halfGreen, 0 + Time.deltaTime);
            bg.GetComponent<Image>().color = Color.Lerp(bg.GetComponent<Image>().color, green,0 + Time.deltaTime);
            resultText.GetComponent<TextMeshProUGUI>().text = "Perfect!";
            resultText.GetComponent<TextMeshProUGUI>().color = Color.Lerp(resultText.GetComponent<TextMeshProUGUI>().color, green, 0 + Time.deltaTime);
            reticle.finalScore = 100;
        }
        if (roastPercentage > 25 && roastPercentage <= 80 && roasting)
        {
            marshmallow1.GetComponent<MeshRenderer>().material.color = Color.Lerp(marshmallow1.GetComponent<MeshRenderer>().material.color, good, 0 + Time.deltaTime);
            marshmallow2.GetComponent<MeshRenderer>().material.color = Color.Lerp(marshmallow1.GetComponent<MeshRenderer>().material.color, good, 0 + Time.deltaTime);
            marshmallow3.GetComponent<MeshRenderer>().material.color = Color.Lerp(marshmallow1.GetComponent<MeshRenderer>().material.color, good, 0 + Time.deltaTime);
            target.gameObject.GetComponent<Image>().color = Color.Lerp(target.gameObject.GetComponent<Image>().color, halfOrange, 0 + Time.deltaTime);
            bg.GetComponent<Image>().color = Color.Lerp(bg.GetComponent<Image>().color, orange, 0 + Time.deltaTime);
            resultText.GetComponent<TextMeshProUGUI>().text = "Good!";
            resultText.GetComponent<TextMeshProUGUI>().color = Color.Lerp(resultText.GetComponent<TextMeshProUGUI>().color, orange, 0 + Time.deltaTime);
            reticle.finalScore = 50;
        }
        if (roastPercentage <= 25 && roastPercentage > 10 && roasting)
        {
            marshmallow1.GetComponent<MeshRenderer>().material.color = Color.Lerp(marshmallow1.GetComponent<MeshRenderer>().material.color, burnt, 0 + Time.deltaTime);
            marshmallow2.GetComponent<MeshRenderer>().material.color = Color.Lerp(marshmallow1.GetComponent<MeshRenderer>().material.color, burnt, 0 + Time.deltaTime);
            marshmallow3.GetComponent<MeshRenderer>().material.color = Color.Lerp(marshmallow1.GetComponent<MeshRenderer>().material.color, burnt, 0 + Time.deltaTime);
            target.gameObject.GetComponent<Image>().color = Color.Lerp(target.gameObject.GetComponent<Image>().color, halfRed, 0 + Time.deltaTime);
            bg.GetComponent<Image>().color = Color.Lerp(bg.GetComponent<Image>().color, red, 0 + Time.deltaTime);
            resultText.GetComponent<TextMeshProUGUI>().text = "Burned!";
            resultText.GetComponent<TextMeshProUGUI>().color = Color.Lerp(resultText.GetComponent<TextMeshProUGUI>().color, red, 0 + Time.deltaTime);
            reticle.finalScore = -100;
        }
        if(roastPercentage<= 10 && roasting)
        {
            marshmallow1.GetComponent<MeshRenderer>().material.color = Color.Lerp(marshmallow1.GetComponent<MeshRenderer>().material.color, burnt, 0 + Time.deltaTime);
            marshmallow2.GetComponent<MeshRenderer>().material.color = Color.Lerp(marshmallow1.GetComponent<MeshRenderer>().material.color, burnt, 0 + Time.deltaTime);
            marshmallow3.GetComponent<MeshRenderer>().material.color = Color.Lerp(marshmallow1.GetComponent<MeshRenderer>().material.color, burnt, 0 + Time.deltaTime);
            target.gameObject.GetComponent<Image>().color = Color.Lerp(target.gameObject.GetComponent<Image>().color, halfRed, 0 + Time.deltaTime);
            bg.GetComponent<Image>().color = Color.Lerp(bg.GetComponent<Image>().color, red, 0 + Time.deltaTime);
            resultText.GetComponent<TextMeshProUGUI>().text = "Burned!";
            resultText.GetComponent<TextMeshProUGUI>().color = Color.Lerp(resultText.GetComponent<TextMeshProUGUI>().color, red, 0 + Time.deltaTime);
            reticle.finalScore = -100;
        }


        if (feeding)
        {
            playerAnim.SetBool("Walking", false);
            playerAnim.SetBool("Roasting", true);
            
            horseAnim.SetBool("Feeding", true);
            feedTime -= Time.deltaTime;
            if (feedTime > 0)
            {
                playerSpeed = 0;
                _direction = (steed.transform.position - transform.position).normalized;
                //roastPercentageDisplay.enabled = true;
                //roastPercentageDisplay.gameObject.SetActive(true);
                //roastPercentage -= 50 * Time.deltaTime;
                transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * 10);

            }
        }

        if (feedTime < 0)
        {
            horseAnim.SetBool("Feeding", false);
            playerAnim.SetBool("Roasting", false);
            if(reticle.finalScore >= 50 && !hasPlayed)
            {
                horseAnim.SetTrigger("Happy");
                hasPlayed = true;
            }
            if(reticle.finalScore < 0 && !hasPlayed)
            {
                horseAnim.SetTrigger("Burnt");
                hasPlayed = true;
            }
            feeding = false;
            //reticle.score = 0;
            marshmallow1.SetActive(false);
            marshmallow2.SetActive(false);
            marshmallow3.SetActive(false);
            playerSpeed = 4;
            
        }
        if (roasting)
        {
            RTrigger.SetActive(false);
            playerAnim.SetBool("Walking", false);
            playerAnim.SetBool("Roasting", true);
            _direction = (fire.transform.position - transform.position).normalized;
            roastTime -= Time.deltaTime;

            //roastPercentageDisplay.gameObject.SetActive(true);
            if (roastPercentage < 0)
            {
                //roastPercentageDisplay.gameObject.SetActive(false);
            }

            if (roastTime > 0)
            {
                //roastPercentageDisplay.gameObject.SetActive(true);
                bg.SetActive(true);
                target.gameObject.SetActive(true);
                reticle.gameObject.SetActive(true);
                transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * 10);
                //roastPercentage += 20 * Time.deltaTime;
                playerSpeed = 0;
            }
            else
            {
                playerAnim.SetBool("Roasting", false);
                if (reticle.rectTrans != null)
                {
                    reticle.rectTrans.anchoredPosition = reticle.beginningPos.anchoredPosition;
                }
                roasting = false;
                bg.SetActive(false);
                target.gameObject.SetActive(false);
                reticle.gameObject.SetActive(false);
                playerSpeed = 4;
                hasRoastedAndFed = true;
                //roastPercentageDisplay.gameObject.SetActive(false);
                if (!showedResult)
                {

                    resultText.SetActive(true);
                    resultTextAnim.SetTrigger("Play");
                    StartCoroutine(hideAnim());

                }
            }
        }
        else
        {
            //playerAnim.SetBool("Roasting", false);
        }

        if(nearTable && !carrying || nearSteed && carrying)
        {
            Abutton.SetActive(true);
        }
        else
        {
            Abutton.SetActive(false);
        }
        if(nearCampfire && carrying && !roasting && !hasRoastedAndFed)
        {
            RTrigger.SetActive(true);
        }
        else
        {
            RTrigger.SetActive(false);
        }
        
    }

    public IEnumerator hideAnim()
    {
        yield return new WaitForSeconds(1f);
        showedResult = true;
        Debug.Log("Working");
        resultText.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Campfire"))
        {
            nearCampfire = true;
        }
        if (other.gameObject.CompareTag("Table"))
        {
            nearTable = true;
        }
        if (other.gameObject.CompareTag("Steed"))
        {
            nearSteed = true;
        }
        if (other.gameObject.CompareTag("SteedTarget"))
        {
            steed = other.gameObject;
            horseAnim = other.gameObject.GetComponentInChildren<Animator>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Campfire"))
        {
            nearCampfire = false;
        }
        if (other.gameObject.CompareTag("Table"))
        {
            nearTable = false;
        }
        if (other.gameObject.CompareTag("Steed"))
        {
            nearSteed = false;
            feeding = false;
        }
    }
}
