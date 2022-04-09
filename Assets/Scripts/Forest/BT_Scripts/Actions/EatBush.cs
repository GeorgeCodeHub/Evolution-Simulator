using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[TaskCategory("Chicken")]
public class EatBush : Action
{
    public SharedTransform _bushTransform;
    public float _eatDuration = 3;

    private Chicken _chicken;

    private Bush _bush;

    private float _timeStartedEating;

    public override void OnAwake()
    {
        _chicken = GetComponent<Chicken>();

    }

    public override void OnStart()
    {
        _bush = _bushTransform.Value.GetComponent<Bush>();
        _timeStartedEating = Time.time;
    }

    public override TaskStatus OnUpdate()
    {
        if (_bush == null)
        {
            return TaskStatus.Failure;
        }


        float amtEaten = Mathf.Clamp(Time.deltaTime / 5, 0, _bush._foodValue);
        _chicken._Fullness += amtEaten;
        _bush._foodValue -= amtEaten;

        return TaskStatus.Running;
    }
}
