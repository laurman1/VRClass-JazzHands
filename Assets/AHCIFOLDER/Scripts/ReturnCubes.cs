using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ReturnCubes : MonoBehaviour
{
    private Vector3 spawnPos;
    private Vector3 platePos;
    private Quaternion spawnRotation;

    public GameObject plate;
    public float yRise;
    public string animTrigger;

    public Animator animator;
    public GameObject cubeSelf;

    public string correctPlate;

    public AudioSource wrongPlate;
    

    // Start is called before the first frame update
    void Start()
    {
        spawnRotation = transform.rotation;
        spawnPos = transform.position;
        platePos = plate.transform.position;
        animator.GetComponent<Animator>().enabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Plate"))
        {
            if (collision.gameObject.name == correctPlate)
            {
                
                
                
                

                Debug.Log("platehit");
                animator.GetComponent<Animator>().enabled = true;
                cubeSelf.GetComponent<XRGrabInteractable>().enabled = false;
                transform.position = platePos;

                transform.rotation = spawnRotation;
                animator.SetTrigger(animTrigger);

            }
            else
            {
                Debug.Log("SpawnPos");
                transform.position = spawnPos;
                wrongPlate.Play();
            }
        }
        else if (collision.gameObject.CompareTag("Floor"))
        {
            Debug.Log("Floorhit");
            transform.position = spawnPos;
        }
        
    }

    

}
