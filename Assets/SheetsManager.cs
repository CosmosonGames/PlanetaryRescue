using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using UnityEngine;

public class SheetsManager : MonoBehaviour
{
    static private String spreadsheetId = "1vaJzdKDZP85q5CDc6RHnONkJTOlMGAFX3Mdioqn4a98";
    static private String jsonPath = "/Creds/cosmosongames-0c17ee530b7b.json";
    static private String serviceAccountID = "planetary@cosmosongames.iam.gserviceaccount.com";
    static private SheetsService service;

    void Start()
    {
        String fullJsonPath = Application.dataPath + jsonPath;

        Stream jsonCreds = (Stream)File.Open(fullJsonPath, FileMode.Open);

        ServiceAccountCredential credential = ServiceAccountCredential.FromServiceAccountData(jsonCreds);

        service = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
        });
    }

    public IList<IList<object>> getSheetRange(String sheetNameAndRange)
    {
        SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, sheetNameAndRange);

        ValueRange response = request.Execute();
        IList<IList<object>> values = response.Values;
        if (values != null && values.Count > 0)
        {
            return values;
        }
        else
        {
            Debug.Log("No data found.");
            return null;
        }
    }
}
