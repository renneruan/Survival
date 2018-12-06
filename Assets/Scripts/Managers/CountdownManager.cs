using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownManager : MonoBehaviour {

    Text text;
    float timer = 4.5f;

    void Awake()
    {
        text = GetComponent<Text>();
    }


    // Update is called once per frame
    void Update () {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            gameObject.SetActive(false);
        }
        else if (timer < 1)
        {
            text.text = "SURVIVE!";
        }else if(timer < 2)
        {
            text.text = "1";
        }else if(timer < 3)
        {
            text.text = "2";
        }
		
	}
}
