using UnityEngine.InputSystem;
using UnityEngine;
using TMPro;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.Rendering.Universal;
using System.Collections;
using UnityEngine.Advertisements;

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

    public float roastTime = 5;
    public bool roasting;
    public float feedTime;

    public float roastPercentage;
    public TextMeshProUGUI roastPercentageDisplay;

    public GameObject fire;
    public GameObject steed;

    private Vector3 _direction;
    private Quaternion _lookRotation;

    public GameObject stick;

    public RoastingReticle reticle;
    public RoastingMiniGame target;
    public GameObject bg;

    public GameObject resultText;
    public Animator resultTextAnim;
    public bool showedResult;

    private Animator playerAnim;

    private void Start()
    {
        playerAnim = GetComponent<Animator>();
        showedResult = false;
        resultText.SetActive(false);
        bg.SetActive(false);
        target = GetComponentInChildren<RoastingMiniGame>();
        target.gameObject.SetActive(false);
        reticle = GetComponentInChildren<RoastingReticle>();
        reticle.gameObject.SetActive(false);
        fire = GameObject.FindGameObjectWithTag("Fire");
        steed = GameObject.FindGameObjectWithTag("SteedTarget");
        roastPercentageDisplay = GetComponentInChildren<TextMeshProUGUI>();
        roastPercentage = 0;
        //roastPercentageDisplay.enabled = false;
        roastPercentageDisplay.gameObject.SetActive(false);
        roasting = false;
        nearCampfire = false;
        controller = gameObject.GetComponent<CharacterController>();
        stick.gameObject.SetActive(false);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();

    }

    public void OnRoast(InputAction.CallbackContext context)
    {
        target.start = target.beginning;
        if (nearCampfire && carrying)
        {
            roasting = true;
            //roastPercentageDisplay.enabled = true;
            Debug.Log("Roasting...");
        }
        //if(context.canceled)
        //{
        //    roastPercentageDisplay.enabled = false;
        //    roasting = false;
        //    Debug.Log("Done Roasting!");
        //}
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (nearTable)
        {
            if (!carrying)
            {
                stick.gameObject.SetActive(true);
                carrying = true;
                showedResult = false;
                roastTime = 5;
                feedTime = 3;
                roastPercentage = 0;
            }
        }
        if (nearSteed)
        {
            if (carrying)
            {
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

    void Update()
    {

        _lookRotation = Quaternion.LookRotation(_direction);
        roastPercentageDisplay.text = roastPercentage.ToString("F0") + "%";
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


        // Changes the height position of the player..
        //if (Input.GetButtonDown("Jump") && groundedPlayer)
        //{
        //    playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        //}

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        if (roastPercentage > 80)
        {
            resultText.GetComponent<TextMeshProUGUI>().text = "Perfect!";
        }
        if (roastPercentage > 25 && roastPercentage <= 80)
        {
            resultText.GetComponent<TextMeshProUGUI>().text = "Good!";
        }
        if (roastPercentage <= 25)
        {
            resultText.GetComponent<TextMeshProUGUI>().text = "Burned!";
        }

        if (feeding)
        {
            playerAnim.SetBool("Roasting", true);
            playerAnim.SetBool("Walking", false);
            feedTime -= Time.deltaTime;
            if (feedTime > 0)
            {
                playerSpeed = 0;
                _direction = (steed.transform.position - transform.position).normalized;
                //roastPercentageDisplay.enabled = true;
                roastPercentageDisplay.gameObject.SetActive(true);
                roastPercentage -= 50 * Time.deltaTime;
                transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * 10);

                //if (roastPercentage <= 0)
                //{
                //    reticle.score = 0;
                //    stick.gameObject.SetActive(false);
                //    playerSpeed = 4;
                //}
            }
            //playerSpeed = 0;
            //_direction = (steed.transform.position - transform.position).normalized;
            //roastPercentageDisplay.enabled = true;
            //roastPercentageDisplay.gameObject.SetActive(true);
            //roastPercentage -= 50 * Time.deltaTime;
            //transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * 10);

            //if (roastPercentage <= 0)
            //{
            //    reticle.score = 0;
            //    stick.gameObject.SetActive(false);
            //    playerSpeed = 4;
            //}
        }

        if (feedTime < 0)
        {
            playerAnim.SetBool("Roasting", false);
            feeding = false;
            reticle.score = 0;
            stick.gameObject.SetActive(false);
            playerSpeed = 4;
        }
        if (roasting)
        {
            playerAnim.SetBool("Walking", false);
            playerAnim.SetBool("Roasting", true);
            _direction = (fire.transform.position - transform.position).normalized;
            roastTime -= Time.deltaTime;

            roastPercentageDisplay.gameObject.SetActive(true);
            if (roastPercentage < 0)
            {
                roastPercentageDisplay.gameObject.SetActive(false);
            }

            if (roastTime > 0)
            {
                roastPercentageDisplay.gameObject.SetActive(true);
                bg.SetActive(true);
                target.gameObject.SetActive(true);
                reticle.gameObject.SetActive(true);
                transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * 10);
                //roastPercentage += 20 * Time.deltaTime;
                playerSpeed = 0;
            }
            else
            {
                if (reticle.rectTrans != null)
                {
                    reticle.rectTrans.anchoredPosition = reticle.beginningPos.anchoredPosition;
                }
                roasting = false;
                bg.SetActive(false);
                target.gameObject.SetActive(false);
                reticle.gameObject.SetActive(false);
                playerSpeed = 4;
                roastPercentageDisplay.gameObject.SetActive(false);
                if (!showedResult)
                {

                    resultText.SetActive(true);
                    resultTextAnim.SetTrigger("Play");
                    StartCoroutine(hideAnim());

                }
            }
            //if (roastPercentage < 100)
            //{
            //    roastPercentageDisplay.enabled = true;
            //    bg.SetActive(true);
            //    target.gameObject.SetActive(true);
            //    reticle.gameObject.SetActive(true);
            //    transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * 10);
            //    roastPercentage += 20 * Time.deltaTime;
            //    playerSpeed = 0;
            //}
        }
        //else
        //{
        //    if(reticle.rectTrans != null)
        //    {
        //        reticle.rectTrans.anchoredPosition = reticle.beginningPos.anchoredPosition;
        //    }

        //    bg.SetActive(false);
        //    target.gameObject.SetActive(false);
        //    reticle.gameObject.SetActive(false);
        //    playerSpeed = 4;
        //}


        //if(roastPercentage >= 100)
        //{
        //    if(!showedResult)
        //    {
        //        resultTextAnim.SetTrigger("Play");
        //        resultText.SetActive(true);
        //        StartCoroutine(hideAnim());

        //    }

        //    roasting = false;
        //}
        if (roastPercentage <= 0)
        {
            //roastPercentageDisplay.gameObject.SetActive(false);
            //feeding = false;
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
