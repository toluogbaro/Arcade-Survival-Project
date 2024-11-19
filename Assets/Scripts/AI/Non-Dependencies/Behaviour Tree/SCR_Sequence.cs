using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class SCR_Sequence : SCR_Node
    {
        public SCR_Sequence() : base() { }
        public SCR_Sequence(List<SCR_Node> children) : base(children) { }

        public override NodeState Evaluate()
        {
            bool anyChildRunning = false;

            foreach(SCR_Node node in nodeChildren)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        nodeState = NodeState.FAILURE;
                        return nodeState;

                    case NodeState.SUCCESS:
                        continue;

                    case NodeState.RUNNING:
                        anyChildRunning = true;
                        continue;

                    default:
                        nodeState = NodeState.SUCCESS;
                        return nodeState;
                
                
                }
            }



            nodeState = anyChildRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return nodeState;
        }
    }
}

