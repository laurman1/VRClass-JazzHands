using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ReturnCubes : MonoBehaviour
{
    private Vector3 spawnPos;
    private Vector3 platePos;

    public GameObject plate;
    public float yRise;
    public string animTrigger;

    public Animator animator;
    public GameObject cubeSelf;

    public string correctPlate;

    // Start is called before the first frame update
    void Start()
    {
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
                animator.GetComponent<Animator>().enabled = true;
                cubeSelf.GetComponent<XRGrabInteractable>().enabled = false;
                transform.position = platePos;
                transform.rotation = Quaternion.identity;
                animator.SetTrigger(animTrigger);
            }
            else
            {
                transform.position = spawnPos;
            }
        }
        else if (collision.gameObject.CompareTag("Floor"))
        {
            transform.position = spawnPos;
        }
        
    }

   
}
