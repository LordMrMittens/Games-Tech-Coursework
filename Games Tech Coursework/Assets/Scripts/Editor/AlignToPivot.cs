using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AlignToPivot : EditorWindow
{
    public GameObject pivotPoint;
    public GameObject[] children;
    [MenuItem("JB Tools/Align to pivot")]
    static void OpenWindow()
    {
        EditorWindow.GetWindow(typeof(AlignToPivot));
    }
    void OnGUI()
    {
       
        EditorGUILayout.PrefixLabel("Choose an object to pivot around");
        
        pivotPoint = (GameObject)EditorGUILayout.ObjectField(pivotPoint, typeof(GameObject),true);
        children = Selection.gameObjects;

        if (GUILayout.Button("Parent and align to pivotpoint"))
        {
            foreach (var child in children)
            {
                child.transform.parent = pivotPoint.transform;
                child.transform.position = pivotPoint.transform.position;
            }

        }
        if (GUILayout.Button("Parent to pivotpoint"))
        {
            foreach (var child in children)
            {
                child.transform.parent = pivotPoint.transform;
            }
        }

        

    }
}
