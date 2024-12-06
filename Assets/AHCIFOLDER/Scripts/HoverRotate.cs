using System.Collections;
using UnityEngine;

/// <summary>
/// Handles object rotation for a hover animation.
/// </summary>
public class HoverRotate : MonoBehaviour
{
    [Header("Visual Settings")]
    public Renderer objectRenderer;   // The renderer of the object to apply fade effects

    [Header("Rotation Settings")]
    public float rotationSpeed;       // Speed at which the object rotates

    /// <summary>
    /// Rotates the object continuously.
    /// </summary>

    /// <summary>
    /// Continuously rotates the object around its Z-axis.
    /// </summary>
    private void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime); // Rotate based on rotationSpeed
    }
}
