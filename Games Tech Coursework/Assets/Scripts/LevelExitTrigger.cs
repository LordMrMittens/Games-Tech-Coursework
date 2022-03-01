using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExitTrigger : MonoBehaviour
{
    [SerializeField] float exitTime;
    float exitTimer = 0;
    [SerializeField]int sceneToLoad;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            exitTimer += Time.deltaTime;
            if (exitTimer > exitTime)
            {
                exitTimer = -5;
                GameManager.TGM.LoadScene(sceneToLoad);
                
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            exitTimer = 0;
            Debug.Log("Timer reset");
        }
    }
}
