using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowSliderValue : MonoBehaviour
{
    Text valueText;

    // Start is called before the first frame update
    void Start()
    {
        valueText = GetComponent<Text>();
    }

    public void TextUpdate(float value)
    {
        float temp = (float)Math.Round(value, 2, MidpointRounding.ToEven) *100;
        valueText.text = temp.ToString();
    }
}
