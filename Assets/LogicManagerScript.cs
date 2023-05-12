using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogicManagerScript : MonoBehaviour
{
    public bool multiplayer = true;

    //load storyline scene
    public void startGame()
    {
        SceneManager.LoadScene("IntroScene");
    }

    //get leaderboard from database, display it, and show the leaderboard scene
    public void openLeaderboard()
    {
        //get leaderboard here...
        SceneManager.LoadScene("LeaderboardScene");
    }
}