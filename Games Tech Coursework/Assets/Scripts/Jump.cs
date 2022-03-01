using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Jump : MonoBehaviour
{
    public float jumpHeight;
    public float jumpCharge;
    public Color neutral, charging, jump;
    Rigidbody2D rb;
    public CircleCollider2D circleCollider;
    bool canJump;
    public Gradient gradient;
    GradientColorKey[] colorkey = new GradientColorKey[3];
    GradientAlphaKey[] alphakey = new GradientAlphaKey[3];
    float gradientChargeTimer = 0;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        colorkey[0].color = neutral;
        colorkey[0].time = 0f;
        colorkey[1].color = charging;
        colorkey[1].time = .5f;
        colorkey[2].color = jump;
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
        JumpingLogic();
    }

    private void JumpingLogic()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ChargeJump();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            DoAJump();
            jumpCharge = 0;
        }
    }
    private void DoAJump()
    {
        if (canJump)
        {
            rb.AddForce(Vector2.up * (jumpHeight + jumpCharge), ForceMode2D.Impulse);
            canJump = false;
            gradientChargeTimer = 0;
            gameObject.GetComponent<SpriteRenderer>().color = gradient.Evaluate(0);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        canJump = true;
    }
    private void ChargeJump()
    {
        jumpCharge += Time.deltaTime;
        gradientChargeTimer  += Time.deltaTime / 3;
        float t = Mathf.Lerp(0,1, gradientChargeTimer);
        gameObject.GetComponent<SpriteRenderer>().color = gradient.Evaluate(t);
        if (jumpCharge > 3)
        {
            jumpCharge = 3;
        }
    }
}
[CustomEditor(typeof(Jump)), CanEditMultipleObjects]
public class jumpHandles : Editor
{
    private void OnSceneGUI()
    {
        Jump ju = (Jump)target;
        EditorGUI.BeginChangeCheck();
        Handles.DrawWireArc(ju.transform.position, ju.transform.forward,ju.transform.right, 180, ju.jumpHeight-ju.circleCollider.radius);
        Handles.Label(ju.transform.position + Vector3.up * (ju.jumpHeight), "Approximate max jump height");
        Handles.color = Color.red;
        float newJumpHeight = (float)Handles.ScaleValueHandle(ju.jumpHeight, ju.transform.position + ju.transform.up * (ju.jumpHeight - ju.circleCollider.radius), Quaternion.identity, 1, Handles.CircleHandleCap, 1);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(ju, "Changed moving platform positions");
            ju.jumpHeight = newJumpHeight;
        }
        }
}