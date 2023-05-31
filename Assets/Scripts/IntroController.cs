using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
    private List<string> dialog = new List<string>() {"You've finally made it.", "But…why are you sleeping? Come on!", "You've only got a bit more to go, you can't give up now!", "Don't you want to save your planet? They're going to destroy it, don't you remember?", "...", "No?", "I'll help you remember then.  I can't save Hermes without you. Our home planet.", "If we can disable their missile then you'll have saved us all.", "Come on, wake up.", "I'll be waiting for you."};

    public GameObject topBlink;
    public GameObject bottomBlink;

    private float blinkMS = 85f;
    private float blinkDelta = 20f;
    private float topBlinkOpen = 8.0967f;
    private float topBlinkClosed = 2.6989f;

    private float bottomBlinkOpen = -8.1615f;
    private float bottomBlinkClosed = -2.7205f;

    private float bottomDistance = -5.441f;
    private float topDistance = 5.3978f;

    public GameObject slowTypeObject;
    private OldSlowType slowType;

    private bool eyeInMotion = false;

    private IEnumerator GoToSleep() {
        yield return StartCoroutine(OpenEyes(randomBlinkSpeed(100, 0)));
        yield return StartCoroutine(CloseEyes(randomBlinkSpeed(175, 0)));
        yield return StartCoroutine(OpenEyes(randomBlinkSpeed(150, 0)));
        yield return StartCoroutine(CloseEyes(randomBlinkSpeed(300, 0)));

        yield return new WaitForSeconds(2f);

        slowType.delay = 0.1f;
        foreach (string text in dialog) {
            slowType.WriteText(text);
            while (slowType.isTyping){
                yield return new WaitForSeconds(0.01f);
            }
            yield return new WaitForSeconds(1f);
        }  
        SceneManager.LoadScene("MainScene");
    }

    private float randomBlinkSpeed(float blinkMS, float blinkDelta) {
        System.Random rnd = new System.Random();
        float randomValue = (float)Math.Tanh(rnd.NextDouble());
        return randomValue * blinkDelta + blinkMS;
    } 

    void Start(){
        slowType = slowTypeObject.GetComponent<OldSlowType>();

        StartCoroutine(GoToSleep());
    }

    private IEnumerator CloseEyes(float ms){
        float topAdvance = topDistance/ms;
        float bottomAdvance = bottomDistance/ms;    

        eyeInMotion = true;
        while (topBlink.transform.position.y > topBlinkClosed && bottomBlink.transform.position.y < bottomBlinkClosed){
            topBlink.transform.position = new Vector3(0f, topBlink.transform.position.y - topAdvance, 0f);
            bottomBlink.transform.position = new Vector3(0f, bottomBlink.transform.position.y - bottomAdvance, 0f);
            yield return new WaitForSeconds(0.01f);
        }
        eyeInMotion = false;
    }

    private IEnumerator OpenEyes(float ms) {
        float topAdvance = topDistance/ms;
        float bottomAdvance = bottomDistance/ms;

        eyeInMotion = true;
        while (topBlink.transform.position.y < topBlinkOpen && bottomBlink.transform.position.y > bottomBlinkOpen){
            topBlink.transform.position = new Vector3(0f, topBlink.transform.position.y + topAdvance, 0f);
            bottomBlink.transform.position = new Vector3(0f, bottomBlink.transform.position.y + bottomAdvance, 0f);
            yield return new WaitForSeconds(0.01f);
        }
        eyeInMotion = false;
    }
}
