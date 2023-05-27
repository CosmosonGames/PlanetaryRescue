using System.Collections;
using System.Collections.Generic;
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
    
    public TextMeshPro textBox;

    IEnumerator ShowText()
    {
        textBox = GetComponent<TextMeshPro>();
        isTyping = true;
        for(int i = 0; i <= fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i);
            textBox.text = currentText;
            yield return new WaitForSeconds(delay);
        }
        isTyping = false;
    }

    // Function for writing out the text
    public void WriteText(string text)
    {
        fullText = text;
        StartCoroutine(ShowText());
    }

    public void Update(){
        if (INITIATE && !isTyping){
            WriteText(fullText);
            INITIATE = false;
        }
    }
}
