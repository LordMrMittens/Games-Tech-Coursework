using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowController : MonoBehaviour
{
    GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
               
                transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, transform.position.z);
            }
        }
    }
}
