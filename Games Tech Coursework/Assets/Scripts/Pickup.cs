using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public int value;
    [SerializeField] GameObject pickupSound;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameManager.TGM.UpdateScore(value);
            Instantiate(pickupSound);
            Destroy(gameObject);
        }
    }
}
