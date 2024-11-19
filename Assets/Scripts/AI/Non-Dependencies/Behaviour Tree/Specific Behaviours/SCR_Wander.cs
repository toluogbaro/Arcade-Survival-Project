using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
public class SCR_Wander : SCR_Node
{
    private Transform transform;
    private Transform[] wayPoints;

    public SCR_Wander(Transform _transform, Transform[] _wayPoints /*,Animator animator*/)
    {
        transform = _transform;
        wayPoints = _wayPoints;
    }

    public override NodeState Evaluate()
    {
        return base.Evaluate();
    }
}
