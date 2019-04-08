using UnityEngine;
using UnityEngine.UI;
using System;

public class Leaderboard : MonoBehaviour
{
    public GameObject playerOnBoard;

    ScoreKeeper scoreKeeper;
    Vector3 pos;
    GameObject[] scoresOnBoard;

    void OnEnable()
    {
        pos = Vector3.zero;
        pos.y = -115;

        scoreKeeper = Resources.Load("ScoreKeeper") as ScoreKeeper;
        if (scoreKeeper == null || scoreKeeper.playerScores == null) return;
        scoresOnBoard = new GameObject[scoreKeeper.playerScores.Count];
        scoreKeeper.playerScores.Sort(delegate (PlayerScore p1, PlayerScore p2) { return p1.score.CompareTo(p2.score); });

        // Display all saved cumulative player scores.
        for (var i = 0; i < scoreKeeper.playerScores.Count; i++) {
            scoresOnBoard[i] = (GameObject)Instantiate(
                playerOnBoard,
                pos,
                Quaternion.identity);
            scoresOnBoard[i].transform.SetParent(transform, false);
            TimeSpan ts = TimeSpan.FromSeconds(scoreKeeper.playerScores[i].score);
            var scoreText = string.Format("{0:D2}:{1:D2}:{2:D2}", ts.Hours, ts.Minutes, ts.Seconds);
            scoresOnBoard[i].GetComponent<Text>().text = scoreKeeper.playerScores[i].pName + " | " + scoreText;
            pos.y -= 40;
        }
    }

    void OnDisable()
    {
        if (scoresOnBoard != null) {
            foreach (var score in scoresOnBoard)
            {
                Destroy(score);
            }
        }
    }
}
