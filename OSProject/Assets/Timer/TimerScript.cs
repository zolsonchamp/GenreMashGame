using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public float timeLeft;
    public bool timerOn=false;
    public Text timerText;
    // Start is called before the first frame update
    void Start()
    {
        timerOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerOn)
        {
            if (timeLeft > 0)
            {
                timeLeft-=Time.deltaTime;
                updateTimer(timeLeft);
            }
            else
            {
                timeLeft = 0;
                timerOn = false;
                GameManager.Instance.timeUp = true;
            }
        }
    }
    void updateTimer(float currentTime)
    {
        currentTime += 1;
        float minutes=Mathf.FloorToInt(currentTime/60);
        float seconds=Mathf.FloorToInt(currentTime%60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
