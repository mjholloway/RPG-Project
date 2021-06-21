using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] float maxSpeed = 6f;
        [SerializeField] float maxPathLength = 40f;

        NavMeshAgent navAgent;

        private void Awake()
        {
            navAgent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public bool CanMoveTo(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
            if (!hasPath || path.status != NavMeshPathStatus.PathComplete || GetPathLength(path) > maxPathLength) { return false; }

            return true;
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navAgent.SetDestination(destination);
            navAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navAgent.isStopped = false;
        }

        private float GetPathLength(NavMeshPath path)
        {
            float totalDistance = 0;
            if (path.corners.Length < 2) { return totalDistance; }
            for (int corner = 1; corner < path.corners.Length; ++corner)
            {
                totalDistance += Vector3.Distance(path.corners[corner], path.corners[corner - 1]);
            }
            return totalDistance;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = navAgent.velocity;
            Vector3 localVeclocity = transform.InverseTransformDirection(velocity);
            float speed = localVeclocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }

        public void Cancel()
        {
            navAgent.isStopped = true;
        }

        public object CaptureState()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["position"] = new SerializableVector3(transform.position);
            data["rotation"] = new SerializableVector3(transform.eulerAngles);
            return data;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> data = (Dictionary<string, object>)state;
            navAgent.enabled = false;
            transform.position = ((SerializableVector3)data["position"]).ToVector();
            transform.eulerAngles = ((SerializableVector3)data["rotation"]).ToVector();
            navAgent.enabled = true;

        }
    }
}
