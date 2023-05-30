using System.Collections;
using UnityEngine;
using TMPro;

public class SlowType : MonoBehaviour
{
    //Unity code for slow-type or animated typing of text
    public float delay = 0.1f;
    public string fullText;
    public bool isTyping;
    private string currentText = "";

    public bool INITIATE = false;
    public int runNum = 0;
    
    public TextMeshProUGUI textBox;

    public RectTransform backgroundTransform;

    IEnumerator ShowText(int currentRun, float waitTime = 0.0f)
    {
        textBox = GetComponent<TextMeshProUGUI>();
        isTyping = true;
        for(int i = 0; i <= fullText.Length; i++)
        {
            if (runNum == currentRun) {
                currentText = fullText.Substring(0, i);
                textBox.text = currentText;

                if (" " == fullText.Substring(i)){
                    yield return new WaitForSeconds(0.1f * delay);
                } else {
                    yield return new WaitForSeconds(delay);
                }
                // Set the width of the RectTransform
                backgroundTransform.sizeDelta = new Vector2(textBox.minWidth, backgroundTransform.sizeDelta.y);
            } else {
                StopCoroutine(ShowText(currentRun, waitTime));
            }
        }
        yield return new WaitForSeconds(waitTime);
        isTyping = false;
    }

    // Function for writing out the text
    public void WriteText(string text, float waitTime = 0.0f)
    {
        fullText = text;
        runNum ++;
        StartCoroutine(ShowText(currentRun: runNum, waitTime: waitTime));
    }

    public void Update(){
        if (INITIATE && !isTyping){
            WriteText(fullText);
            INITIATE = false;
        }
    }
}
