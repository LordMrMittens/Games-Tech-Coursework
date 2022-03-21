using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class MovementController : MonoBehaviour
{
    [SerializeField] float Speed =40;
    float z;
    void Update()
    {
        float inputZ = Input.GetAxisRaw("Horizontal");
        z += (-inputZ * Time.deltaTime) * Speed;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, z));
    }
}