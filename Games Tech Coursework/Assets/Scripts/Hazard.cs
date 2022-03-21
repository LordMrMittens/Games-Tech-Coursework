using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Hazard : MonoBehaviour
{
    /// <summary>
    /// create a tool that shows the area of effect of attacks and can modify their range and timings (easy) with sliders?
    /// 
    /// display angry face and green happy face to dsplay attack?
    /// </summary>
    public float attackRadius;
    public float timeBetweenAttacks;
    public float timeAttacking;
    float attackTimer = 0;
    float downtimeTimer = 0;
    public Color neutral, charging, attacking;
    public bool isAttacking;
    public bool displayGizmos;
    public GameObject hazardObject;
    public Gradient gradient;
    GradientColorKey[] colorkey = new GradientColorKey[3];
    GradientAlphaKey[] alphakey = new GradientAlphaKey[3];
    float gradientChargeTimer = 0;
    private void Start()
    {
   
        colorkey[0].color = neutral;
        colorkey[0].time = 0f;
        colorkey[1].color = charging;
        colorkey[1].time = .5f;
        colorkey[2].color = attacking;
        colorkey[2].time = 1f;
        alphakey[0].alpha = 1;
        alphakey[0].time = 0f;
        alphakey[1].alpha = 1;
        alphakey[1].time = .5f;
        alphakey[2].alpha = 1;
        alphakey[2].time = 1f;
        gradient.SetKeys(colorkey, alphakey);
    }
    void Update()
    {
        hazardObject.transform.localScale = new Vector3(attackRadius/transform.localScale.x, attackRadius / transform.localScale.y, attackRadius / transform.localScale.z);
        if (isAttacking)
        {
            hazardObject.SetActive(true);
            attackTimer += Time.deltaTime;
            if (attackTimer > timeAttacking)
            {
                downtimeTimer = 0;
                gradientChargeTimer = 0;
                isAttacking = false;
            }
        } else
        {
            hazardObject.SetActive(false);
            downtimeTimer += Time.deltaTime;
            gradientChargeTimer += Time.deltaTime / 8;
            float t = Mathf.Lerp(0, 1, gradientChargeTimer);
            gameObject.GetComponent<SpriteRenderer>().color = gradient.Evaluate(t);
            if (downtimeTimer > timeBetweenAttacks)
            {
                attackTimer = 0;
                isAttacking = true;
            }
        }
    }
    
}

[CustomEditor(typeof(Hazard)), CanEditMultipleObjects]
public class hazardHandles : Editor
{
    private void OnSceneGUI()
    {
        Hazard haz = (Hazard)target;
        EditorGUI.BeginChangeCheck();
        Handles.Label(haz.transform.position + Vector3.up * ((haz.attackRadius/2) + .5f), "Approximate attack Radius");
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(haz.transform.position, haz.transform.forward, haz.attackRadius / 2f);
        Handles.color = Color.red;
        float newAttackRadius = (float)Handles.ScaleValueHandle(haz.attackRadius, haz.transform.position + haz.transform.up * (haz.attackRadius/2), Quaternion.LookRotation(Vector3.up), 1, Handles.ConeHandleCap, 10);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(haz, "Changed moving platform positions");
            haz.attackRadius = newAttackRadius;
            haz.hazardObject.transform.localScale = new Vector3(haz.attackRadius, haz.attackRadius, haz.attackRadius);
        }
    }
}
