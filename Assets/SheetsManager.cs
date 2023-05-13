using System;
using System.Collections.Generic;
using System.IO;

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

    static private SheetsService service;

    public LogicManagerScript logic;

    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicManagerScript>();

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

        if (logic.debug)
        {
            var toAdd = new List<object>() { "YAY", 1 };
            CreateEntry(toAdd, "Sheet1!A:B");

            var data = ReadEntries("Sheet1!A:B");

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
}