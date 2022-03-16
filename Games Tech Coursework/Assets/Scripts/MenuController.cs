using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour
{
    public int sceneNumber;

    public void loadScene(int sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
