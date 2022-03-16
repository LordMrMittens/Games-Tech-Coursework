using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TempObject : MonoBehaviour
{
    float maxJumpDistance;
    public float range { get; set; }
    GameObject playerPrefab;
    public bool isDrawingRange = true;

    private void OnDrawGizmos()
    {
        playerPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Player.prefab", typeof(GameObject));
        maxJumpDistance = playerPrefab.GetComponent<PlayerController>().jumpHeight;
        Gizmos.DrawSphere(transform.position, .2f);
        Gizmos.color = Color.red;
        Handles.Label(new Vector3(transform.position.x - 1, transform.position.y + maxJumpDistance+.3f, transform.position.z), "Approximate Player Max Jump");
        Gizmos.DrawLine(new Vector3(transform.position.x-.5f, transform.position.y + maxJumpDistance, transform.position.z), new Vector3(transform.position.x +.5f, transform.position.y + maxJumpDistance, transform.position.z));
        Gizmos.color = Color.blue;
        if (isDrawingRange)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}
[CustomEditor(typeof(TempObject)), CanEditMultipleObjects]
public class TempObjectHandles : Editor
{
    private void OnSceneGUI()
    {
        TempObject to = (TempObject)target;
        
    }
}