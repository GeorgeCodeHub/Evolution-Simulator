using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSpeed : MonoBehaviour
{
    Text speedText;

    public static float time;
    public static bool exist = false;

    //Start is called before the first frame update
    void Start()
    {
        speedText = GetComponent<Text>();
        exist = true;
        time = 1.0f;
    }

    public void SpeedAndTextUpdate(float value)
    {
        //Update game speed
        Time.timeScale = (float)value;

        time = (float)value;

        float temp = (float)Math.Round(value, 2, MidpointRounding.ToEven);
        speedText.text = "x"+temp.ToString();
    }
}
