using System.Collections.Generic;

namespace WUG.BehaviorTreeVisualizer
{

    public delegate void NodeStatusChangedEventHandler(NodeBase sender);
    public enum NodeStatus
    {
        FAILURE,
        SUCCESS,
        RUNNING, //evaluation incomplete
        UNKNOWN,
        NOT_RUN
    }
    public class NodeBase
    {
        public string Name { get; set; }
        public string StatusReason { get; set; } = "";
        public List<NodeBase> ChildNodes = new List<NodeBase>();
        public NodeStatus LastNodeStatus = NodeStatus.NOT_RUN;
        
        public event NodeStatusChangedEventHandler NodeStatusChanged;

        /// <summary>
        /// Handles invoking the NodeStatusChangedEventHandler delegate.
        /// </summary>
        protected virtual void OnNodeStatusChanged(NodeBase sender)
        {
            NodeStatusChanged?.Invoke(sender);
        }

    }
}