using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverOverTime : MonoBehaviour
{
    public Vector3 positionOne;
    public Vector3 positionTwo;
    public float travelTime;
    public float speed;
    public float waitTime;
    float startTime;
    float journeyLength;
    bool forward;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = positionOne;
        startTime = Time.time;
        journeyLength = Vector3.Distance(positionOne, positionTwo);
    }

    // Update is called once per frame
    void Update()
    {
            MovePlatform(positionOne,positionTwo);
    }

    private void MovePlatform(Vector3 pointA, Vector3 pointB)
    {
        float time = Mathf.PingPong(Time.time * speed, travelTime);
        transform.position = Vector3.Lerp(pointA, pointB, time);

    }

}
