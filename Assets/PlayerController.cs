using UnityEngine.InputSystem;
using UnityEngine;
using TMPro;
using static UnityEngine.GraphicsBuffer;

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

    private Vector2 movementInput = Vector2.zero;

    public bool nearCampfire;
    public bool nearTable;
    public bool nearSteed;
    public bool carrying;
    public bool feeding;

    public float roastTime;
    public bool roasting;

    public float roastPercentage;
    public TextMeshProUGUI roastPercentageDisplay;

    public GameObject fire;

    private Vector3 _direction;
    private Quaternion _lookRotation;

    public GameObject stick;

    private void Start()
    {
        fire = GameObject.FindGameObjectWithTag("Fire");
        roastPercentageDisplay = GetComponentInChildren<TextMeshProUGUI>();
        roastPercentage = 0;
        roastPercentageDisplay.enabled = false;
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
        if(nearCampfire)
        {
            roasting = true;
            roastPercentageDisplay.enabled = true;
            Debug.Log("Roasting...");
        }
        if(context.canceled)
        {
            roastPercentageDisplay.enabled = false;
            roasting = false;
            Debug.Log("Done Roasting!");
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if(nearTable)
        {
            if(!carrying)
            {
                stick.gameObject.SetActive(true);
                carrying = true;
            }
        }
        if(nearSteed)
        {
            if(carrying)
            {
                carrying = false;
                roastPercentageDisplay.enabled = true;
                feeding = true;
            }
        }
    }

    void Update()
    {
        _direction = (fire.transform.position - transform.position).normalized;
        _lookRotation = Quaternion.LookRotation(_direction);
        roastPercentageDisplay.text = roastPercentage.ToString("F0") + "%";
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(movementInput.x, 0, movementInput.y);
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero && !roasting)
        {
            gameObject.transform.forward += move;
        }

        // Changes the height position of the player..
        //if (Input.GetButtonDown("Jump") && groundedPlayer)
        //{
        //    playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        //}

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
        
        if(feeding)
        {
            roastPercentage -= 50 * Time.deltaTime;
            if(roastPercentage <= 0)
            {
                stick.gameObject.SetActive(false);
            }
        }
        if(roasting)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * 10);
            roastPercentage += 10 * Time.deltaTime;
            playerSpeed = 0;
        }
        else
        {
            playerSpeed = 4;
        }

        if(roastPercentage >= 100)
        {
            roasting = false;
        }
        if(roastPercentage <= 0)
        {
            roastPercentageDisplay.enabled = false;
            feeding = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Campfire"))
        {
            nearCampfire = true;
        }
        if(other.gameObject.CompareTag("Table"))
        {
            nearTable = true;
        }
        if(other.gameObject.CompareTag("Steed"))
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