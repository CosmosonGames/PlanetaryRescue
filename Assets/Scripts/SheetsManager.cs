using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

using UnityEngine;

public class SheetsManager : MonoBehaviour
{
    static readonly string[] scopes = { Google.Apis.Sheets.v4.SheetsService.Scope.Spreadsheets };

    static readonly string applicationName = "planetary";

    static readonly string spreadsheetId = "1vaJzdKDZP85q5CDc6RHnONkJTOlMGAFX3Mdioqn4a98";

    static private string jsonPath = "/Creds/cosmosongames-0c17ee530b7b.json";

    static public SheetsService service;

    public LogicManagerScript logic;

    public bool debug;

    public void Start()
    {
        if (logic != null) {
            logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicManagerScript>();
            debug = logic.debug;
        }

        string fullPath = Application.dataPath + jsonPath;

        GoogleCredential credential;

        using (var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
        {
            credential = GoogleCredential.FromStream(stream).CreateScoped(scopes);
        }

        service = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = applicationName,
        });

        GetLeaderboard();
    }

    public IList<IList<object>> ReadEntries(string range)
    { 
        var request = service.Spreadsheets.Values.Get(spreadsheetId, range);

        var response = request.Execute();
        var values = response.Values;

        if (values != null && values.Count > 0)
        {
            return values;
        } else
        {
            return null;
        }
    }

    public void CreateEntry(List<object> data, string range)
    {
        var valueRange = new ValueRange();
        valueRange.Values = new List<IList<object>> { data };

        var appendRequest = service.Spreadsheets.Values.Append(valueRange, spreadsheetId, range);
        appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
        var appendResponse = appendRequest.Execute();

        //ADD ERROR HANDLING!
    }

    public void GetLeaderboard()
    {
        var rawData = ReadEntries(range: "rawLB!A:B");

        if (rawData != null && rawData.Count > 0)
        {
            Dictionary<string, int> leaderboard = new Dictionary<string, int>();

            string players = "";
            foreach (var row in rawData)
            {
                int num;
                if (int.TryParse(row[1].ToString(), out num))
                {
                    string user = row[0].ToString();
                    leaderboard.Add(user, num);
                    players = $"{players}{user},";
                }
            }

            var sortedLeaderboard = leaderboard.OrderBy(x => x.Value).Take(5);

            string lbString = "";
            foreach (var item in sortedLeaderboard)
            {
                lbString = $"{lbString}{item.Key},{item.Value.ToString()}|";
            }

            if (debug)
            {
                Debug.Log(lbString);
                Debug.Log(players);
            }
            PlayerPrefs.SetString("Leaderboard", lbString);
            PlayerPrefs.SetString("PreviousPlayers", players);
        }
        else
        {
            PlayerPrefs.SetString("Leaderboard", null);
            PlayerPrefs.SetString("PreviousPlayers", null);
        }
        PlayerPrefs.Save();

        if (debug)
        {
            var data = ReadEntries("rawLB!A:B");

            if (data != null && data.Count > 0)
            {
                foreach (var row in data)
                {
                    Debug.Log($"Username: {row[0]}");
                    Debug.Log($"Time: {row[1]}");
                }
            }
        }
    }

    public void AddUserToLeaderboard(string username, int time)
    {
        var toAdd = new List<object>() {username, time};
        CreateEntry(toAdd, "rawLB!A:B");
        if (debug)
        {
            Debug.Log($"INFO: Succesfully added '{username}' with time of '{time.ToString()}' seconds to leaderboard!");
        }
    }

    public void AddPuzzleData(int roomNum, int puzzleNum, int timeTaken, int numOpened = -1)
    {
        string range = $"r{roomNum.ToString()}p{puzzleNum.ToString()}!A:B";
        var toAdd = new List<object>() {DateTime.Now.ToString(), timeTaken.ToString(), numOpened.ToString()};
        CreateEntry(toAdd, range);

        if (debug)
        {
            Debug.Log($"INFO: Succesfully added time data for room {roomNum.ToString()} puzzle {puzzleNum.ToString()} with time of '{timeTaken.ToString()}'");
        }
    }

    public void AddRoomData(string roomName, float startTime, float endTime) {
        string range = $"{roomName}!A:B";
        var toAdd = new List<object>() {DateTime.Now.ToString(), startTime.ToString(), endTime.ToString(), (endTime - startTime).ToString()};
        CreateEntry(toAdd, range);
        
        if (debug) {
            Debug.Log($"INFO: Succesfully added time data for room {roomName} with start time of '{startTime.ToString()}' and end time of '{endTime.ToString()}'");
        }
    }

    public void AddSurveyData(string Q1, string Q2, string Q3) {
        string range = $"survey!A:C";
        var toAdd = new List<object>() {DateTime.Now.ToString(), Q1, Q2, Q3};
        CreateEntry(toAdd, range);
        
        if (debug) {
            Debug.Log($"INFO: Succesfully added survey data with Q1 of '{Q1}' and Q2 of '{Q2}' and Q3 of '{Q3}'");
        }
    }
}