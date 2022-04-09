using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatMeat : Action
{
    private Villager _villager;

    public override void OnAwake()
    {
        _villager = GetComponent<Villager>();
    }

    public override TaskStatus OnUpdate()
    {
        if (_villager._CarriedItem == ItemTypes.Meat)
        {
            _villager.EatMeat();
            return TaskStatus.Success;
        }
        else
            return TaskStatus.Failure;
    }
}
