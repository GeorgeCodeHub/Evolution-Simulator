using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IsObjectClose : Conditional
{
    public SharedFloat _range;
    public string _objectTag;

    public SharedTransform _closestObject;

    public override void OnAwake()
    {
    }

    public override TaskStatus OnUpdate()
    {
        Transform closestObject = Util.FindClosestObj(transform, _range.Value, _objectTag);
        if (closestObject != null)
        {
            _closestObject.Value = closestObject;
            return TaskStatus.Success;
        }
        else
            return TaskStatus.Failure;
    }
}
