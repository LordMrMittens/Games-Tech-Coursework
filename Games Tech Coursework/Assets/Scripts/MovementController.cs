using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float Speed;
    float z;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float inputZ = Input.GetAxisRaw("Horizontal");
        z += (inputZ * Time.deltaTime) * Speed;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, z));
    }
}