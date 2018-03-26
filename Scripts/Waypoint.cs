using UnityEngine;

namespace Waypoints
{
    [ExecuteInEditMode()]
    public class Waypoint : MonoBehaviour
    {
        public Waypoint nextWaypoint = null;
        public Waypoints_Creator creator;//the creator that MADE this waypoint
        [SerializeField] private Waypoints_Creator branchingPath_Creator = null;//This is an optional creator that can be added to this waypoint. This will turn this waypoint into a branching pathway

        [Range(0, 100)] [Tooltip("The chance of the AI traveling down the branch.")] public float branchingChance = 50f;

        public bool isLooped = false;//Set via the editor, this allows the waypoint to link to an existing waypoint

        private void OnDrawGizmos()
        {
            if (!creator.IsShowingPathway)
                return;

            if (nextWaypoint != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, nextWaypoint.transform.position);
            }
            else
            {
                Gizmos.DrawWireSphere(transform.position, 0.35f);
                Gizmos.DrawLine(transform.position, transform.position + transform.forward);//for showing direction
            }

        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 0.35f);
            Gizmos.DrawLine(transform.position, transform.position + transform.forward);//for showing direction
        }

        public Waypoint GetNextWaypoint()
        {
            Random.InitState(System.DateTime.Now.Millisecond);

            Waypoint next_chosenWaypoint = nextWaypoint; //default state

            if (branchingPath_Creator != null && Random.Range(0, 101) <= branchingChance)//branching path
                next_chosenWaypoint = branchingPath_Creator.GetInitalWaypoint();

            return next_chosenWaypoint;
        }

        public bool IsBranchingPathway()
        {
            return branchingPath_Creator == null ? false : true;
        }


        public void CreateBranchingPathway()
        {
            if (!IsBranchingPathway())
            {
                branchingPath_Creator = gameObject.AddComponent<Waypoints_Creator>();
                branchingPath_Creator.ResourcesPath = creator.ResourcesPath;
                branchingPath_Creator.CreateWaypoint();
                creator.UpdateWaypointNames();//update the names in the root pathway
            }
        }

        public void RemoveBranchingPathway()
        {
            if (IsBranchingPathway())
            {
                if (gameObject.activeInHierarchy)//only destroy if the Waypoint is not being destroyed; otherwise the breanchingcreator is destroyed twice
                    DestroyImmediate(branchingPath_Creator);

                branchingPath_Creator.ClearWaypoints();
                creator.UpdateWaypointNames();//update the names in the root pathway
            }

        }

        public void LoopWaypoints()
        {
            isLooped = true;
        }

        private void OnDestroy()
        {
            RemoveBranchingPathway();
            creator.FixWaypointLinks(this);
            creator.RemoveWaypointFromList(this);
        }

    }
}