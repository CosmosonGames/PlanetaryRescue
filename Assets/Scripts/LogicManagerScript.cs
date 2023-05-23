using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogicManagerScript : MonoBehaviour
{
    public bool multiplayer = true;
    public bool debug = true;
    public long startTime;

    public long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

    //load storyline scene
    public void startGame()
    {
        SceneManager.LoadScene("IntroScene");
        PlayerPrefs.GetFloat("start");
    }

    //get leaderboard from database, display it, and show the leaderboard scene
    public void openLeaderboard()
    {
        //get leaderboard here...
        SceneManager.LoadScene("LeaderboardScene");
    }
}