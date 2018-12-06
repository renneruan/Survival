using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;

    Animator anim;
    GameObject player1, player2;
    PlayerHealth player1Health, player2Health;
    EnemyHealth enemyHealth;
    int playerInRange = 0;
    float timer;


    void Awake ()
    {
        player1 = GameObject.FindGameObjectWithTag("Player1");
        player1Health = player1.GetComponent<PlayerHealth>();
        player2 = GameObject.FindGameObjectWithTag("Player2");
        player2Health = player2.GetComponent<PlayerHealth>();

        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent <Animator> ();
    }


    void OnTriggerEnter (Collider other)
    {
        if(other.gameObject == player1)
        {
            playerInRange = 1;
        }else if(other.gameObject == player2)
        {
            playerInRange = 2;
        }
    }


    void OnTriggerExit (Collider other)
    {
        if(other.gameObject == player1 || other.gameObject == player2)
        {
            playerInRange = 0;
        }
    }


    void Update ()
    {
        timer += Time.deltaTime;

        if(timer >= timeBetweenAttacks && (playerInRange != 0) && enemyHealth.currentHealth > 0)
        {
            Attack (playerInRange);
        }

        if(player1Health.currentHealth <= 0 && player2Health.currentHealth <= 0)
        {
            anim.SetTrigger ("PlayerDead");
        }
    }


    void Attack (int playerNumber)
    {
        timer = 0f;

        if (playerNumber == 1)
        {
            if (player1Health.currentHealth > 0)
            {
                player1Health.TakeDamage(attackDamage);
            }
        } else if (playerNumber == 2)
        {
            if (player2Health.currentHealth > 0)
            {
                player2Health.TakeDamage(attackDamage);
            }
        }
    }
}
