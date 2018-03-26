# Unity Waypoint Editor
A waypoint system created for AI navigation in Unity from a game I worked on. This system is based on a binary tree system, where each waypoint can branch off to another path and so forth. For this Git upload, I also created a simple AI example that will follow the path.

## Installation
- Drag the scripts in the editor folder into your unity editor folder
- Drag the scripts in the scripts folder into any folder of your liking
- Drag the Waypoint.prefab into the Resources folder 

## Usage
Create an object and drag the waypoints_creator script onto the object.
When the waypoints_creator object is selected, you will have the following options:
- Add New Waypoint: This creates a waypoint at the end of the waypath
- Delete Last Waypoint: This deletes the waypoint at the end the waypath
- Clear Waypoints: This clears every waypoint that was created under the waypoints_creator.

After a waypoint is created, you can select that waypoint for more options:
- Add Next Waypoint: This creates a waypoint at the end of the main path.
- Insert New Waypoint: This adds a waypoint between the current waypoint and the next one. This option is only possible when the waypoint is not the last one.
- Delete This Waypoint: Deletes the selected waypoint; this is equivilent to pressing delete.
- Create Branching Path: This creates a path branch where the AI can randomly choose which to take.
- Loop to Existing Waypoint: This allows the path to go to a previously created waypoint, which will loop the AI's movement

Lastly, create an object and drag the AI_waypoint_follower_base script onto it and assign a waypoint to be it's inital point.

## Outcome
To gain experience with tools design and create a practical tool for AI path generation.
