using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ReturnCubes : MonoBehaviour
{
    
    private Vector3 spawnPos;  // Initial position of the cube when spawned
    private Vector3 platePos;  // Position of the plate where the cube should land
    private Quaternion spawnRotation;  // Initial rotation of the cube

    [Header("Plate and Cube Settings")]
    public GameObject plate;           // Reference to the plate object
    public float yRise;                // Amount the cube rises when placed on the plate (currently unused)
    public string animTrigger;         // Name of the animation trigger

    public Animator animator;          // Animator component for handling cube animations
    public GameObject cubeSelf;        // Reference to the cube itself

    [Header("Plate Interaction Settings")]
    public string correctPlate;        // Name of the correct plate for placing the cube

    public AudioSource wrongPlate;     // Audio source for playing sound when the wrong plate is hit

    /// <summary>
    /// Initializes the cube's position, rotation, and animator state.
    /// </summary>
    void Start()
    {
        // Store the initial position and rotation of the cube
        spawnRotation = transform.rotation;
        spawnPos = transform.position;
        platePos = plate.transform.position;

        // Disable animator at the start to prevent animation from playing prematurely
        animator.GetComponent<Animator>().enabled = false;
    }

    // Update method (empty in this case but can be used for future updates)
    void Update()
    {
        // Add update logic here if needed in the future
    }

    /// <summary>
    /// Handles collisions with other objects, including plates and the floor.
    /// </summary>
    /// <param name="collision">The collision event information.</param>
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the cube collides with a plate
        if (collision.gameObject.CompareTag("Plate"))
        {
            // If the cube is placed on the correct plate, trigger the associated animation
            if (collision.gameObject.name == correctPlate)
            {
                Debug.Log("Plate hit: Correct plate!");

                // Enable the animator to start animation
                animator.GetComponent<Animator>().enabled = true;

                // Disable grab interaction so the cube can't be moved anymore
                cubeSelf.GetComponent<XRGrabInteractable>().enabled = false;

                // Move the cube to the plate position and reset its rotation
                transform.position = platePos;
                transform.rotation = spawnRotation;

                // Trigger the specified animation for the cube
                animator.SetTrigger(animTrigger);
            }
            else
            {
                // If the wrong plate is hit, return the cube to its spawn position
                Debug.Log("Wrong plate hit, returning cube to spawn position.");

                // Reset the cube to its spawn position and play the wrong plate sound
                transform.position = spawnPos;
                wrongPlate.Play();
            }
        }
        else if (collision.gameObject.CompareTag("Floor"))
        {
            // If the cube falls to the floor, reset its position to spawn position
            Debug.Log("Cube hit the floor, returning to spawn position.");
            transform.position = spawnPos;
        }
    }
}
