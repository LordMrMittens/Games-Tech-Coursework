using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]GameObject playerPrefab;
    public GameObject player;
    public float score;
    public float time;
    public static GameManager TGM;
    [SerializeField] Text scoreText;
    [SerializeField] Text timerText;
    float secondsCounter;
    float minutesCounter;
    [field: SerializeField] float timeToRespawn { get; set; }
    float respawnTimer;
    public bool playerIsalive { get; set; }
    Transform playerSpawnPoint;
    private void Start()
    {
        TGM = this;
        scoreText.text = "score: " + 0;
        
    }
    private void Update()
    {
        if (playerSpawnPoint == null)
        {
            playerSpawnPoint = GameObject.FindGameObjectWithTag("EntryDoor").transform;
        }
        if (player ==null)
        {
            player = Instantiate(playerPrefab, playerSpawnPoint.transform.position,Quaternion.identity);
            player.transform.parent = GameObject.FindGameObjectWithTag("LevelRotationPoint").transform;
            playerIsalive = true;
            respawnTimer = 0;
        }
        if (player!=null&&playerIsalive == false)
        {

            respawnTimer += Time.deltaTime;
            if (respawnTimer > timeToRespawn)
            {
                player.SetActive(true);
                player.transform.position = playerSpawnPoint.position;
                playerIsalive = true;
                respawnTimer = 0;
            }
        }
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
