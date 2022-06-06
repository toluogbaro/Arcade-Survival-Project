using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{

    public class SCR_Selector : SCR_Node
    {
        public SCR_Selector() : base() { }
        public SCR_Selector(List<SCR_Node> children) : base(children) { }

        public override NodeState Evaluate()
        {
            foreach (SCR_Node node in nodeChildren)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        continue;
                        

                    case NodeState.SUCCESS:
                        nodeState = NodeState.SUCCESS;
                        return nodeState;

                    case NodeState.RUNNING:
                        nodeState = NodeState.RUNNING;
                        return nodeState;

                    default:
                        continue;


                }
            }



            nodeState = NodeState.FAILURE;
            return nodeState;
        }
    }

}


