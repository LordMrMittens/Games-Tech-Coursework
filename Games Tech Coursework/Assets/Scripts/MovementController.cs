using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class MovementController : MonoBehaviour
{
    /// <summary>
    /// can we draw gizmos to display the rotation limits?
    /// </summary>
   
    public float Speed;
    float z;
    Transform[] childrenTransforms;
    void Update()
    {
        float inputZ = Input.GetAxisRaw("Horizontal");
        z += (-inputZ * Time.deltaTime) * Speed;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, z));
    }

    public virtual void OnDrawGizmosSelected()
    {
        childrenTransforms = GetComponentsInChildren<Transform>();
        foreach (var item in childrenTransforms)
        {
            if (item.GetComponent<Collider2D>())
            {
                float distance = Vector3.Distance(transform.position, item.transform.position);
                float distanceFromCenter = Vector3.Distance(item.GetComponent<Collider2D>().bounds.extents, item.GetComponent<Collider2D>().bounds.center);
                Gizmos.DrawWireSphere(transform.position, distance%distanceFromCenter);
            }
        }
    }
}