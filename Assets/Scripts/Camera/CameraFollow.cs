using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject p1, p2;
    Transform t1, t2;
    PlayerHealth p1H, p2H;
    Vector3 offset;
    float followTimeDelta = 0.8f;

    private void Start()
    {
        t1 = p1.transform;
        t2 = p2.transform;
        p1H = p1.GetComponent<PlayerHealth>();
        p2H = p2.GetComponent<PlayerHealth>();
        offset = new Vector3(1.0f, 10.0f, -15.0f);
    }

    private void FixedUpdate()
    {
        Vector3 targetCamPos;
        if (p1H.currentHealth > 0 && p2H.currentHealth > 0)
        {
            FixedCameraFollowSmooth(gameObject.GetComponent<Camera>(), t1, t2);
        }
        else
        {
            if(p1H.currentHealth > 0)
            {
                if(((t1.position + offset) - transform.position).magnitude >= 1f)
                {
                    targetCamPos = transform.position + (((t1.position + offset) - transform.position).normalized);
                }
                else
                {
                    targetCamPos = t1.position + offset;
                }
                transform.position = Vector3.Lerp(transform.position, targetCamPos, followTimeDelta);
            }
            else if (p2H.currentHealth > 0)
            {
                if (((t2.position + offset) - transform.position).magnitude >= 1f)
                {
                    targetCamPos = transform.position + (((t2.position + offset) - transform.position).normalized);
                }
                else
                {
                    targetCamPos = t2.position + offset;
                }
                transform.position = Vector3.Lerp(transform.position, targetCamPos, followTimeDelta);
            }
        }
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
