using System;
using _Game.Scripts.Characters.AICharacter;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace _Game.Scripts.Behavior.Actions {
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "Is Target Detected", story: "Has [Agent] [DetectedTarget]?", 
        category: "Action", id: "c9fcfd1e597c48a8d15c67b019b1d0c5")]
    public class IsTargetDetectedAction : Action {
        [SerializeReference] public BlackboardVariable<GameObject> Agent;
        [SerializeReference] public BlackboardVariable<bool> DetectedTarget;

        private AISensor _sensor;
        private GameObject _target;

        protected override Status OnStart()
        {
            if (Agent.Value == null) {
                LogFailure("Agent is not assigned!");
                return Status.Failure;
            }
            
            _sensor = Agent.Value.GetComponent<AISensor>();
            if ( _sensor == null) {
                LogFailure("Sensor is null");
                return Status.Failure;
            }
            
            return Status.Running;
        }

        protected override Status OnUpdate() {
            _target = _sensor.GetDetectedTarget();
            DetectedTarget.Value = _target != null;
            
            return Status.Running;
        }

        protected override void OnEnd()
        {
        }
    }
}

