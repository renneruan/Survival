using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;
    public int playerNumber;
    public GameObject anotherPlayer;

    PlayerShooting shooting;

    Vector3 movement;
    Animator anim;
    Rigidbody playerRigidbody;
    PlayerHealth anotherPlayerHealth;

    public float rotationSpeed = 8f;
    public float joystickDeadzone = 0.2f;

    float xAxis;
    float yAxis;

    float timeRemaining;
    float timeToLog = 0.5f;

    void Awake ()
    {
        timeRemaining = timeToLog;
        anim = GetComponent <Animator> ();
        playerRigidbody = GetComponent<Rigidbody> ();
        anotherPlayerHealth = anotherPlayer.GetComponent<PlayerHealth>();
        shooting = GetComponentInChildren<PlayerShooting>();
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("LeftJoyHorizontal" + playerNumber);
        float v = Input.GetAxisRaw("LeftJoyVertical"+ playerNumber);

        Move(h, v);
        Turning();
        Animating(h, v);

        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0)
        {
            float distance = Vector3.Distance(this.transform.position, anotherPlayer.transform.position);
            LogManager.AddPlayerMovementLog(playerNumber, this.transform);
            LogManager.AddPlayerDistanceLog(playerNumber, distance);
            timeRemaining = timeToLog;
        }
    }

    void Move(float h, float v)
    {
        float difHorizontal;
        float difVertical;

        movement.Set(h, 0f, v);

        movement = movement.normalized * speed * Time.deltaTime;

        difHorizontal = Mathf.Abs((this.transform.position.x + movement.x) -
            anotherPlayer.transform.position.x);
        difVertical = Mathf.Abs((this.transform.position.z + movement.z) -
            anotherPlayer.transform.position.z);

        if (anotherPlayerHealth.currentHealth > 0) {
            if (difHorizontal > 20)
                movement.x = 0;
            if ( difVertical > 14)
                movement.z = 0;
        }

        playerRigidbody.MovePosition(transform.position + movement);
    }

    void Turning()
    {
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

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "PowerUp" && !shooting.PowerIsActive()
            )
        {
            Destroy(other.gameObject);
            shooting.SetPowerUp(other.gameObject.GetComponent<PowerUpScript>().getPowerType());
        }else if(other.gameObject.tag == "Enemy")
        {
            //LogManager.enemiesAroundPerSecond[playerNumber-1]++;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            //LogManager.enemiesAroundPerSecond[playerNumber-1]--;
        }
    }
}
