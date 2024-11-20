using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ReturnCubes : MonoBehaviour
{
    private Vector3 spawnPos;
    private Vector3 platePos;

    public GameObject plate;
    public float yRise;
    public string animTrigger;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        spawnPos = transform.position;
        platePos = plate.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Plate"))
        {
            transform.position = platePos;
            transform.rotation = Quaternion.identity;
            animator.SetTrigger(animTrigger);
        }
        else if (collision.gameObject.CompareTag("Floor"))
        {
            transform.position = spawnPos;
        }
    }

   
}
