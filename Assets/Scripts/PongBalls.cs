using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongBalls : MonoBehaviour
{


    public AudioClip collisionSound; // Assign the sound clip in the Inspector
    private AudioSource audioSource;

    private void Start()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {


        if (other.CompareTag("Hit"))
        {

            GetComponent<Renderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            audioSource.PlayOneShot(collisionSound);
        }
    }


}
