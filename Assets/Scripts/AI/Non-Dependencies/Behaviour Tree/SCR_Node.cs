using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public enum NodeState {  RUNNING, SUCCESS, FAILURE }
    public class SCR_Node
    {
        protected NodeState nodeState;

        public SCR_Node nodeParent;
        protected List<SCR_Node> nodeChildren;

        private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

        public SCR_Node()
        {
            nodeParent = null;
        }
        public SCR_Node(List<SCR_Node> children)
        {
            foreach (SCR_Node child in children)
                _Attach(child);
        }

        private void _Attach(SCR_Node node)
        {
            node.nodeParent = this;
            nodeChildren.Add(node);
        }

        public virtual NodeState Evaluate() => NodeState.FAILURE;

        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }

        public object GetData(string key)

        {
            object value = null;
            if (_dataContext.TryGetValue(key, out value))
                return value;

            SCR_Node node = nodeParent;

            while(node != null)
            {
                value = node.GetData(key);
                if (value != null) return value;

                node = node.nodeParent;
            }

            return null;
        }

        public bool ClearData (string key)

        {

           if(_dataContext.ContainsKey(key))
            {
                _dataContext.Remove(key);
                return true;
            }


            SCR_Node node = nodeParent;

            while (node != null)
            {
                bool cleared = node.ClearData(key);
                if (cleared) return true;
                node = node.nodeParent;

                    
            }
            return false;

        }
    }
}


