using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Growable : MonoBehaviour
{
    public float _startScale = 0.1f;
    [SerializeField] public float _growDuration = 10;

    private Vector3 _targetScale;
    private Tween _tw;

    public float GrowProgress
    {
        get
        {
            if (_tw.active)
                return _tw.ElapsedDirectionalPercentage();
            else
                return 1;
        }
    }

    private void Awake()
    {
        _targetScale = transform.localScale;
        transform.localScale = _targetScale * _startScale;
    }

    void Start()
    {
        _tw = transform.DOScale(_targetScale, _growDuration);
    }

}
