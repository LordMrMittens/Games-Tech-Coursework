using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempMarker : MonoBehaviour
{
    public Color color { get; set; }
    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, .15f) ;
    }
    
}
