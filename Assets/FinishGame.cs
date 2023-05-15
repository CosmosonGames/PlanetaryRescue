using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class FinishGame : MonoBehaviour
{
    public InputField username;
    public SheetsManager sheets;
    public LogicManagerScript logic;

    private void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicManagerScript>();
    }

    public void FinishButton()
    {
        if (PlayerPrefs.HasKey("PreviousPlayers"))
        {
            string[] prevPlayers = PlayerPrefs.GetString("PreviousPlayers").Split(",");
            if (prevPlayers.Contains(username.text))
            {
                if (logic.debug)
                {
                    Debug.Log("Duplicate name detected!");
                }

                //HANDLE ERROR HERE


            }
            else
            {
                sheets.AddUserToLeaderboard(username.text, 123);
            }
        }else
        {
            if (logic.debug)
            {
                Debug.Log("WARNING: Missing PreviousPlayers Key in PlayerPrefs! Ignoring and proceeding...");
            }
            sheets.AddUserToLeaderboard(username.text, 123);
        }
    }
}
