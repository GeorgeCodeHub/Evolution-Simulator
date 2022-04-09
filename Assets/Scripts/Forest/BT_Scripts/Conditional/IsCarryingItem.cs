
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[TaskCategory("Villager")]
public class IsCarryingItem : Conditional
{
    public ItemTypes _itemToCompare;

    public override TaskStatus OnUpdate()
    {
        ItemTypes carriedItem = GetComponent<Villager>()._CarriedItem;

        if (carriedItem == _itemToCompare)
        {
            return TaskStatus.Success;
        }
        else
            return TaskStatus.Failure;
    }

}
