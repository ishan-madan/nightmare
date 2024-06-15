using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target; // target for the camera to follow
    public float smoothing = 5f; // want a little bit of lag to follow the player

    Vector3 offset; // store the camera offset


    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - target.position; // set offset between target and camera
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetCamPos = target.position + offset; // set position for camera to go to
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime); // smoothly moves from first parameter to second parametier in third parameter time
    }
}
