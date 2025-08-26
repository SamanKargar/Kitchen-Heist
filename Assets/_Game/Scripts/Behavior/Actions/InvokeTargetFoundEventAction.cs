using System;
using _Game.Scripts.Managers;
using Unity.Behavior;
using Unity.Properties;
using Action = Unity.Behavior.Action;

namespace _Game.Scripts.Behavior.Actions {
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "InvokeTargetFoundEvent", story: "Invokes TargetFound Event", category: "Action", id: "c163f6cf11df26764fc68ffd0efe5bed")]
    public class InvokeTargetFoundEventAction : Action
    {
        protected override Status OnStart()
        {
            GameEventsManager.Instance.BehaviorEvents.OnTargetFound();
            return Status.Success;
        }

        protected override Status OnUpdate()
        {
            return Status.Success;
        }

        protected override void OnEnd()
        {
        }
    }
}

