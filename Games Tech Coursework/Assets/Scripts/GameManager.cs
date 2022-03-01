using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float score;
    public float time;
    public static GameManager TGM;
    [SerializeField] Text scoreText;
    [SerializeField] Text timerText;
    float secondsCounter;
    float minutesCounter;
    
    private void Start()
    {
        TGM = this;
        scoreText.text = "score: " + 0;
    }
    private void Update()
    {
        secondsCounter += Time.deltaTime;
        timerText.text = $"{minutesCounter}:{(int)secondsCounter}";
        if (secondsCounter >= 60)
        {
            minutesCounter++;
            secondsCounter %= 60;
        }
    }
    public void AddPoints(int points)
    {
        score += points;
        scoreText.text = $"score: {score}"; 
    }
    public void LoadScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
