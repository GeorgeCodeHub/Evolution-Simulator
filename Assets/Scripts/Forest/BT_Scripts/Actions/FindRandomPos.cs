using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindRandomPos : Action
{
    public float _radius;
    public SharedVector3 _rndPos;

    public override TaskStatus OnUpdate()
    {
        _rndPos.Value = GenerateRandomPosAroundTransform(transform, _radius);
        if (_rndPos.Value == Vector3.negativeInfinity)
            return TaskStatus.Failure;

        return TaskStatus.Success;
    }

    Vector3 GenerateRandomPosAroundTransform(Transform origin, float radius)
    {
        int rndPosCounter = 0;
        while (rndPosCounter < 100)
        {
            Vector2 rndPosAdditive = Random.insideUnitCircle * radius;
            float rndX = Mathf.Clamp(origin.position.x + rndPosAdditive.x, -75, 75);
            float rndZ = Mathf.Clamp(origin.position.z + rndPosAdditive.y, -75, 75);

            Vector3 rndPos = new Vector3(rndX, 0, rndZ);

            Collider[] collidingObjects = Physics.OverlapSphere(rndPos, 1,
            ~(1 << LayerMask.NameToLayer("Ground")));
            if (collidingObjects.Length == 0)
                return rndPos;
            else
                rndPosCounter++;
        }
        return Vector3.negativeInfinity;
    }


}
