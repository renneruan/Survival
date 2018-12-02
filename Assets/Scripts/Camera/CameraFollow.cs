using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform t1, t2;
    Vector3 offset;
    float followTimeDelta = 0.8f;

    private void Start()
    {
        offset = new Vector3(1.0f, 10.0f, -15.0f);
    }

    private void FixedUpdate()
    {
        Debug.Log((t1.position - t2.position).magnitude);
        FixedCameraFollowSmooth(gameObject.GetComponent<Camera>(), t1, t2);
    }

    public void FixedCameraFollowSmooth(Camera cam, Transform t1, Transform t2)
    {
        Vector3 midpoint = (t1.position + t2.position) / 2f;
        float distance = (t1.position - t2.position).magnitude;

        Vector3 cameraDestination = (midpoint - cam.transform.forward * distance) + offset;
       
        cam.transform.position = Vector3.Slerp(cam.transform.position, cameraDestination, followTimeDelta);

        // Snap when close enough to prevent annoying slerp behavior
        if ((cameraDestination - cam.transform.position).magnitude <= 0.05f)
            cam.transform.position = cameraDestination;
    }

}
