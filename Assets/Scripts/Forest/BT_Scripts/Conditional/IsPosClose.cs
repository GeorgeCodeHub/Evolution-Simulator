using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IsPosClose : Conditional
{
    public SharedFloat _range = 1;
    public SharedVector3 _pos;

    public override TaskStatus OnUpdate()
    {
        return Vector3.Distance(transform.position, _pos.Value) <= _range.Value ? TaskStatus.Success : TaskStatus.Failure;
    }
}
