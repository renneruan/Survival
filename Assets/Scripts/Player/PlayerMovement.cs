using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;
    public int playerNumber;

    Vector3 movement;
    Animator anim;
    Rigidbody playerRigidbody;

    public float rotationSpeed = 8f;
    public float joystickDeadzone = 0.2f;

    float xAxis;
    float yAxis;

    void Awake ()
    {
        anim = GetComponent <Animator> ();
        playerRigidbody = GetComponent<Rigidbody> ();
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("LeftJoyHorizontal" + playerNumber);
        float v = Input.GetAxisRaw("LeftJoyVertical"+ playerNumber);

        Move(h, v);
        Turning();
        Animating(h, v);
    }

    void Move(float h, float v)
    {
        movement.Set(h, 0f, v);

        movement = movement.normalized * speed * Time.deltaTime;

        playerRigidbody.MovePosition(transform.position + movement);
    }

    void Turning()
    {
        /*
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, camRayLenght, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigidbody.MoveRotation(newRotation);
        }
        */
        

        if (Mathf.Abs(Input.GetAxis("RightJoyHorizontal" + playerNumber)) >= joystickDeadzone ||
            Mathf.Abs(Input.GetAxis("RightJoyVertical" + playerNumber)) >= joystickDeadzone)
        {
            xAxis = Input.GetAxis("RightJoyHorizontal" + playerNumber);
            yAxis = Input.GetAxis("RightJoyVertical" + playerNumber);
        }

        float joystickAngle = Mathf.Atan2(xAxis, yAxis) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.Euler(0, joystickAngle, 0), Time.deltaTime * rotationSpeed);

    }

    void Animating(float h, float v)
    {
        bool walking = h != 0f || v != 0f;
        anim.SetBool("IsWalking", walking);
    }
}
