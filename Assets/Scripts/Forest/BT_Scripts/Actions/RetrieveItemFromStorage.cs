using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;


[TaskCategory("Villager")]
public class RetrieveItemFromStorage : Action
{
    public ItemTypes _itemToRetrieve;

    private Villager _villager;
    private Storage _storage;
    private NavMeshAgent _navAgent;
    private Animator _animator;

    public override void OnAwake()
    {
        _villager = GetComponent<Villager>();
        _navAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _storage = GameObject.FindObjectOfType<Storage>();
    }

    public override void OnStart()
    {
        _navAgent.SetDestination(_storage.transform.position);
        _navAgent.isStopped = false;
        _animator.SetBool("isWalking", true);
    }

    public override TaskStatus OnUpdate()
    {
        if (Util.isInRange(transform.position, _storage.transform.position, 6f))
        {
            _navAgent.isStopped = true;
            _animator.SetBool("isWalking", false);

            if (_itemToRetrieve == ItemTypes.Meat && _storage._Meat > 0)
            {
                _storage._Meat--;
                _villager._CarriedItem = ItemTypes.Meat;
            }
            else
                return TaskStatus.Failure;

            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }

}
