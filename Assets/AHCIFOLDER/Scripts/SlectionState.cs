using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

public class SlectionState : MonoBehaviour
{
    public TextMeshProUGUI armsText;
    public TextMeshProUGUI exerText;
    public TextMeshProUGUI angleText;
    public TextMeshProUGUI repsText;

    public GameObject angleCube;
    private Vector3 AnglesCubeScale;
    public GameObject anglePlatform;
    private float angleScale;
    private float angle;
    public int anglesInt;
    private string anglesString = "Selected Angle: ";
    public TextMeshProUGUI smallAngleText;

    public GameObject repsCube;
    private Vector3 RepsCubeScale;
    public GameObject repsPlatform;
    private float repsScale;
    private float reps;
    public int repsInt;
    private string repsString = "Selected Reps: ";
    public TextMeshProUGUI smallRepsText;

    public CubePlate armsPlateScript;
    public CubePlate exerPlateScript;

    public AudioSource canvasOpen;
    public AudioSource canvasClose;

    public GameObject hoverCubeR;
    public GameObject hoverCubeL;
    public GameObject hoverCube2;
    public GameObject hoverCube1;
    public GameObject hoverCubeReps;
    public GameObject hoverCubeAngle;
    public GameObject buttonLight;
    public GameObject canvas;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (armsPlateScript.leftOrRight == 0)
        {
            armsText.text = "Selected Arm: Left";
        }
        else if (armsPlateScript.leftOrRight == 1)
        {
            armsText.text = "Selected Arm: Right";
        }
        else
        {
            armsText.text = "Selected Arm: None";
        }

        
        if (exerPlateScript.exer1or2 == 0)
        {
            exerText.text = "Selected Exericese: 1";
        }
        else if (exerPlateScript.exer1or2 == 1)
        {
            exerText.text = "Selected Exercise: 2";
        }
        else
        {
            exerText.text = "Selected Exercise: None";
        }
        //Debug.Log("exer: " + exerPlateScript.exer1or2);

        
        DisplayTMP(angleCube, angleScale, angle, anglesInt, angleText, smallAngleText, 0.05f, 0.2f, 45f, 90f, anglesString);
        ScalePlatform(AnglesCubeScale, angleCube, anglePlatform);       
        
        DisplayTMP(repsCube, repsScale, reps, repsInt, repsText, smallRepsText, 0.05f, 0.2f, 5f, 15f, repsString);    
        ScalePlatform(RepsCubeScale, repsCube, repsPlatform);

        /*if (hoverCube1.activeSelf || hoverCube2.activeSelf)
        {
            if (hoverCubeL.activeSelf || hoverCubeR.activeSelf)
            {
                if (hoverCubeReps.activeSelf && hoverCubeAngle.activeSelf)
                {
                    if (!canvas.activeSelf)
                    {
                        buttonLight.SetActive(true);
                    }
                    else
                    {
                        buttonLight.SetActive(false);
                    }
                }
                else
                {
                    buttonLight.SetActive(false);
                }
            }
            else
            {
                buttonLight.SetActive(false);
            }
        
        }
        else
        {
            buttonLight.SetActive(false);
        }*/
        if ((hoverCube1.activeSelf || hoverCube2.activeSelf) &&
        (hoverCubeL.activeSelf || hoverCubeR.activeSelf) &&
        hoverCubeReps.activeSelf && hoverCubeAngle.activeSelf)
        {
            buttonLight.SetActive(!canvas.activeSelf);
        }
        else
        {
            buttonLight.SetActive(false);
        }
    }

    
    Vector3 ScalePlatform(Vector3 cubeScale, GameObject cube, GameObject platform)
    {
        cubeScale = cube.transform.localScale;
        return platform.transform.localScale = new Vector3(cubeScale.x, platform.transform.localScale.y, cubeScale.z);
    }

    TextMeshProUGUI DisplayTMP(GameObject cube, float scale, float value, int valueInt, TextMeshProUGUI text, TextMeshProUGUI smallText, float oldMin, float oldMax, float newMin, float newMax, string uiString)
    {
        scale = cube.transform.localScale.x;
        value = newMin + (scale - oldMin) * (newMax - newMin) / (oldMax - oldMin);
        valueInt = (int)value;
        text.text = (uiString + valueInt);
        smallText.text = (valueInt).ToString();
        return text;
    }

    public void CanvasOnOff(GameObject canvas)
    {

         if (canvas.activeSelf)
         {
            canvas.SetActive(false);
            canvasClose.Play();
         }
         else
         {
            canvas.SetActive(true);
            canvasOpen.Play();

         }
    
    }

}
