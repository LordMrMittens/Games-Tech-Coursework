using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class JBTools : MonoBehaviour
{
}
public class MovingPlatformWindow : EditorWindow
{
    public Vector3 firstPosition;
    public Vector3 secondPosition;
    public float speed;

    //[MenuItem("JB Tools/ Moving Platform Tool %&m")]
    static void OpenWindow()
    {
        EditorWindow.GetWindow(typeof(MovingPlatformWindow));
    }

    void OnGUI()
    {
        EditorGUILayout.PrefixLabel("Moving object tool");
        firstPosition = EditorGUILayout.Vector3Field("Starting Position", firstPosition);
        secondPosition = EditorGUILayout.Vector3Field("Ending Position", secondPosition);
        speed = EditorGUILayout.FloatField("Speed", speed);
        if (GUILayout.Button("Add component selected object"))
        {
            if (!Selection.activeGameObject)
            {
                Debug.Log("No Object Selected!");
            }
            else if (!Selection.activeGameObject.GetComponent<MoverOverTime>() && !Selection.activeGameObject.GetComponentInParent<MoverOverTime>())
            {
                CreateEmptyGameObjects();
            }
            else
            {
                TryAssignValues();
            }
        }
    }
    private void TryAssignValues()
    {
        MoverOverTime mover = Selection.activeGameObject.GetComponent<MoverOverTime>();
        if (mover == null)
        {
            mover = Selection.activeGameObject.GetComponentInParent<MoverOverTime>();
            Debug.Log("please make all adjustments from the Moving Object!");
            AssignTheValues(mover);
        }
        AssignTheValues(mover);
    }
    private void AssignTheValues(MoverOverTime mover)
    {
        if (mover != null)
        {
            mover.positionOne = firstPosition;
            mover.posOne.transform.position = firstPosition;
            mover.positionTwo = secondPosition;
            mover.posTwo.transform.position = secondPosition;
            mover.speed = speed;
            Selection.activeGameObject.transform.position = Vector3.Lerp(firstPosition, secondPosition, 0.5f);
        }
    }
    private void CreateEmptyGameObjects()
    {
        var master = new GameObject();
        master.name = "MovingObject";
        master.AddComponent<MoverOverTime>();
        master.transform.position = Selection.activeGameObject.transform.position;
        master.transform.localScale = Selection.activeGameObject.transform.localScale;
        Selection.activeGameObject.transform.parent = master.transform;
        MoverOverTime mover = master.GetComponent<MoverOverTime>();
        mover.movingObject = Selection.activeGameObject;
        mover.positionOne = firstPosition;
        mover.positionTwo = secondPosition;
        mover.speed = speed;
        var po = new GameObject();
        var pt = new GameObject();
        ApplyMoverDestinations(po.transform, master.transform, mover, "Position One");
        ApplyMoverDestinations(pt.transform, master.transform, mover, "Position Two");
        mover.posOne = po;
        mover.posTwo = pt;
        Selection.activeGameObject = master;
    }
    void ApplyMoverDestinations(Transform targetPosition, Transform parentObject, MoverOverTime mover, string name)
    {
        targetPosition.position = mover.positionOne;
        targetPosition.parent = parentObject;
        targetPosition.name = name;
    }
}