using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour {
    public GameObject tutorial;

    int stage = 0;
    float timeLeft = 3.0f;
     
    void Update()
    {
        if (stage == 0)
        {
            if (Input.GetKey("escape"))
            {
                Application.Quit();
            }
            if (Input.anyKey)
            {
                stage++;
                tutorial.SetActive(true);
            }
        }
        if (stage == 1)
        {
            timeLeft -= Time.deltaTime;
            if (Input.GetKey("escape"))
            {
                stage--;
                tutorial.SetActive(false);
                timeLeft = 3.0f;
            }
            if (Input.anyKey && timeLeft <= 0)
            {
                SceneManager.LoadScene("Level 01");
            }
        }
    }
}
