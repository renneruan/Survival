using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;
    public int playerNumber;

    int otherNumber;

    float timer;
    Ray shootRay = new Ray();
    RaycastHit shootHit;
    int shootableMask;
    ParticleSystem gunParticles;
    LineRenderer gunLine;
    AudioSource gunAudio;
    Light gunLight;
    float effectsDisplayTime = 0.2f;

    void Awake ()
    {
        if (playerNumber == 1) otherNumber = 2;
        else otherNumber = 1;
        shootableMask = LayerMask.GetMask ("Shootable");
        gunParticles = GetComponent<ParticleSystem> ();
        gunLine = GetComponent <LineRenderer> ();
        gunAudio = GetComponent<AudioSource> ();
        gunLight = GetComponent<Light> ();
    }


    void Update ()
    {
        timer += Time.deltaTime;

		if(Input.GetAxisRaw("JoyFire" + playerNumber) > 0.1f && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            SimpleShoot ();
        }
        if(timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects ();
        }
    }


    public void DisableEffects ()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }


    void SimpleShoot ()
    {
        timer = 0f;

        gunAudio.Play ();

        gunLight.enabled = true;

        gunParticles.Stop ();
        gunParticles.Play ();

        gunLine.enabled = true;
        gunLine.SetPosition (0, transform.position);

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
        {
            Debug.Log(shootHit.collider.name + "\n" + "Player " + playerNumber + "\n" + (shootHit.collider.name == "Player " + playerNumber));
            if(shootHit.collider.name == "Player "+ otherNumber)
            {
                Debug.Log("Acertou");

                PlayerHealth playerHealth = shootHit.collider.GetComponent<PlayerHealth>();
                if (playerHealth != null && playerHealth.currentHealth > 0)
                {
                    playerHealth.TakeDamage(damagePerShot);
                    gunLine.SetPosition(1, shootHit.point);
                }
            }
            else
            {
                EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damagePerShot, shootHit.point, playerNumber);
                }
                gunLine.SetPosition(1, shootHit.point);
            }
        }
        else
        {
            gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
        }
    }
}
