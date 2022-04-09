using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{

    public float _foodValue = 0.5f;
    public event Action<Bush> onDie;

    private void Update()
    {
        if (_foodValue <= 0)
            Destroy(gameObject);
    }

    private void OnDestroy()
    {

        if (onDie != null)
            onDie(this);
        transform.DOKill();
        Destroy(gameObject);
    }
}
