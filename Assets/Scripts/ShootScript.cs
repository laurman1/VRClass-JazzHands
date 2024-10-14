using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShootScript : MonoBehaviour
{
    public GameObject bullet;
    public Transform spawnPos;

    [SerializeField] private float bulletSpeed = 10;

    // Start is called before the first frame update
    void Start()
    {
        XRBaseInteractable theGunGrabable = GetComponent<XRBaseInteractable>();
        theGunGrabable.activated.AddListener(bangBang);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void bangBang(ActivateEventArgs arg)
    {
        GameObject newBullet = Instantiate(bullet);
        newBullet.transform.position = spawnPos.position;
        newBullet.GetComponent<Rigidbody>().velocity = spawnPos.forward * bulletSpeed;

        Destroy(newBullet, 5);
    }
}
