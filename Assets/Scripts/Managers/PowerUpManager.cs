using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour {
    public PlayerHealth player1Health, player2Health;
    public GameObject powerUp;
    public float spawnTime ;
    public Material[] m;
    public Transform[] spawnPoints;

    GameObject[] existentPowers;

    int powerUpType = 0;

    void Start()
    {
        InvokeRepeating("Spawn", 1, spawnTime);
    }

    bool verifyExistent()
    {
        foreach(GameObject power in existentPowers)
        {
            if (power.GetComponent<PowerUpScript>().getPowerType() == powerUpType)
            {
                return true;
            }
        }
        return false;
    }

    void Spawn () {
        if (player1Health.currentHealth <= 0f && player2Health.currentHealth <= 0f)
        {
            return;
        }
        existentPowers = GameObject.FindGameObjectsWithTag("PowerUp");

        if (!verifyExistent())
        {
            powerUp.GetComponent<MeshRenderer>().material =
                powerUp.GetComponent<ParticleSystemRenderer>().material = m[powerUpType];
            powerUp.GetComponent<PowerUpScript>().type = powerUpType;
            powerUp.GetComponent<Light>().color = m[powerUpType].color;
            Instantiate(powerUp, spawnPoints[powerUpType].position, spawnPoints[powerUpType].rotation);
            powerUpType++;
            powerUpType = powerUpType % 3;
        }
        else
        {
            powerUpType++;
            powerUpType = powerUpType % 3;
        }
    }

   
}
