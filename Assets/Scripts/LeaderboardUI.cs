using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class LeaderboardUI : MonoBehaviour
{
    public GameObject timeLabels;
    public GameObject userLabels;

    // Start is called before the first frame update
    void Start()
    {
        string rawLeaderboard;
        if (PlayerPrefs.HasKey("Leaderboard") && PlayerPrefs.GetString("Leaderboard") != null)
        {
            Transform timeTransform = timeLabels.transform;
            Transform usersTransform = userLabels.transform;

            rawLeaderboard = PlayerPrefs.GetString("Leaderboard");
            string[] splitLeaderboard = rawLeaderboard.Split("|");

            int index = 0;

            for (int i = 0; i < timeTransform.childCount; i++)
            {
                // Get the i-th child object
                Transform tTransform = timeTransform.GetChild(i);
                Transform uTransform = usersTransform.GetChild(i);

                Text tLabel = tTransform.GetComponentInChildren<Text>();
                Text uLabel = uTransform.GetComponentInChildren<Text>();

                tLabel.text = "";
                uLabel.text = "";
            }

        foreach (string entry in splitLeaderboard)
            {
                if (entry != "")
                {
                    string[] divided = entry.Split(",");
                    string user = divided[0];
                    string time = divided[1];

                    Transform cTime = timeTransform.GetChild(index);
                    Text timeLabel = cTime.GetComponentInChildren<Text>();
                    timeLabel.text = time;

                    Transform cUser = usersTransform.GetChild(index);
                    Text userLabel = cUser.GetComponentInChildren<Text>();
                    userLabel.text = user;

                    Debug.Log($"User: {user}");
                    Debug.Log($"Time: {time}");
                    index += 1;
                }
            }
        }
        else
        {
            Debug.Log("WARNING: Leaderboard not found!");
        }
    }
}