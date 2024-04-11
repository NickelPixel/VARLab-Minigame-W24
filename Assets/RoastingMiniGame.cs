using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class RoastingMiniGame : MonoBehaviour
{
    public float radius;
    public float speed;

    public GameObject panel;

    // The point we are going around in circles
    private Vector2 basestartpoint;

    // Destination of our current move
    private Vector2 destination;

    // Start of our current move
    public Vector2 start;
    public Vector2 beginning;
    // Current move's progress
    private float progress = 0.0f;

    // Use this for initialization
    void Start()
    {
        start = transform.position;
        beginning = panel.transform.localPosition;
        basestartpoint = transform.position;
        progress = 0.0f;

        PickNewRandomDestination();
    }

    // Update is called once per frame
    void Update()
    {
        bool reached = false;

        // Update our progress to our destination
        progress += speed * Time.deltaTime;

        // Check for the case when we overshoot or reach our destination
        if (progress >= 1f)
        {
            progress = 1f;
            reached = true;
        }

        // Update out position based on our start postion, destination and progress.
        transform.localPosition = (destination * progress) + start * (1 - progress);

        // If we have reached the destination, set it as the new start and pick a new random point. Reset the progress
        if (reached)
        {
            start = destination;
            PickNewRandomDestination();
            progress = 0.0f;
        }
    }

    void PickNewRandomDestination()
    {
        // We add basestartpoint to the mix so that is doesn't go around a circle in the middle of the scene.
        destination = Random.insideUnitCircle * radius + basestartpoint;
        speed = Random.Range(1.5f, 3.5f);
    }
}
