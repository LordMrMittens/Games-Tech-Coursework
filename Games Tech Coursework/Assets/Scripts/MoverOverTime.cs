using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[ExecuteAlways]
public class MoverOverTime : MonoBehaviour
{
    public GameObject posOne;
    public GameObject posTwo;
    public GameObject movingObject;
    public Vector3 positionOne;
    public Vector3 positionTwo;
    float distance;
    public float speed;
    [SerializeField] bool displayPreviews = true;
    void Start()
    {
        distance = Vector3.Distance(posOne.transform.position.normalized, posTwo.transform.position.normalized);
    }
    public virtual void Update()
    {
        if (Application.isPlaying)
        {
            MovePlatform(posOne.transform.position, posTwo.transform.position);
        }
    }

    private void MovePlatform(Vector3 pointA, Vector3 pointB)
    {
        float time = Mathf.PingPong(Time.time * (speed / Mathf.Abs(distance)), 1);
        movingObject.transform.position = Vector3.Lerp(pointA, pointB, time);
    }
    public virtual void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            posOne.transform.position = positionOne;
            posTwo.transform.position = positionTwo;
            transform.position = Vector3.Lerp(positionOne, positionTwo, 0.5f);
            if (displayPreviews)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(positionOne, transform.localScale);
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireCube(positionTwo, transform.localScale);
                Gizmos.color = Color.green;
                float gizmoTime = Mathf.PingPong((float)(EditorApplication.timeSinceStartup * (speed / Mathf.Abs(distance))), 1);
                Vector3 gizmoPreviewPosition = Vector3.Lerp(positionOne, positionTwo, gizmoTime);
                Gizmos.DrawWireCube(gizmoPreviewPosition, transform.localScale);
                if (Event.current.type == EventType.Repaint)
                {
                    SceneView.RepaintAll();
                }
            }
        }
    }
}
[CustomEditor(typeof(MoverOverTime)), CanEditMultipleObjects]
public class MoverOverTimeHandles : Editor
{
    private void OnSceneGUI()
    {
        if (Selection.activeGameObject.GetComponent<MoverOverTime>())
        {
            Tools.hidden = true;
        }
        else {
            Tools.hidden = false;
                }
        if (!Application.isPlaying)
        {
            MoverOverTime mot = (MoverOverTime)target;
            Vector3 posOnePosition = mot.positionOne;
            Vector3 posTwoPosition = mot.positionTwo;
            EditorGUI.BeginChangeCheck();
            Vector3 newPosOne = Handles.DoPositionHandle(posOnePosition, Quaternion.identity);
            Vector3 newPosTwo = Handles.DoPositionHandle(posTwoPosition, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(mot, "Changed moving platform positions");
                mot.positionOne = newPosOne;
                mot.positionTwo = newPosTwo;
                mot.Update();
            }
        }
    }
}
