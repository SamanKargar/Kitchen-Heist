using System;
using System.Collections.Generic;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace _Game.Scripts.Behavior.Actions {
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "SetNextWaypoint", story: "Set Next [Waypoint] From [PatrolPointsList]", category: "Action", id: "a426d07a1eeb65cd971703b9c06f4228")]
    public class SetNextWaypointAction : Action
    {
        [SerializeReference] public BlackboardVariable<GameObject> Waypoint;
        [SerializeReference] public BlackboardVariable<List<GameObject>> PatrolPointsList;

        private int _waypointIndex;
        private bool _isMovingForward;

        protected override Status OnStart()
        {
            if (PatrolPointsList.Value.Count == 0) {
                Debug.LogError("No patrol points set inside the PatrolPointsList!");
                return Status.Failure;
            }

            if (Waypoint.Value == null) {
                Waypoint.Value = PatrolPointsList.Value[0];
            }
            
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            if (PatrolPointsList.Value == null || PatrolPointsList.Value.Count == 0)
                return Status.Failure;

            if (PatrolPointsList.Value.Count == 1)
            {
                Waypoint.Value = PatrolPointsList.Value[0];
                return Status.Success;
            }

            _waypointIndex = Mathf.Clamp(_waypointIndex, 0, PatrolPointsList.Value.Count - 1);
            Waypoint.Value = PatrolPointsList.Value[_waypointIndex];

            if (_isMovingForward)
            {
                _waypointIndex++;
                if (_waypointIndex >= PatrolPointsList.Value.Count)
                {
                    _waypointIndex = PatrolPointsList.Value.Count - 2;
                    _isMovingForward = false;
                }
            }
            else
            {
                _waypointIndex--;
                if (_waypointIndex < 0)
                {
                    _waypointIndex = 1;
                    _isMovingForward = true;
                }
            }
            
            return Status.Success;
        }

        protected override void OnEnd()
        {
        }
    }
}

