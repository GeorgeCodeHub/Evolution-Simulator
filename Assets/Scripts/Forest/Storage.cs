using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Storage : MonoBehaviour
{
    public Text _meatAmt;

    private int _meat = 0;

    public int _Meat
    {
        get => _meat;
        set
        {
            _meat = value;
        }
    }
}
