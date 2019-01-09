using UnityEngine;
 using System.Linq;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.25f;
    public float range = 50f;
    public int playerNumber;

    int otherNumber;
    int originalDamagePerShot;
    float originalTimeBetweenBullets;
    float originalRange;

    float timer;
    Ray shootRay = new Ray();
    RaycastHit shootHit;
    int shootableMask;
    ParticleSystem gunParticles;
    ParticleSystem.MainModule psmain;
    LineRenderer gunLine;
    AudioSource gunAudio;
    Light gunLight;
    float effectsDisplayTime = 0.2f;

    bool powerActive;
    int powerType;
    float powerTime;
    float basePowerTime = 10.0f;

    void Awake ()
    {
        originalDamagePerShot = damagePerShot;
        originalRange = range;
        originalTimeBetweenBullets = timeBetweenBullets;

        if (playerNumber == 1) otherNumber = 2;
        else otherNumber = 1;
        shootableMask = LayerMask.GetMask ("Shootable");
        gunParticles = GetComponent<ParticleSystem> ();
        psmain = gunParticles.main;
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

        if(powerActive)
            VerifyPowerUp();
    }


    public void DisableEffects ()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }

    public void SetPowerUp(int type)
    {
        LogManager.AddPlayerPowerPicked(playerNumber, type);
        // 0  vermelho + força no tiro
        // 1 azul + rapido
        // 2 amarelo sem parar
        powerActive = true;
        powerType = type;
 
        if (type == 0)
        {
            psmain.startColor = gunLine.material.color = gunLight.color = Color.red;
            damagePerShot = 70;
            powerTime = basePowerTime;
        }else if(type == 1)
        {
            psmain.startColor = gunLine.material.color = gunLight.color = Color.blue;
            timeBetweenBullets = 0.1f;
            powerTime = basePowerTime;
        }
        else if(type == 2)
        {
            psmain.startColor = gunLine.material.color = gunLight.color =  Color.white;
            range = 150.0f;
            powerTime = basePowerTime;
        }
    }

    public void VerifyPowerUp()
    {
        Color defaultLight = new Color((253.0f/255.0f), 1, (136.0f/255.0f));
        Color defaultLine = new Color(1, (232.0f/255.0f), (163.0f/255.0f));
        Color defaultParticle = new Color((249.0f/255.0f), (232.0f/255.0f), 0.0f);
        powerTime -= Time.deltaTime;
        if(powerTime <= 0)
        {
            powerActive = false;
            gunLight.color = defaultLight;
            gunLine.material.color = defaultLine;
            psmain.startColor = defaultParticle;
            damagePerShot = originalDamagePerShot;
            timeBetweenBullets = originalTimeBetweenBullets;
            range = originalRange;
        }
    }

    public bool PowerIsActive()
    {
        return powerActive;
    }

    void SimpleShoot ()
    {
        timer = 0f;
        bool hitSuccess = false;
        int whoHitted = 0;

        gunAudio.Play ();

        gunLight.enabled = true;

        gunParticles.Stop ();
        gunParticles.Play ();

        gunLine.enabled = true;
        gunLine.SetPosition (0, transform.position);

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if (powerActive == true && powerType == 2)
        {

            RaycastHit[] hits;
            hits = Physics.RaycastAll(shootRay, range, shootableMask);

            foreach (RaycastHit hit in hits.OrderBy(x => x.distance))
            {
                if (hit.collider.name == "Player " + otherNumber)
                {
                    PlayerHealth playerHealth = hit.collider.GetComponent<PlayerHealth>();
                    if (playerHealth != null && playerHealth.currentHealth > 0)
                    {
                        hitSuccess = true;
                        whoHitted = 3;

                        LogManager.playerDamageMadePerSecond[playerNumber - 1] += damagePerShot;
                        LogManager.AddPlayerDamageMade(playerNumber, whoHitted, damagePerShot);
                        LogManager.AddPlayerDamageLog(otherNumber, whoHitted, damagePerShot);
                        playerHealth.TakeDamage(damagePerShot);
                    }
                }
                else
                {
                    EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
                    if (enemyHealth != null)
                    {
                        hitSuccess = true;
                        whoHitted = enemyHealth.type;

                        LogManager.playerDamageMadePerSecond[playerNumber - 1] += damagePerShot;
                        LogManager.AddPlayerDamageMade(playerNumber, whoHitted, damagePerShot);
                        // LogManager.enemiesShootedPerSecond[playerNumber]++;
                        enemyHealth.TakeDamage(damagePerShot, hit.point, playerNumber);
                    }
                } 
            }
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
        else
        {
            if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
            {
                if (shootHit.collider.name == "Player " + otherNumber)
                {
                    PlayerHealth playerHealth = shootHit.collider.GetComponent<PlayerHealth>();
                    if (playerHealth != null && playerHealth.currentHealth > 0)
                    {
                        hitSuccess = true;
                        whoHitted = 3;

                        LogManager.playerDamageMadePerSecond[playerNumber - 1] += damagePerShot;
                        LogManager.AddPlayerDamageMade(playerNumber, whoHitted, damagePerShot);
                        LogManager.AddPlayerDamageLog(otherNumber, 3, damagePerShot);
                        playerHealth.TakeDamage(damagePerShot);
                    }
                }
                else
                {
                    EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();
                    if (enemyHealth != null)
                    {
                        hitSuccess = true;
                        whoHitted = enemyHealth.type;

                        LogManager.playerDamageMadePerSecond[playerNumber - 1]+= damagePerShot;
                        LogManager.AddPlayerDamageMade(playerNumber, whoHitted, damagePerShot);
                        // LogManager.enemiesShootedPerSecond[playerNumber]++;
                        enemyHealth.TakeDamage(damagePerShot, shootHit.point, playerNumber);
                    }
                }

                gunLine.SetPosition(1, shootHit.point);
            }
            else
            {
                gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
            }
        }
        if (hitSuccess)
        {
            LogManager.AddPlayerShootLog(playerNumber, 1);
            LogManager.playerHittedPerSecond[playerNumber - 1]++;
        }
        else
        {
            LogManager.AddPlayerShootLog(playerNumber, 0);
        }

        LogManager.playerShootPerSecond[playerNumber-1]++;
    }
}
