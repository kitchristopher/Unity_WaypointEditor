using System;
using System.Collections.Generic;
using UnityEngine;

namespace Waypoints
{
    public class Waypoints_Creator : MonoBehaviour
    {
        [Tooltip("This is the Resouces Path and name of the Waypoint Prefab.")] public string ResourcesPath = "Waypoint";

        [SerializeField] private List<Waypoint> waypoints = new List<Waypoint>();
        [SerializeField] private GameObject _waypoints_Holder = null;

        [SerializeField][Tooltip("If the pathway gizmos should be shown in editor.")]
        private bool _isDisplayingPathway = true; public bool IsShowingPathway { get { return _isDisplayingPathway; } }

        /// <summary>
        /// Creates a new waypoint at the end of the list
        /// </summary>
        /// <returns></returns>
        public Waypoint CreateWaypoint()
        {
            Waypoint waypointInstance = Instantiate(Resources.Load(ResourcesPath, typeof(Waypoint))) as Waypoint;

            if (waypoints.Count > 0)//Create a forward offset from the last waypoint and connect the waypoints
            {
                waypoints[waypoints.Count - 1].nextWaypoint = waypointInstance;
                waypoints[waypoints.Count - 1].isLooped = false;
                InitWaypoint(waypointInstance, waypoints[waypoints.Count - 1].transform);
            }
            else//spawn to the spawners transform
                InitWaypoint(waypointInstance, transform);

            waypoints.Add(waypointInstance);
            return waypointInstance;
        }

        /// <summary>
        /// Inserts a new waypoint after the input waypoint.
        /// </summary>
        /// <param name="waypoint"></param>
        /// <returns></returns>
        public Waypoint InsertWaypoint(Waypoint waypoint)
        {
            Waypoint waypointInstance = Instantiate(Resources.Load(ResourcesPath, typeof(Waypoint))) as Waypoint;
            int current_Index = waypoints.IndexOf(waypoint);

            InitWaypoint(waypointInstance, waypoint.transform);

            waypoints.Insert(current_Index + 1, waypointInstance);

            waypointInstance.nextWaypoint = waypoint.nextWaypoint;
            waypointInstance.isLooped = waypoint.isLooped;
            waypoint.isLooped = false;
            waypoint.nextWaypoint = waypointInstance;

            UpdateWaypointNames();

            return waypointInstance;
        }

        /// <summary>
        /// Sets the main variables of the new waypoint.
        /// </summary>
        /// <param name="waypointInstance"></param>
        /// <param name="nextTransform"></param>
        private void InitWaypoint(Waypoint waypointInstance, Transform nextTransform)
        {
            if (_waypoints_Holder == null)
            {
                _waypoints_Holder = new GameObject("Waypoint Holder");
                _waypoints_Holder.transform.parent = transform;
            }
 
            waypointInstance.transform.parent = _waypoints_Holder.transform;
            waypointInstance.name = "Waypoint " + waypoints.Count;
            waypointInstance.creator = this;

            waypointInstance.transform.position = nextTransform.position;
            waypointInstance.transform.rotation = nextTransform.rotation;
            waypointInstance.transform.position += waypointInstance.transform.forward;
        }

        /// <summary>
        /// Ensures the waypoints are numbered in order
        /// </summary>
        public void UpdateWaypointNames()
        {
            for (int i = 0; i < waypoints.Count; i++)
            {
                waypoints[i].name = "Waypoint " + i;

                if (waypoints[i].IsBranchingPathway())
                    waypoints[i].name = "Branching " + waypoints[i].name;
            }
        }

        public void RemoveWaypointFromList(Waypoint waypoint)
        {
            waypoints.Remove(waypoint);
            UpdateWaypointNames();
        }

        /// <summary>
        /// Deletes the Last waypoint stored
        /// </summary>
        public void DeleteWaypoint()
        {
            DestroyImmediate(waypoints[waypoints.Count - 1].gameObject);
        }

        /// <summary>
        /// Fixes the waypoints links by linking the previous waypoint to the input Waypoint's next link.
        /// </summary>
        /// <param name="waypoint"></param>
        public void FixWaypointLinks(Waypoint waypoint)
        {
            int previous_waypointIndex = waypoints.IndexOf(waypoint) - 1;

            if (previous_waypointIndex >= 0)
                waypoints[previous_waypointIndex].nextWaypoint = waypoint.nextWaypoint;
        }

        /// <summary>
        /// Destroys all of the waypoints
        /// </summary>
        public void ClearWaypoints()
        {
            for (int i = waypoints.Count - 1; i >= 0; --i)
                DestroyImmediate(waypoints[i].gameObject);

            waypoints.Clear();
        }

        private void OnDrawGizmos()
        {
            if (!_isDisplayingPathway)
                return;

            if (waypoints.Count > 0)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, waypoints[0].transform.position);
            }

        }

        private void OnDestroy()
        {
            ClearWaypoints();
        }

        /// <summary>
        /// Returns the first waypoint in the list 
        /// </summary>
        /// <returns></returns>
        public Waypoint GetInitalWaypoint()
        {
            Waypoint initalWaypoint = null;

            if (waypoints.Count > 0)
                initalWaypoint = waypoints[0];
            else//no existing waypoints, so create and return a new one
                initalWaypoint = CreateWaypoint();


            return initalWaypoint;
        }

        public bool IsEmpty()
        {
            return waypoints.Count > 0 ? false : true;
        }


    }
}