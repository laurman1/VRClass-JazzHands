using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CubePlate : MonoBehaviour
{
    public Vector3 startPos;
    private Vector3 lerpTo;
    public float lerpSpeed;
    public Transform plate;
    public GameObject hoverCube1;
    public GameObject hoverCube2;
    //public HoverRotate hoverScript;

    public string cubeTag;
    public string cube1Name;
    public string cube2Name;

    private bool shouldDrop = false;
    public float plateDrop;
    private float lerpProgress = 0f;
    public float t;

    public float waitTime;
    private Transform cube;

    public Renderer cubeRenderer1;
    public Material startMaterial1;
    public Material lockedInMaterial1;
    public Material onPlateMaterial1;
    
    public Renderer cubeRenderer2;
    public Material startMaterial2;
    public Material lockedInMaterial2;
    public Material onPlateMaterial2;

    public int leftOrRight;
    public int exer1or2;

    public ReturnCubes cubeScript1;
    public ReturnCubes cubeScript2;

    public float audioDelay;
    public AudioSource lockedIn;
    public AudioSource spin;
    public AudioSource cubeRemoved;

    // Start is called before the first frame update
    void Start()
    {
        startPos = plate.transform.position;
        lerpTo = new Vector3(startPos.x, startPos.y-plateDrop, startPos.z);
        leftOrRight = 2;

    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision");
        if (collision.gameObject.CompareTag(cubeTag))
        {
            spin.Play();
            cube = collision.transform;
            lockedIn.PlayDelayed(audioDelay);

            StartCoroutine(waitBeforeLerp());

            if (collision.gameObject.name == cube1Name)
            {
                leftOrRight = 0;
                exer1or2 = 0;
                cubeRenderer1.material = onPlateMaterial1;
            }
            else if (collision.gameObject.name == cube2Name)
            {   
                leftOrRight = 1;
                exer1or2 = 1;
                cubeRenderer2.material = onPlateMaterial2;
            }
        else
            {
                return;
            }
        }
        
    }

    public void OnCollisionExit(Collision collision)
    {
        Debug.Log("collisionexit");

        if (collision.gameObject.CompareTag(cubeTag))
        {
            cubeRemoved.Play();

            cubeScript1.animator.GetComponent<Animator>().enabled = false;
            cubeScript2.animator.GetComponent<Animator>().enabled = false;
            
            transform.position = startPos;
            shouldDrop = false;
            t = 0f;
            lerpProgress = 0f;

            if (leftOrRight == 0)
            {
                cubeRenderer1.material = startMaterial1;
                hoverCube1.SetActive(false);
            }
            else if (leftOrRight == 1)
            {
                cubeRenderer2.material = startMaterial2;
                hoverCube2.SetActive(false);
            }
            leftOrRight = 2;
            exer1or2 = 2;
            
        }
    }
    

    


    private IEnumerator waitBeforeLerp()
    {
        yield return new WaitForSeconds(waitTime);
        shouldDrop = true;
        

        //cube.SetParent(plate);
    }

    void Update()
    {
        if (shouldDrop == true)
        {
            
            lerpProgress += lerpSpeed * Time.deltaTime;

            t = lerpProgress / lerpSpeed;

            t = Mathf.SmoothStep(0,1,t);

            
            transform.position = Vector3.Lerp(startPos, lerpTo, t);


            if (t == 1f)
            {

                
                cubeScript1.cubeSelf.GetComponent<XRGrabInteractable>().enabled = true;
                cubeScript2.cubeSelf.GetComponent<XRGrabInteractable>().enabled = true;
                
                //cubeScript1.cubeSelf.transform.SetParent(null);
                //cubeScript2.cubeSelf.transform.SetParent(null);


                if (leftOrRight == 0)
                {
                    
                    cubeRenderer1.material = lockedInMaterial1;
                    
                    hoverCube1.SetActive(true);
                    
                }
                else if (leftOrRight == 1)
                {
                    
                    cubeRenderer2.material = lockedInMaterial2;
                    hoverCube2.SetActive(true);
                    
                }
                
            }   
        }
         
    }
}
