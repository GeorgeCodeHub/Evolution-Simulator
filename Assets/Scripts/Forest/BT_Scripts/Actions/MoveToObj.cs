using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToObj : Action
{
    private NavMeshAgent _navAgent;

    public SharedTransform _targetObj;

    public override void OnAwake()
    {
        _navAgent = GetComponent<NavMeshAgent>();
    }

    public override void OnStart()
    {
        if (Util.isInRange(transform.position, _targetObj.Value.position, 1f) == false)
        {
            _navAgent.SetDestination(_targetObj.Value.position);
            _navAgent.isStopped = false;

        }
    }

    public override TaskStatus OnUpdate()
    {
        if (_targetObj.Value == null)
            return TaskStatus.Failure;

        if (_navAgent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            _navAgent.SetDestination(_targetObj.Value.position);
            _navAgent.isStopped = false;
        }

        if (Util.isInRange(transform.position, _targetObj.Value.position, 1f))
        {
            _navAgent.isStopped = true;

            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}
