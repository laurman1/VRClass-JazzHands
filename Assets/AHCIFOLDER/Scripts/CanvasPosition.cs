using UnityEngine;
using TMPro;

/// <summary>
/// Positions a canvas relative to the scale cube and dynamically adjusts text size and orientation based on the cube's scale and player's camera position.
/// </summary>
public class CanvasPosition : MonoBehaviour
{
    [Header("Target Settings")]
    public GameObject cube;                // Target cube to position the canvas relative to
    public TextMeshProUGUI text;           // Text element displayed on the canvas

    [Header("Player Camera")]
    public Transform playerCamera;         // Reference to the player's camera to orient the canvas

    [Header("Font Settings")]
    public float fontScale = 10f;          // Scaling factor for font size based on cube size

    private float yOffset;                 // Vertical offset based on the cube's scale
    private Vector3 cubePos;               // Position of the target cube

    /// <summary>
    /// Updates the canvas position and rotation every frame.
    /// </summary>
    void Update()
    {
        // Calculate the vertical offset based on the cube's scale
        yOffset = cube.transform.localScale.x;

        // Adjust the font size dynamically based on the cube's scale
        text.fontSize = yOffset * fontScale;

        // Position the canvas above the cube
        cubePos = cube.transform.position;
        transform.position = new Vector3(cubePos.x, cubePos.y + yOffset, cubePos.z);

        // Rotate the canvas to face the player's camera
        Vector3 direction = playerCamera.position - transform.position;
        transform.rotation = Quaternion.LookRotation(-direction);
    }
}
