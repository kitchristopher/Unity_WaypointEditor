using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Waypoints;

namespace Waypoints
{

    [RequireComponent(typeof(Rigidbody))]
    public class AI_WaypointFollower_Base : MonoBehaviour
    {
        [SerializeField] [Tooltip("When the agent has passed a waypoint and looking for the next.")] private UnityEvent OnPassedWaypoint;
        [SerializeField] [Tooltip("After the agent has followd the path to the end.")] private UnityEvent OnFinishedPath;
        [SerializeField] [Tooltip("The distance between the AI and the waypoint before it goes to the next one.")] private float _distanceThreshold;
        [SerializeField] private Waypoint _currentWaypoint;
        [SerializeField] private float _agentSpeed = 0.5f;
        [SerializeField] private float _agentTurnSpeed = 0.5f;

        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        void Start()
        {
            StartCoroutine(Follow_Path());
        }

        /// <summary>
        /// Translates the AI's rigidbody position by the forward axis
        /// </summary>
        protected void Waypoint_Translate()
        {
            Vector3 translatePosition = _rb.position + transform.forward * _agentSpeed;//move forward
            _rb.MovePosition(translatePosition);
        }

        /// <summary>
        /// Rotates the AI to face the current waypoint
        /// </summary>
        protected void Waypoint_Orient(Transform targetTransfrom)
        {
            Vector3 targetDirection = targetTransfrom.position - transform.position;
            Vector3 rotatePosition = Vector3.RotateTowards(transform.forward, targetDirection, _agentTurnSpeed * Time.deltaTime, 0f); //rotate z to face waypoint
            transform.rotation = Quaternion.LookRotation(rotatePosition);
        }

        private IEnumerator Follow_Path()
        {
            while (_currentWaypoint)
            {
                Waypoint_Orient(_currentWaypoint.transform);

                if (Vector3.Distance(transform.position, _currentWaypoint.transform.position) <= _distanceThreshold)//get the next waypoint
                {
                    _currentWaypoint = _currentWaypoint.GetNextWaypoint();
                    OnPassedWaypoint.Invoke();
                }
                else
                    Waypoint_Translate();

                yield return new WaitForFixedUpdate();
            }

            OnFinishedPath.Invoke();
        }

    }
}