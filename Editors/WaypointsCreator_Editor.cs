using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Waypoints;
using System;

[CustomEditor(typeof(Waypoints_Creator))]
public class WaypointsCreator_Editor : Editor
{
    Waypoints_Creator creator;

    public override void OnInspectorGUI()
    {
        creator = (Waypoints_Creator)target;

        SerializedProperty waypointResourcesPath_Property = serializedObject.FindProperty("ResourcesPath");
        EditorGUILayout.PropertyField(waypointResourcesPath_Property, new GUIContent("Resources Path: "), GUILayout.Height(20));

        if (!creator.IsEmpty())
        {
            SerializedProperty showPathway_Property = serializedObject.FindProperty("_isDisplayingPathway");
            EditorGUILayout.PropertyField(showPathway_Property, new GUIContent("Display Pathway: "), GUILayout.Height(20));
        }

        if (creator.gameObject.GetComponent<Waypoint>() != null)
            GUILayout.Label("Secondary Branching Pathway");

        if (GUILayout.Button("Add New Waypoint"))
            creator.CreateWaypoint();
        
        if (!creator.IsEmpty() && GUILayout.Button("Delete Last Waypoint"))
            creator.DeleteWaypoint();

        if (!creator.IsEmpty() && GUILayout.Button("Clear Waypoints"))
            creator.ClearWaypoints();     

        serializedObject.ApplyModifiedProperties();
    }

}
