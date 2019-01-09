using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    Transform player1, player2;
    PlayerHealth player1Health, player2Health;
    EnemyHealth enemyHealth;
    UnityEngine.AI.NavMeshAgent nav;

    float timeChangeTarget = 0.0f;
    float distanceToPlayer1 = 0.0f;
    float distanceToPlayer2 = 0.0f;

    bool around1, around2, chasing1, chasing2;

    int target;

    void Awake ()
    {
        around1 = false;
        around2 = false;
        chasing1 = false;
        chasing2 = false;
        player1 = GameObject.FindGameObjectWithTag("Player1").transform;
        player1Health = player1.GetComponent <PlayerHealth> ();
        player2 = GameObject.FindGameObjectWithTag("Player2").transform;
        player2Health = player2.GetComponent<PlayerHealth>();

        enemyHealth = GetComponent <EnemyHealth> ();
        nav = GetComponent <UnityEngine.AI.NavMeshAgent> ();
    }


    void Update ()
    {
        timeChangeTarget -= Time.deltaTime;
        if(timeChangeTarget < 0)
        {
            distanceToPlayer1 = (this.transform.position - player1.position).magnitude;
            distanceToPlayer2 = (this.transform.position - player2.position).magnitude;
            timeChangeTarget = 1.5f;
        }

        if((this.transform.position - player1.position).magnitude <= 6.0f){
        
            if (around1 == false)
            {
                LogManager.enemiesAroundPerSecond[0]++;
                around1 = true;
            }
        }
        else
        {
            if (around1 == true)
            {
                LogManager.enemiesAroundPerSecond[0]--;
                around1 = false;
            }
        }

        if ((this.transform.position - player2.position).magnitude <= 6.0f)
        {
            if (!around2)
            {
                LogManager.enemiesAroundPerSecond[1]++;
                around2 = true;
            }
        }
        else
        {
            if (around2 == true)
            {
                LogManager.enemiesAroundPerSecond[1]--;
                around2 = false;
            }
        }

        if(enemyHealth.currentHealth <= 0)
        {
            if (around1 == true)
            {
                LogManager.enemiesAroundPerSecond[0]--;
                around1 = false;
            }
            if (around2 == true)
            {
                LogManager.enemiesAroundPerSecond[1]--;
                around2 = false;
            }
        }


        if (enemyHealth.currentHealth > 0 
            && (player1Health.currentHealth > 0 || player2Health.currentHealth > 0))
        {
            if (player1Health.currentHealth > 0 && distanceToPlayer1 < distanceToPlayer2)
            {
                target = 1;
                nav.SetDestination(player1.position);
            }else if(player2Health.currentHealth > 0 && distanceToPlayer1 > distanceToPlayer2)
            {
                target = 2;
                nav.SetDestination(player2.position);
            }
            if (player1Health.currentHealth <= 0 && player2Health.currentHealth > 0){
                target = 2;
                nav.SetDestination(player2.position);
            }else if(player1Health.currentHealth > 0 && player2Health.currentHealth <= 0){
                target = 1;
                nav.SetDestination(player1.position);
            }

            if (target == 1)
            {
                if (chasing2)
                {
                    LogManager.enemiesChasingPerSecond[1]--;
                }
                if (!chasing1)
                {
                    LogManager.enemiesChasingPerSecond[0]++;
                }
                chasing1 = true;
                chasing2 = false;
            }
            else if (target == 2)
            {
                if (chasing1)
                {
                    LogManager.enemiesChasingPerSecond[0]--;
                }
                if (!chasing2)
                {
                    LogManager.enemiesChasingPerSecond[1]++;
                }
                chasing1 = false;
                chasing2 = true;
            }
        }
        else
        {
            if (chasing1)
            {
                LogManager.enemiesChasingPerSecond[0]--;
            }
            if (chasing2)
            {
                LogManager.enemiesChasingPerSecond[1]--;
            }
            chasing1 = false;
            chasing2 = false;
            nav.enabled = false;
        }
    }
}

