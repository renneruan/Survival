using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public PlayerHealth player1Health;
    public PlayerHealth player2Health;

    Animator anim;
    


    void Awake()
    {
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        if (player1Health.currentHealth <= 0 && player2Health.currentHealth <= 0)
        {
            anim.SetTrigger("GameOver");
        }
    }
}
