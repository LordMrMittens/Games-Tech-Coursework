using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardHitDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.SetActive(false);
            GameManager.TGM.playerIsalive = false;
        }
    }
}
