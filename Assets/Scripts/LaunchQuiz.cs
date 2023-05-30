using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchQuiz : MonoBehaviour
{
    public GameObject quiz;
    private SpriteRenderer apSpriteRenderer;

    public GameObject player;
    private CharacterControl characterControl;

    [Header("Logic")]
    public GameObject logicObject;
    private LogicManagerScript logic;

    public bool puzzleComplete = false;

    // Start is called before the first frame update
    void Start()
    {
        characterControl = player.GetComponent<CharacterControl>();
        logic = logicObject.GetComponent<LogicManagerScript>();
        apSpriteRenderer = quiz.GetComponent<SpriteRenderer>();
        foreach (Transform child in transform)
        {
            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
            spriteRenderer.gameObject.SetActive(false);
        } 
    }

    private void OnMouseUp()
    {
        Debug.Log("Clicked");
        if (!puzzleComplete && (!apSpriteRenderer.gameObject.activeInHierarchy) && !characterControl.puzzleEnabled)
        {
            EnablePuzzle();
        }
    }

    void EnablePuzzle()
    {
        apSpriteRenderer.gameObject.SetActive(true);
        foreach (Transform child in transform)
        {
            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
            spriteRenderer.gameObject.SetActive(true);
        } 
        characterControl.puzzleEnabled = true;
        apSpriteRenderer.transform.position = player.transform.position;
    }
}
