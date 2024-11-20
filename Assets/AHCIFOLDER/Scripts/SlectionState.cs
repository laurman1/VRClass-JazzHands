using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlectionState : MonoBehaviour
{
    public TextMeshProUGUI armsText;
    public CubePlate plateScript;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (plateScript.leftOrRight == 0)
        {
            armsText.text = "Selected Arm: Left";
        }
        else if (plateScript.leftOrRight == 1)
        {
            armsText.text = "Selected Arm: Right";
        }
        else
        {
            armsText.text = "Selected Arm: None";
        }
        
    }
}
