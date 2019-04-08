using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System;
using UnityEditor;

public class GameController : MonoBehaviour {

    public GameObject playerPrefab;
    public GameObject npcPrefab;
    public Transform ground;
    public int numNpcPlayers;
    public GameObject pauseMenu;
    public GameObject startMenu;
    public GameObject gameOverMenu;
    public GameObject leaderboard;
    public float gameTime;
    public Text titleText;
    public Text timerText;

    GameObject[] activePlayers;
    ScoreKeeper scoreKeeper;
    bool gameActive = false;
    bool gameEnded = false;
    bool restart = false;

	// Use this for initialization
	void Start () {
        Time.timeScale = 0;
        startMenu.SetActive(true);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        leaderboard.SetActive(false);
        titleText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        scoreKeeper = Resources.Load("ScoreKeeper") as ScoreKeeper;
        if (scoreKeeper == null) {
            scoreKeeper = ScoreKeeperBuilder.Build();
        }
    }

    // Update is called once per frame
    void Update()
    {
        gameTime -= Time.deltaTime;
        TimeSpan ts = TimeSpan.FromSeconds(gameTime);
        timerText.text = string.Format("{0:D2}:{1:D2}", ((int)ts.Minutes), ts.Seconds);
        if (gameTime < 0 && gameActive && !gameEnded)
        {
            gameEnded = true;
            OnEndGame();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
                gameActive = false;
            }
            else
            {
                Time.timeScale = 1;
                pauseMenu.SetActive(false);
                gameActive = true;
            }
        }
    }

    /// <summary>
    /// Starts the game.
    /// </summary>
    public void StartGame() {
        startMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        titleText.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);
        gameActive = true;
        gameTime = 5*60; // 5 Minutes
        gameEnded = false;
        TimeSpan ts = TimeSpan.FromSeconds(gameTime);
        timerText.text = string.Format("{0:D2}:{1:D2}", ((int)ts.Minutes), ts.Seconds);
        Time.timeScale = 1;
        var corners = new[] {
            new Vector3(ground.position.x + 1, 0.647f, ground.position.z + 1),
            new Vector3(ground.position.x + 1, 0.0f, ground.position.z - 1),
            new Vector3(ground.position.x - 1, 0.0f, ground.position.z + 1),
            new Vector3(ground.position.x - 1, 0.0f, ground.position.z - 1)
        };

        if (!restart) {
            activePlayers = new GameObject[numNpcPlayers + 1];
            activePlayers[0] = (GameObject)Instantiate(
                playerPrefab,
                corners[0],
                ground.rotation);
            activePlayers[0].name = "You";

            for (var i = 1; i <= numNpcPlayers; i++)
            {
                activePlayers[i] = (GameObject)Instantiate(
                    npcPrefab,
                    corners[i],
                    ground.rotation);
                activePlayers[i].name = "Most Evil NPC " + i;
            }
        } 
        else {
            foreach (var projectile in GameObject.FindGameObjectsWithTag("Projectile")) {
                Destroy(projectile);
            }
            activePlayers[0].GetComponent<Player>().it = true;
            activePlayers[0].GetComponent<Player>().youreItIndicator.GetComponent<TextMesh>().text = activePlayers[0].GetComponent<Player>().youreItMsg;
            activePlayers[0].GetComponent<Player>().youreItIndicator.SetActive(true);
            activePlayers[0].GetComponent<Player>().score = 0;
            for (var i = 0; i < activePlayers.Length; i++) {
                if (i > 0) {
                    activePlayers[i].GetComponent<Player>().HitEnemy();
                }
                activePlayers[i].transform.position = corners[i];
                activePlayers[i].SetActive(true);
                activePlayers[i].GetComponent<Player>().score = 0;
            }
        }
    }

    /// <summary>
    /// Restarts the game.
    /// </summary>
    public void RestartGame() {
        foreach (var player in activePlayers)
        {
            player.SetActive(false);
        }
        pauseMenu.SetActive(false);
        restart = true;
        StartGame();
    }

    /// <summary>
    /// Fires at the end of the game.
    /// </summary>
    public void OnEndGame() {
        Time.timeScale = 0;
        gameOverMenu.SetActive(true);
        foreach (var player in activePlayers) {
            var playerScore = scoreKeeper.playerScores.FirstOrDefault(p => p.pName.Equals(player.name));
            if (playerScore == null) {
                playerScore = new PlayerScore
                {
                    pName = player.name
                };
                scoreKeeper.playerScores.Add(playerScore);
            }
            playerScore.score += player.GetComponent<Player>().score;
        }
        EditorUtility.SetDirty(scoreKeeper);
    }

    /// <summary>
    /// Display the leaderboard.
    /// </summary>
    public void Leaderboard() {
        startMenu.SetActive(false);
        leaderboard.SetActive(true);
    }

    /// <summary>
    /// Exits the game and returns to the main menu.
    /// </summary>
    public void ExitGame() {
        foreach (var player in activePlayers)
        {
            player.SetActive(false);
        }
        var projectiles = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (var p in projectiles)
        {
            Destroy(p);
        }
        BackToStartMenu();
    }

    /// <summary>
    /// Quits the game.
    /// </summary>
    public void QuitGame() {
        Application.Quit();
    }

    /// <summary>
    /// Navigate back to start menu.
    /// </summary>
    public void BackToStartMenu() {
        gameOverMenu.SetActive(false);
        pauseMenu.SetActive(false);
        leaderboard.SetActive(false);
        titleText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        startMenu.SetActive(true);
    }
}
