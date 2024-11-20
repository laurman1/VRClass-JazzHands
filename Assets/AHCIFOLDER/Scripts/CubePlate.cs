using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CubePlate : MonoBehaviour
{
    public Vector3 startPos;
    private Vector3 lerpTo;
    public float lerpSpeed;
    public GameObject plate;
    public GameObject hoverCubeLeft;
    public GameObject hoverCubeRight;
    public HoverRotate hoverScript;

    private bool shouldDrop = false;
    public float plateDrop;
    private float lerpProgress = 0f;
    public float t;

    public float waitTime;
    private Transform cube;

    public Renderer cubeRendererLeft;
    public Material startMaterialLeft;
    public Material lockedInMaterialLeft;
    public Material onPlateMaterialLeft;
    
    public Renderer cubeRendererRight;
    public Material startMaterialRight;
    public Material lockedInMaterialRight;
    public Material onPlateMaterialRight;

    public int leftOrRight;
    
    // Start is called before the first frame update
    void Start()
    {
        startPos = plate.transform.position;
        lerpTo = new Vector3(startPos.x, startPos.y-plateDrop, startPos.z);
        leftOrRight = 2;

    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Arm"))
        {
            cube = collision.transform;
            cube.SetParent(plate.transform);

            StartCoroutine(waitBeforeLerp());

            if (collision.gameObject.name == "CubeLeft")
            {
                leftOrRight = 0;
                cubeRendererLeft.material = onPlateMaterialLeft;
            }
            else if (collision.gameObject.name == "CubeRight")
            {   
                leftOrRight = 1;
                cubeRendererRight.material = onPlateMaterialRight;
            }
        }
        
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Arm"))
        {
            transform.position = startPos;
            shouldDrop = false;
            t = 0f;
            lerpProgress = 0f;

            if (leftOrRight == 0)
            {
                cubeRendererLeft.material = startMaterialLeft;
                hoverCubeLeft.SetActive(false);
            }
            else if (leftOrRight == 1)
            {
                cubeRendererRight.material = startMaterialRight;
                hoverCubeRight.SetActive(false);
            }
            leftOrRight = 2;
            
        }
    }


    private IEnumerator waitBeforeLerp()
    {
        yield return new WaitForSeconds(waitTime);
        shouldDrop = true;

    }

    void Update()
    {
        if (shouldDrop == true)
        {
            lerpProgress += lerpSpeed * Time.deltaTime;

            t = lerpProgress / lerpSpeed;

            t = Mathf.SmoothStep(0,1,t);

            
            transform.position = Vector3.Lerp(startPos, lerpTo, t);


            if (t >= 1f)
            {
                cube.SetParent(null);

                if (leftOrRight == 0)
                { 
                    cubeRendererLeft.material = lockedInMaterialLeft;
                    hoverCubeLeft.SetActive(true);
                }
                else if (leftOrRight == 1)
                {
                    cubeRendererRight.material = lockedInMaterialRight;
                    hoverCubeRight.SetActive(true);
                }
                
            }   
        }
        
    }
}
