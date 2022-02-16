using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MoverOverTime : MonoBehaviour
{
    public Vector3 positionOne;
    public Vector3 positionTwo;
    float distance;
    public float speed;
    [SerializeField] bool displayPreviews=true;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = positionOne;
        distance = Vector3.Distance(positionOne.normalized, positionTwo.normalized);
        
    }

    // Update is called once per frame
    void Update()
    {
            MovePlatform(positionOne,positionTwo);
        
    }
    
    private void MovePlatform(Vector3 pointA, Vector3 pointB)
    {
        float time = Mathf.PingPong(Time.time * (speed / Mathf.Abs(distance)), 1);
        transform.position = Vector3.Lerp(pointA, pointB, time);

    }
    //[ExecuteAlways]
    void OnDrawGizmos()
    {
        if (displayPreviews)
        {
            //Gizmos.matrix = Matrix4x4.TRS(positionOne, transform.rotation, transform.lossyScale);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(positionOne, transform.localScale);
            //Gizmos.matrix = Matrix4x4.TRS(positionTwo, transform.rotation, transform.lossyScale);
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(positionTwo, transform.localScale);
            Gizmos.color = Color.green;
            float gizmoTime = Mathf.PingPong((float)(EditorApplication.timeSinceStartup * (speed / Mathf.Abs(distance))), 1);
            Vector3 gizmoPreviewPosition = Vector3.Lerp(positionOne, positionTwo, gizmoTime);
            Gizmos.DrawWireCube(gizmoPreviewPosition, transform.localScale);
            /*            if (!Application.isPlaying)
                        {
                            EditorApplication.QueuePlayerLoopUpdate();
                            SceneView.RepaintAll();
                        }*/
            if (Event.current.type == EventType.Repaint)
            {
                SceneView.RepaintAll();
            }
        }
    }
    


    

}



