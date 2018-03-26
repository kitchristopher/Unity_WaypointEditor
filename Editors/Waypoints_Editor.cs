using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Waypoints;

[CustomEditor(typeof(Waypoint)), CanEditMultipleObjects]
public class Waypoints_Editor : Editor
{

    public override void OnInspectorGUI()
    {
        Waypoint self = (Waypoint)target;

        if (!self.creator)
            return;

        if (self.IsBranchingPathway())
            GUILayout.Label("Main Branch Pathway");

        if (!self.isLooped && GUILayout.Button("Add Next Waypoint"))
            Selection.activeGameObject = self.creator.CreateWaypoint().gameObject;

        if (!self.isLooped && self.nextWaypoint == null && GUILayout.Button("Loop to Existing Waypoint"))
        {
            self.isLooped = true;
        }

        if (self.isLooped)
        {
            SerializedProperty loopedWaypoint_Property = serializedObject.FindProperty("nextWaypoint");
            EditorGUILayout.PropertyField(loopedWaypoint_Property, new GUIContent("Looping Waypoint"), GUILayout.Height(20));
        }

        if (self.isLooped && GUILayout.Button("Clear Loop to Existing Waypoint"))
        {
            self.isLooped = false;
            self.nextWaypoint = null;
        }

        if (self.nextWaypoint != null && GUILayout.Button("Insert New Waypoint"))//Cant Insert a waypoint if there if the current one isnot wedged between another
            Selection.activeGameObject = self.creator.InsertWaypoint(self).gameObject;

        if (GUILayout.Button("Delete This Waypoint"))
            DestroyImmediate(self.gameObject);

        if (!self.IsBranchingPathway() && GUILayout.Button("Create Branching Path"))
            self.CreateBranchingPathway();

        if (self.IsBranchingPathway())
        {
            SerializedProperty branchingChance_Property = serializedObject.FindProperty("branchingChance");
            EditorGUILayout.PropertyField(branchingChance_Property, new GUIContent("Branching Chance"), GUILayout.Height(20));
        }

        if (self.IsBranchingPathway() && GUILayout.Button("Remove Branching Path"))
            self.RemoveBranchingPathway();

        if (self)
            serializedObject.ApplyModifiedProperties();
    }

}
