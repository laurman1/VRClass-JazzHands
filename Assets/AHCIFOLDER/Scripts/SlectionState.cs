using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

public class SlectionState : MonoBehaviour
{
    // Text elements for displaying UI information
    public TextMeshProUGUI armsText;      // Displays the selected arm (Left/Right/None)
    public TextMeshProUGUI exerText;      // Displays the selected exercise (1/2/None)
    public TextMeshProUGUI angleText;     // Displays the selected angle value
    public TextMeshProUGUI repsText;      // Displays the selected reps value

    // Cube and platform references for the angle and reps
    public GameObject angleCube;          // Cube that represents the angle scale
    private Vector3 AnglesCubeScale;      // Cube's scale to adjust angle
    public GameObject anglePlatform;      // Platform to adjust based on angle cube scale
    private float angleScale;             // Current scale of the angle
    private float angle;                  // Calculated angle value
    public int anglesInt;                 // Integer representation of angle
    private string anglesString = "Selected Angle: "; // Label for angle UI
    public TextMeshProUGUI smallAngleText;  // Text to display small angle value

    public GameObject repsCube;           // Cube that represents reps scale
    private Vector3 RepsCubeScale;        // Cube's scale to adjust reps
    public GameObject repsPlatform;       // Platform to adjust based on reps cube scale
    private float repsScale;              // Current scale of the reps
    private float reps;                   // Calculated reps value
    public int repsInt;                   // Integer representation of reps
    private string repsString = "Selected Reps: "; // Label for reps UI
    public TextMeshProUGUI smallRepsText;  // Text to display small reps value

    // Plate scripts for left/right arm and exercises
    public CubePlate armsPlateScript;     // Reference to the arms plate script
    public CubePlate exerPlateScript;     // Reference to the exercise plate script

    // Audio sources for canvas animations
    public AudioSource canvasOpen;        // Audio when the canvas opens
    public AudioSource canvasClose;       // Audio when the canvas closes
    public AudioSource buttonLightSound;  // Audio for button light activation (unused)

    // Hover cubes and canvas objects for interaction
    public GameObject hoverCubeR;         // Right hover cube
    public GameObject hoverCubeL;         // Left hover cube
    public GameObject hoverCube2;         // Another hover cube (unused)
    public GameObject hoverCube1;         // Another hover cube (unused)
    public GameObject hoverCubeReps;      // Hover cube for reps
    public GameObject hoverCubeAngle;     // Hover cube for angle
    public GameObject buttonLight;        // Button light for interaction feedback
    public GameObject canvas;             // The canvas for the selection UI

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        // Initialization if needed (currently empty)
    }

    /// <summary>
    /// Update is called once per frame to update UI and interactions
    /// </summary>
    void Update()
    {
        // Update selected arm based on plate selection
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

        // Update selected exercise based on plate selection
        if (exerPlateScript.exer1or2 == 0)
        {
            exerText.text = "Selected Exercise: 1";
        }
        else if (exerPlateScript.exer1or2 == 1)
        {
            exerText.text = "Selected Exercise: 2";
        }
        else
        {
            exerText.text = "Selected Exercise: None";
        }

        // Update and display angle value
        DisplayTMP(angleCube, angleScale, angle, anglesInt, angleText, smallAngleText, 0.05f, 0.2f, 45f, 90f, anglesString);
        ScalePlatform(AnglesCubeScale, angleCube, anglePlatform);

        // Update and display reps value
        DisplayTMP(repsCube, repsScale, reps, repsInt, repsText, smallRepsText, 0.05f, 0.2f, 5f, 15f, repsString);
        ScalePlatform(RepsCubeScale, repsCube, repsPlatform);

        // Manage button light visibility based on active cubes and canvas state
        if ((hoverCube1.activeSelf || hoverCube2.activeSelf) &&
            (hoverCubeL.activeSelf || hoverCubeR.activeSelf) &&
            hoverCubeReps.activeSelf && hoverCubeAngle.activeSelf)
        {
            buttonLight.SetActive(!canvas.activeSelf); // Toggle light if canvas is not active
        }
        else
        {
            buttonLight.SetActive(false); // Hide light if conditions are not met
        }
    }

    /// <summary>
    /// Scales the platform based on the cube's scale.
    /// </summary>
    Vector3 ScalePlatform(Vector3 cubeScale, GameObject cube, GameObject platform)
    {
        cubeScale = cube.transform.localScale;
        return platform.transform.localScale = new Vector3(cubeScale.x, platform.transform.localScale.y, cubeScale.z);
    }

    /// <summary>
    /// Updates the TextMeshProUGUI text based on the cube's scale and other parameters.
    /// </summary>
    TextMeshProUGUI DisplayTMP(GameObject cube, float scale, float value, int valueInt, TextMeshProUGUI text, TextMeshProUGUI smallText, float oldMin, float oldMax, float newMin, float newMax, string uiString)
    {
        scale = cube.transform.localScale.x;
        value = newMin + (scale - oldMin) * (newMax - newMin) / (oldMax - oldMin);
        valueInt = (int)value;
        text.text = (uiString + valueInt);
        smallText.text = (valueInt).ToString();
        return text;
    }

    /// <summary>
    /// Toggles the visibility of the canvas and plays corresponding audio.
    /// </summary>
    public void CanvasOnOff(GameObject canvas)
    {
        if (canvas.activeSelf)
        {
            canvas.SetActive(false);
            canvasClose.Play(); // Play close sound
        }
        else
        {
            canvas.SetActive(true);
            canvasOpen.Play(); // Play open sound
        }
    }
}
