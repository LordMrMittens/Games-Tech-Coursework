using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class JBTools : MonoBehaviour
{
    [MenuItem("JB Tools/First Tool %&n", false)]
    
    static void CreateMovingPlatform()
    {
        
    }
    [MenuItem("JB Tools/First Tool", true)]
    static bool CreateMovingPlatformValidate()
    {
        
        return Selection.activeGameObject != null;
    }
    // Start is called before the first frame update
    
}
public class MovingPlatformWindow : EditorWindow
{
    public Vector3 firstPosition;
    public Vector3 secondPosition;
    public float travelTime;
    public float waitTime;
    public float speed;
    
    [MenuItem("JB Tools/ Moving Platform Tool %&m")]
    static void OpenWindow()
    {
        EditorWindow.GetWindow(typeof(MovingPlatformWindow));
    }

    void OnGUI()
    {
        
        EditorGUILayout.PrefixLabel("Choose an object, two locations and timings");
        firstPosition = EditorGUILayout.Vector3Field("Starting Position", firstPosition);
        secondPosition = EditorGUILayout.Vector3Field("Ending Position", secondPosition);
        travelTime = EditorGUILayout.FloatField("Travel Time", travelTime);
        waitTime = EditorGUILayout.FloatField("Wait Time", waitTime);
        speed = EditorGUILayout.FloatField("Speed", speed);
        if ( GUILayout.Button("Apply movement to selected object"))
        {
            if (!Selection.activeGameObject)
            {
                Debug.Log("No Object Selected!");
            }
            else if (!Selection.activeGameObject.GetComponent<MoverOverTime>())
            {
                Selection.activeGameObject.AddComponent<MoverOverTime>();
                AssignValues();
            }
            else
            {
                AssignValues();
            }
        }
    }

    private void AssignValues()
    {
        MoverOverTime mover = Selection.activeGameObject.GetComponent<MoverOverTime>();
        mover.positionOne = firstPosition;
        mover.positionTwo = secondPosition;
        mover.travelTime = travelTime;
        mover.waitTime = waitTime;
        mover.speed = speed;
    }
}

