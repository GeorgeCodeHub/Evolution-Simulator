using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompareFloats : Conditional
{
    public enum ComparisonType
    {
        GreaterEqual,
        Greater,
        Equal,
        Lesser,
        LesserEqual
    }

    public SharedFloat _valA;
    public ComparisonType _comparisonType;
    public SharedFloat _valB;


    public override TaskStatus OnUpdate()
    {
        switch(_comparisonType)
        {
            case ComparisonType.GreaterEqual:
                return _valA.Value >= _valB.Value ? TaskStatus.Success : TaskStatus.Failure;
            case ComparisonType.Greater:
                return _valA.Value > _valB.Value ? TaskStatus.Success : TaskStatus.Failure;
            case ComparisonType.Equal:
                return _valA.Value == _valB.Value ? TaskStatus.Success : TaskStatus.Failure;
            case ComparisonType.Lesser:
                return _valA.Value < _valB.Value ? TaskStatus.Success : TaskStatus.Failure;
            case ComparisonType.LesserEqual:
                return _valA.Value <= _valB.Value ? TaskStatus.Success : TaskStatus.Failure;
            default:
                return TaskStatus.Failure;
        }
    }
}
