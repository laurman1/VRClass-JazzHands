using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlectionState : MonoBehaviour
{
    public TextMeshProUGUI armsText;
    public TextMeshProUGUI exerText;
    public CubePlate armsPlateScript;
    public CubePlate exerPlateScript;

    public AudioSource canvasOpen;
    public AudioSource canvasClose;

    
    
    
    
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
