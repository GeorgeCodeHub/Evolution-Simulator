using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text speedText;
    public Slider _speedSlider;
    public static float time;

    private void Start()
    {
        time = Time.timeScale;
    }

    public void OnGameSpeedChanged()
    {
        Time.timeScale = _speedSlider.value;

        time = _speedSlider.value;

        float temp = (float)Math.Round(_speedSlider.value, 2, MidpointRounding.ToEven);
        speedText.text = "x" + temp.ToString();
    }

}
