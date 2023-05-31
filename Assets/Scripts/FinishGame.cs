using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class FinishGame : MonoBehaviour
{
    public InputField username;
    public SheetsManager sheets;
    public bool debug;

    private void Start()
    {
    }

    public void FinishButton()
    {
        if (PlayerPrefs.HasKey("PreviousPlayers"))
        {
            string[] prevPlayers = PlayerPrefs.GetString("PreviousPlayers").Split(",");
            if (prevPlayers.Contains(username.text))
            {
                if (debug)
                {
                    Debug.Log("Duplicate name detected!");
                }

                //HANDLE ERROR HERE


            }
            else
            {
                if (PlayerPrefs.HasKey("TotalTime")) {
                    sheets.AddUserToLeaderboard(username.text, (int)PlayerPrefs.GetFloat("TotalTime"));
                } else {
                    sheets.AddUserToLeaderboard(username.text, 9999999);
                }
            }
        }else
        {
            if (debug)
            {
                Debug.Log("WARNING: Missing PreviousPlayers Key in PlayerPrefs! Ignoring and proceeding...");
            }
            if (PlayerPrefs.HasKey("TotalTime")) {
                    sheets.AddUserToLeaderboard(username.text, (int)PlayerPrefs.GetFloat("TotalTime"));
            } else {
                sheets.AddUserToLeaderboard(username.text, 9999999);
            }
        }

        // Load survey scene
        SceneManager.LoadScene("Survey");

        username.enabled = false;
    }
}
