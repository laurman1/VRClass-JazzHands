using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Controls the behavior of the plates that interact with cubes, including collision detection, material changes, and movement animations.
/// </summary>
public class CubePlate : MonoBehaviour
{
    [Header("Plate Settings")]
    public Transform plate;               // Transform of the plate
    public float plateDrop = 1f;          // Distance the plate drops upon collision
    public float lerpSpeed = 2f;          // Speed at which the plate moves
    private Vector3 startPos;             // Initial position of the plate
    private Vector3 lerpTo;               // Target position of the plate during the drop
    private float lerpProgress = 0f;      // Progress of the plate's movement
    private bool shouldDrop = false;      // Whether the plate should start moving

    [Header("Cubes and Materials")]
    public string cubeTag;                // Tag to identify valid cubes
    public string cube1Name;              // Name of the first cube
    public string cube2Name;              // Name of the second cube
    public Renderer cubeRenderer1;        // Renderer for the first cube
    public Renderer cubeRenderer2;        // Renderer for the second cube
    public Material startMaterial1;       // Initial material for the first cube
    public Material onPlateMaterial1;     // Material when the first cube is on the plate
    public Material lockedInMaterial1;    // Material when the first cube is locked in
    public Material startMaterial2;       // Initial material for the second cube
    public Material onPlateMaterial2;     // Material when the second cube is on the plate
    public Material lockedInMaterial2;    // Material when the second cube is locked in
    public int leftOrRight = 2;          // Tracks which cube (0 = cube1, 1 = cube2, 2 = none)
    public int exer1or2 = 2;            // Tracks which exercise (0 = cube1, 1 = cube2, 2 = none)

    [Header("Hover Effects")]
    public GameObject hoverCube1;         // Visual effect for the first cube
    public GameObject hoverCube2;         // Visual effect for the second cube

    [Header("Cube Scripts")]
    public ReturnCubes cubeScript1;       // Script for cube 1 behavior
    public ReturnCubes cubeScript2;       // Script for cube 2 behavior

    [Header("Audio Settings")]
    public float audioDelay = 0.5f;       // Delay for audio playback
    public AudioSource lockedIn;          // Audio source for locking in a cube
    public AudioSource spin;              // Audio source for spinning animation
    public AudioSource cubeRemoved;       // Audio source for cube removal

    [Header("Lighting")]
    public GameObject spotLight;          // Spotlight to indicate active plate

    [Header("Timing")]
    public float waitTime = 1f;           // Wait time before the plate starts dropping

    // Initializes the plate's start and target positions
    void Start()
    {
        startPos = plate.transform.position;
        lerpTo = new Vector3(startPos.x, startPos.y - plateDrop, startPos.z);
    }

    /// <summary>
    /// Handles behavior when a cube collides with the plate.
    /// </summary>
    public void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag(cubeTag)) return;

        Debug.Log("Collision detected.");
        spin.Play();
        lockedIn.PlayDelayed(audioDelay);

        StartCoroutine(WaitBeforeLerp());

        // Determine which cube collided and set its material
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
    }

    /// <summary>
    /// Handles behavior when a cube exits the plate.
    /// </summary>
    public void OnCollisionExit(Collision collision)
    {
        if (!collision.gameObject.CompareTag(cubeTag)) return;

        Debug.Log("Collision exit detected.");
        cubeRemoved.Play();
        spotLight.SetActive(false);

        // Reset plate and hover effects
        transform.position = startPos;
        shouldDrop = false;
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

        leftOrRight = 2; // Reset state
    }

    /// <summary>
    /// Waits before starting the plate drop animation.
    /// </summary>
    private IEnumerator WaitBeforeLerp()
    {
        yield return new WaitForSeconds(waitTime);
        shouldDrop = true;
    }

    // Updates the plate's movement and hover effects
    void Update()
    {
        if (!shouldDrop) return;

        lerpProgress += lerpSpeed * Time.deltaTime;
        float t = Mathf.SmoothStep(0, 1, lerpProgress / lerpSpeed);
        transform.position = Vector3.Lerp(startPos, lerpTo, t);

        if (t >= 1f)
        {
            // Enable hover effects and lock cube materials
            spotLight.SetActive(true);

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
