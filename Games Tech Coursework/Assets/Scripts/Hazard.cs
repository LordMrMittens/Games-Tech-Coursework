using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Hazard : MonoBehaviour
{
    /// <summary>
    /// create a tool that shows the area of effect of attacks and can modify their range and timings (easey)
    /// </summary>
    [SerializeField] float attackRadius;
    [SerializeField] float attackTiming;
    [SerializeField] Color neutral, charging, attacking;
    [SerializeField] bool displayGizmos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
