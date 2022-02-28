using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class PointCounter : MonoBehaviour
{
    public int totalPoints;
    // Start is called before the first frame update
    /// <summary>
    /// create a tool that allows a developet to see how many points the player can earn in one scene and to be able to modify the points value?
    /// it can also create a checklist of things required on scene. (medium)
    /// </summary>
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
[CustomEditor(typeof(PointCounter))]

class pointCounterHandles : Editor
{
    protected virtual void OnSceneGUI()
    {
        PointCounter pc = (PointCounter)target;
        Handles.BeginGUI();
        {
            int NumberOfPickups =0;
            Pickup[] pickups = GameObject.FindObjectsOfType<Pickup>();
            if (pickups.Length != NumberOfPickups)
            {
                pc.totalPoints = 0;
                NumberOfPickups = pickups.Length;
                foreach (Pickup pickup in pickups)
                {
                    pc.totalPoints += pickup.value;
                }
            }
            GUILayout.Label("Total points available in scene: " + pc.totalPoints);
        }
    }
}
