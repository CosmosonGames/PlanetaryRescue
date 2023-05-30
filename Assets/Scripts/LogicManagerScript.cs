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

    [Header("Sheets")]
    public GameObject sheetsManager;
    private SheetsManager sheets;

    [Header("Timer Limits")]
    public bool hasLimit;
    public float timerLimit;

    public float roomZeroTime = 0f;

    public bool roomOneComplete = false;
    public float roomOneTime = 0f;
    public float roomOneStartTime = 0f;
    public float roomOneEndTime = 0f;

    public bool roomTwoComplete = false;
    public float roomTwoTime = 0f;
    public float roomTwoStartTime = 0f;
    public float roomTwoEndTime = 0f;

    public bool roomThreeComplete = false;
    public float roomThreeTime = 0f;
    public float roomThreeStartTime = 0f;
    public float roomThreeEndTime = 0f;

    public bool roomFourComplete = false;
    public float roomFourTime = 0f;
    public float roomFourStartTime = 0f;
    public float roomFourEndTime = 0f;

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

    public void StartRoomTime(int roomNum)
    {
        sheets = sheetsManager.GetComponent<SheetsManager>();
        switch (roomNum)
        {
            case 1:
                if (roomOneStartTime == 0f)
                {
                    roomOneStartTime = currentTime;
                }
                else
                {
                    if (debug)
                    {
                        Debug.Log("Start time for room one has already been set!");
                    }
                }
                break;
            case 2:
                if (roomTwoStartTime == 0f)
                {
                    roomTwoStartTime = currentTime;
                }
                else
                {
                    if (debug)
                    {
                        Debug.Log("Start time for room two has already been set!");
                    }
                }
                break;
            case 3:
                if (roomThreeStartTime == 0f)
                {
                    roomThreeStartTime = currentTime;
                }
                else
                {
                    if (debug)
                    {
                        Debug.Log("Start time for room three has already been set!");
                    }
                }
                break;
            case 4:
                if (roomFourStartTime == 0f)
                {
                    roomFourStartTime = currentTime;
                }
                else
                {
                    if (debug)
                    {
                        Debug.Log("Start time for room four has already been set!");
                    }
                }
                break;
            default:
                if (debug)
                {
                    Debug.Log("ERROR: Invalid room number!");
                }
                break;
        }
    }

    public void EndRoomTime(int roomNum)
    {
        sheets = sheetsManager.GetComponent<SheetsManager>();
        switch (roomNum)
        {
            case 0: 
                roomZeroTime = currentTime;
                sheets.AddRoomData("transport", 0f, roomZeroTime);
                break;
            case 1:
                if (roomOneEndTime != 0f) {
                    if (debug) {
                        Debug.Log("ERROR: End time for room one already set!");
                    }
                    return;
                }
                roomOneEndTime = currentTime;
                roomOneComplete = true;
                sheets.AddRoomData("admin", roomOneStartTime, roomOneEndTime);
                break;
            case 2:
                if (roomTwoEndTime != 0f) {
                    if (debug) {
                        Debug.Log("ERROR: End time for room two already set!");
                    }
                    return;
                }
                roomTwoEndTime = currentTime;
                roomTwoComplete = true;
                sheets.AddRoomData("combat", roomTwoStartTime, roomTwoEndTime);
                break;
            case 3:
                if (roomThreeEndTime != 0f) {
                    if (debug) {
                        Debug.Log("ERROR: End time for room three already set!");
                    }
                    return;
                }
                roomThreeEndTime = currentTime;
                roomThreeComplete = true;
                sheets.AddRoomData("technical", roomThreeStartTime, roomThreeEndTime);
                break;
            case 4:
                if (roomFourEndTime != 0f) {
                    if (debug) {
                        Debug.Log("ERROR: End time for room four already set!");
                    }
                    return;
                }
                roomFourEndTime = currentTime;
                roomFourComplete = true;
                sheets.AddRoomData("control", roomFourStartTime, roomFourEndTime);
                break;
            default:
                if (debug) {
                    Debug.Log("ERROR: Invalid room number!");
                }
                break;
        }
    }
}