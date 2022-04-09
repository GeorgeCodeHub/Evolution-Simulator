using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[TaskCategory("Villager")]
public class HuntChicken : Action
{
    public SharedTransform _chickenTransform;

    private Villager _villager;
    private NavMeshAgent _navAgent;
    private Animator _animator;
    private float _timeSlaughterStarted;
    public override void OnAwake()
    {
        _villager = GetComponent<Villager>();
        _navAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    public override void OnStart()
    {
        _navAgent.SetDestination(_chickenTransform.Value.position);
        _navAgent.isStopped = false;
        _animator.SetBool("isWalking", true);
        _timeSlaughterStarted = float.MaxValue;
    }

    public override TaskStatus OnUpdate()
    {
        if (_chickenTransform.Value == null)//Chicken died before I could catch it.
            return TaskStatus.Failure;

        _navAgent.SetDestination(_chickenTransform.Value.position);
        //If Chicken is caught -> Start Slaughtering it
        if (Util.isInRange(transform.position, _chickenTransform.Value.position, 1f) && _timeSlaughterStarted == float.MaxValue)
        {
            _timeSlaughterStarted = Time.time;

            _navAgent.isStopped = true;
            _chickenTransform.Value.GetComponent<NavMeshAgent>().isStopped = true;//Stop the chicken
            BehaviorTree chickenBehav = _chickenTransform.Value.GetComponent<BehaviorTree>();
            if (chickenBehav != null)
                chickenBehav.DisableBehavior();
            _chickenTransform.Value.GetComponent<Chicken>()._IsBurningEnergy = false;
            _animator.SetBool("isWalking", false);
            _animator.SetBool("isChopping", true);
        }
        //finished slaughtering
        else if (Time.time > _timeSlaughterStarted + 5)
        {
            _animator.SetBool("isChopping", false);
            _villager._CarriedItem = ItemTypes.Meat;
            GameObject.Destroy(_chickenTransform.Value.gameObject);
            return TaskStatus.Success;
        }
        //currently slaughtering
        else if (_timeSlaughterStarted != float.MaxValue)
        {
            transform.LookAt(_chickenTransform.Value);
        }

        return TaskStatus.Running;
    }

    void SlaughterChicken(Transform _chickenTransform)
    {

    }
}
