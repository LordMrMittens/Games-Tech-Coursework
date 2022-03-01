using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class PointCounter : MonoBehaviour
{
    public int totalPoints;
}
[ExecuteAlways]
[CustomEditor(typeof(PointCounter))]

class pointCounterHandles : Editor
{
    protected virtual void OnSceneGUI()
    {
        PointCounter pc = (PointCounter)target;
        Handles.BeginGUI();
        {
            int numberOfPickups = 0;
            int numberOfHazards = 0;
            int numberOfBumpers = 0;
            int numberOfPlatforms = 0;
            bool hasExit = false;
            bool hasPlayer = false;
            Pickup[] pickups = GameObject.FindObjectsOfType<Pickup>();
            Hazard[] hazards = GameObject.FindObjectsOfType<Hazard>();
            Bumper[] bumpers = GameObject.FindObjectsOfType<Bumper>();
            MoverOverTime[] platforms = GameObject.FindObjectsOfType<MoverOverTime>();
            numberOfPickups = CheckNumberOfItems(pc, numberOfPickups, pickups);
            numberOfHazards = CheckNumberOfItems(pc, numberOfHazards, hazards);
            numberOfBumpers = CheckNumberOfItems(pc, numberOfBumpers, bumpers);
            numberOfPlatforms = CheckNumberOfItems(pc, numberOfPlatforms, platforms);
            hasPlayer = GameObject.FindObjectOfType<Jump>();
            hasExit = GameObject.FindObjectOfType<LevelExitTrigger>();
            GUILayout.Label("Total points available in scene: " + pc.totalPoints);
            GUILayout.Label("Total hazards in scene: " + numberOfHazards);
            GUILayout.Label("Total bumpers in scene: " + numberOfBumpers);
            GUILayout.Label("Total platforms in scene: " + numberOfPlatforms);
            GUILayout.Label("Player present in scene: " + hasPlayer);
            GUILayout.Label("Exit present in scene: " + hasPlayer);
        }
    }
    private static int CheckNumberOfItems(PointCounter pc, int numberOfPickups, Pickup[] pickups)
    {
        
        if (pickups.Length != numberOfPickups)
        {
            pc.totalPoints = 0;
            numberOfPickups = pickups.Length;
            foreach (Pickup pickup in pickups)
            {
                pc.totalPoints += pickup.value;
            }
        }
        return numberOfPickups;
    }
    private static int CheckNumberOfItems(PointCounter pc, int numberOfItems, Component[] Items)
    {
        if (Items.Length != numberOfItems)
        {
            numberOfItems = Items.Length;
        }

        return numberOfItems;
    }
}
