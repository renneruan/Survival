using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;
    public int playerNumber;

    Vector3 movement;
    Animator anim;
    Rigidbody playerRigidbody;
    int floorMask;
    float camRayLenght = 100f;

    public float rotationSpeed = 8f;
    public float joystickDeadzone = 0.2f;

    float xAxis;
    float yAxis;




    void Awake ()
    {
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent <Animator> ();
        playerRigidbody = GetComponent<Rigidbody> ();
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("LeftJoyHorizontal" + playerNumber);
        float v = Input.GetAxisRaw("LeftJoyVertical"+ playerNumber);

        Vector3 direction = new Vector3(Input.GetAxis("RightJoyHorizontal1") * speed *
            Time.deltaTime, Input.GetAxis("RightJoyVertical1") * speed * Time.deltaTime, 0.0f);
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
        

        if (Mathf.Abs(Input.GetAxis("RightJoyHorizontal1")) >= joystickDeadzone ||
            Mathf.Abs(Input.GetAxis("RightJoyVertical1")) >= joystickDeadzone)
        {
            xAxis = Input.GetAxis("RightJoyHorizontal1");
            yAxis = Input.GetAxis("RightJoyVertical1");
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
