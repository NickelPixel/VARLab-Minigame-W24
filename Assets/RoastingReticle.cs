using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RoastingReticle : MonoBehaviour
{
    public Sprite red;
    public Sprite orange;
    public Sprite green;
    public Image reticleImage;

    public PlayerController pc;
    [SerializeField]
    private Vector2 movementInput = Vector2.zero;
    private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;

    [SerializeField]
    private float reticleSpeed;

    public bool roasting;

    [SerializeField]
    public RectTransform beginningPos;
    [SerializeField]
    public RectTransform rectTrans;
    private RectTransform canvasRect;

    public float distance;

    public float score;

    public float finalScore; //this number will be feed to the horse

    public bool scoreCalculated; //mark if the final score has been calculated

    public float minScore = 0;
    public float maxScore = 100;

    // Start is called before the first frame update
    void Start()
    {
        rectTrans = GetComponent<RectTransform>();
        beginningPos.anchoredPosition = rectTrans.anchoredPosition;
        pc = GetComponentInParent<PlayerController>();
        canvasRect = pc.bg.GetComponent<RectTransform>();
    }

    public void OnRoastingMiniGame(InputAction.CallbackContext context)
    {

        movementInput = context.ReadValue<Vector2>();

    }

    // Update is called once per frame
    void Update()
    {
        
        pc.roastPercentage = Mathf.InverseLerp(minScore, maxScore, score) * 100;
        //pc.roastPercentage = (score - minScore) / (maxScore - minScore);
        distance = Vector3.Distance(rectTrans.transform.position, pc.target.gameObject.transform.position);
        currentInputVector = Vector2.SmoothDamp(currentInputVector, movementInput, ref smoothInputVelocity, reticleSpeed);

        if (distance < 0.15f)
        {
            reticleImage.sprite = green;
        }
        if (distance >= 0.15f && distance < 0.6)
        {
            reticleImage.sprite = orange;
        }
        if (distance >= 0.6f)
        {
            reticleImage.sprite = red;
        }

        if (pc.roasting)
        {
            if (distance < 0.15f && score <=100)
            {
                score += 1.5f / (distance / 2) * Time.deltaTime;
                finalScore = score;
            }
            if (distance >= 0.15f && distance < 0.6 && score <= 100)
            {
                score += 0.5f / (distance / 2) * Time.deltaTime;
                finalScore = score;
            }
            if (distance >= 0.6f && score >= -100)
            {
                score -= 6 / (distance / 2) * Time.deltaTime;
                finalScore = score;
            }
            roasting = true;
            Vector3 move = new Vector3(movementInput.x, 0, movementInput.y);
            rectTrans.transform.position += new Vector3(currentInputVector.x, currentInputVector.y, 0) * Time.deltaTime;
        }

        if (rectTrans.anchoredPosition.x < -35)
        {
            rectTrans.anchoredPosition = new Vector3(-35, rectTrans.anchoredPosition.y, 0);
        }
        if (rectTrans.anchoredPosition.x > 34)
        {
            rectTrans.anchoredPosition = new Vector3(34, rectTrans.anchoredPosition.y, 0);
        }
        if (rectTrans.anchoredPosition.y < -35)
        {
            rectTrans.anchoredPosition = new Vector3(rectTrans.anchoredPosition.x, -35, 0);
        }
        if (rectTrans.anchoredPosition.y > 33.5f)
        {
            rectTrans.anchoredPosition = new Vector3(rectTrans.anchoredPosition.x, 33.5f, 0);
        }
    }
}
