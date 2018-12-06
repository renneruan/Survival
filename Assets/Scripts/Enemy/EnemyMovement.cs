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

    void Awake ()
    {
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
        if(enemyHealth.currentHealth > 0 
            && (player1Health.currentHealth > 0 || player2Health.currentHealth > 0))
        {
            if (player1Health.currentHealth > 0 && distanceToPlayer1 < distanceToPlayer2)
            {
                nav.SetDestination(player1.position);
            }else if(player2Health.currentHealth > 0 && distanceToPlayer1 > distanceToPlayer2)
            {
                nav.SetDestination(player2.position);
            }
            if (player1Health.currentHealth <= 0 && player2Health.currentHealth > 0){
                nav.SetDestination(player2.position);
            }else if(player1Health.currentHealth > 0 && player2Health.currentHealth <= 0){
                nav.SetDestination(player1.position);
            }
        }
        else
        {
            nav.enabled = false;
        }
    }
}
