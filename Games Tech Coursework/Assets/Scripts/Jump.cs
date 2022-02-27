using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Jump : MonoBehaviour
{
    /// <summary>
    /// Make player jump, display how high the player can jump (mid)
    /// </summary>
    public float jumpHeight;
    public float jumpCharge;
    public Color neutral, charging, jump;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
[CustomEditor(typeof(Jump)), CanEditMultipleObjects]
public class jumpHandles : Editor
{
    private void OnSceneGUI()
    {
        Jump ju = (Jump)target;
        Handles.DrawWireArc(ju.transform.position, ju.transform.forward,ju.transform.right, 180, ju.jumpHeight);
        Handles.Label(ju.transform.position + Vector3.up * ju.jumpHeight, "Approximate jump height");
    }
}