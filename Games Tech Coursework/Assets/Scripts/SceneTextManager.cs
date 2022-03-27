using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneTextManager : MonoBehaviour
{
  public Text scoreText;
   public Text timerText;
    [SerializeField] GameObject pauseMenu;
    bool paused;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                Time.timeScale = 0;
                paused = true;
                pauseMenu.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                paused = false;
                pauseMenu.SetActive(false);
            }
        }
    }
   public void loadScene(int scene)
    {
        GameManager.TGM.LoadScene(scene);
    }
    public void Quit()
    {
        GameManager.TGM.QuitGame();
    }
}
