using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SendSurvey : MonoBehaviour
{
    public GameObject sheetsManager;
    private SheetsManager sheets;

    public TMP_InputField Q1;
    public TMP_InputField Q2;
    public TMP_InputField Q3;

    private bool sent = false;
    
    public void Start() {
        sheets = sheetsManager.GetComponent<SheetsManager>();
    }

    public void SendSurveyToSheets() {
        if (!sent) {
            sheets.AddSurveyData(Q1.text, Q2.text, Q3.text);
            Q1.enabled = false;
            Q2.enabled = false;
            Q3.enabled = false;
            sent = true;
        }
    }
}
