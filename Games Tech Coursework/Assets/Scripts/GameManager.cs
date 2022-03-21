using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]GameObject playerPrefab;
    public GameObject player;
    public int score;
    public float time;
    public static GameManager TGM;

    float secondsCounter;
    float minutesCounter;
    [field: SerializeField] float timeToRespawn { get; set; }
    float respawnTimer;
    public bool playerIsalive { get; set; }
    Transform playerSpawnPoint;
    SceneTextManager textManager;
    Transform levelContainer;
    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "Main Menu")
        {
            DontDestroyOnLoad(this);
        }
    }
    private void Awake()
    {
        if (TGM != null && TGM != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            TGM = this;
        }
    }
    private void Update()
    {
  
        if (SceneManager.GetActiveScene().name != "Main Menu")
        {
            if (levelContainer == null)
            {
                levelContainer = GameObject.FindGameObjectWithTag("LevelRotationPoint").transform;
            }
            if (textManager == null)
            {
                textManager = GameObject.Find("Canvas").GetComponent<SceneTextManager>();
                textManager.scoreText.text = "score: " + 0;
                UpdateScore(0);
            }
            if (playerSpawnPoint == null)
            {
                playerSpawnPoint = GameObject.FindGameObjectWithTag("EntryDoor").transform;
            }
            if (player == null )
            {
                player = Instantiate(playerPrefab, new Vector3(playerSpawnPoint.position.x, playerSpawnPoint.position.y + .5f, playerSpawnPoint.position.z), Quaternion.identity);
                Debug.Log(player.name);
                player.transform.parent = levelContainer;
                playerIsalive = true;
                respawnTimer = 0;
            }
            if (player != null && playerIsalive == false)
            {

                respawnTimer += Time.deltaTime;
                if (respawnTimer > timeToRespawn)
                {
                    player.SetActive(true);
                    player.transform.position = new Vector3(playerSpawnPoint.position.x, playerSpawnPoint.position.y + .5f, playerSpawnPoint.position.z);
                    playerIsalive = true;
                    respawnTimer = 0;
                }
            }
            if (textManager != null)
            {
                UpdateClock();
            }
        }
    }

    private void UpdateClock()
    {
        secondsCounter += Time.deltaTime;
        textManager.timerText.text = $"{minutesCounter}:{(int)secondsCounter}";
        if (secondsCounter >= 60)
        {
            minutesCounter++;
            secondsCounter %= 60;
        }
    }

    public void UpdateScore(int points)
    {
        score += points;
       textManager.scoreText.text = $"score: {score}"; 
    }
    public void LoadScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
