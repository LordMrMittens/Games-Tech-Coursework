using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowController : MonoBehaviour
{
    GameObject Player;
    [SerializeField] float offset;
    void LateUpdate()
    {

        if (GameManager.TGM.playerIsalive)
        {
            if (Player == null)
            {
                Player = GameObject.FindGameObjectWithTag("Player");
            }
            if (Player != null)
            {
               
                transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y,offset);
            }
        }
    }
}
