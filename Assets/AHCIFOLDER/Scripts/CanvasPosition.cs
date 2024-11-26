using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasPosition : MonoBehaviour
{
    public GameObject cube;
    private Vector3 cubePos;
    private float yOffset;
    public TextMeshProUGUI text;

    public Transform playerCamera;

    public float fontScale;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        yOffset = cube.transform.localScale.x;
        text.fontSize = yOffset * fontScale;
        cubePos = cube.transform.position;
        transform.position = new Vector3(cubePos.x, cubePos.y + yOffset, cubePos.z);

        Vector3 direction = playerCamera.position - transform.position;
        transform.rotation = Quaternion.LookRotation(-direction);
    }
}
