using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public int playerNumber;
    public static int score1;
    public static int score2;
    Text text;
    public GameObject finalScore;
    Text finalScoreText;

    void Awake ()
    {
        text = GetComponent <Text> ();
        finalScoreText = finalScore.GetComponent<Text>();

        score1 = score2 = 0;
    }


    void Update ()
    {
        if (playerNumber == 1)
        {
            text.text = "Score: " + score1;
        }
        else
        {
            text.text = "Score: " + score2;
        }
        finalScoreText.text = "Final Score\nPlayer 1: " + score1 + "\nPlayer 2: " +score2;
    }
}
