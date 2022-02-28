using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    /// <summary>
    /// can we draw gizmos to display the rotation limits?(easy-ish)
    /// </summary>
   
    public float Speed;
    float z;
    void Update()
    {
        float inputZ = Input.GetAxisRaw("Horizontal");
        z += (-inputZ * Time.deltaTime) * Speed;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, z));
    }
}
