using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogicManagerScript : MonoBehaviour
{
    public bool multiplayer = true;
    public bool debug = true;

    [Header("Timer Settings")]
    public TextMeshProUGUI timerText;
    public bool countDown;
    public float currentTime;

    [Header("Timer Limits")]
    public bool hasLimit;
    public float timerLimit;

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

    private void Update()
    {
        currentTime = countDown ? currentTime -= Time.deltaTime : currentTime += Time.deltaTime;

        if(hasLimit && ((countDown && currentTime <= timerLimit) || (!countDown && currentTime >= timerLimit))){
            currentTime = timerLimit;
            SetTimerText();
            timerText.color = Color.red;
            enabled = false;

        }
        SetTimerText();

    }

    private void SetTimerText()
    {
        double minutes = Math.Floor(currentTime/60);
        double seconds = currentTime % 60;
        string time = $"{minutes.ToString("00")}:{seconds.ToString("00")}";

        timerText.text = time;
    } 
}