using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Bumper : MonoBehaviour
{
    public float bumpPower;
    public float timeBetweenBumps; //make these sliders?
    public float bumpTimer = 0;
    public bool canBump = false;
    void Update()
    {
        bumpTimer += Time.deltaTime;
        if(bumpTimer> timeBetweenBumps)
        {
            canBump = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 direction = (collision.transform.position - transform.position).normalized;
        collision.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * bumpPower, ForceMode2D.Impulse);
    }
}
[CustomEditor(typeof(Bumper)), CanEditMultipleObjects]
public class bumperHandles : Editor
{
    private void OnSceneGUI()
    {
        Bumper bump = (Bumper)target;
        EditorGUI.BeginChangeCheck();
        Handles.color = Color.blue;
        Handles.DrawWireDisc(bump.transform.position, bump.transform.forward, bump.bumpPower);
        Handles.Label(bump.transform.position + Vector3.up * (bump.bumpPower + .5f), "Approximate bump power");
        Handles.color = Color.red;
        float newBumpPower = (float)Handles.ScaleValueHandle(bump.bumpPower, bump.transform.position + bump.transform.up * bump.bumpPower, Quaternion.identity, 1, Handles.CircleHandleCap, 1);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(bump, "Changed moving platform positions");
            bump.bumpPower = newBumpPower;
        }
    }
}
