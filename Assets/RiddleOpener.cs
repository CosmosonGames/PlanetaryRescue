using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiddleOpener : MonoBehaviour
{
    public GameObject puzzle;
    private SpriteRenderer spriteRenderer;

    public GameObject player;
    private CharacterControl characterControl;

    [Header("Logic")]
    public GameObject logicObject;
    private LogicManagerScript logic;

    public bool authorized = false;

    [Header("Previous Puzzles")]
    public GameObject ShootingPuzzleObject;
    private ShootingPuzzle shootingPuzzle;

    public GameObject WeaponsQuizObject;
    private WeaponsQuiz weaponsQuiz;

    // Start is called before the first frame update
    void Start()
    {
        characterControl = player.GetComponent<CharacterControl>();
        spriteRenderer = puzzle.GetComponent<SpriteRenderer>();
        logic = logicObject.GetComponent<LogicManagerScript>();
        shootingPuzzle = ShootingPuzzleObject.GetComponent<ShootingPuzzle>();
        weaponsQuiz = WeaponsQuizObject.GetComponent<WeaponsQuiz>();
 
        spriteRenderer.enabled = false;
        Debug.Log($"Sprite Enabled: {spriteRenderer.enabled}");
    }

    private void OnMouseUp()
    {
        if (!authorized) {
            Authorization();
        }
        if (!spriteRenderer.enabled && authorized && !characterControl.puzzleEnabled)
        {
            TogglePuzzle(true);
        } 
    }

    private void Authorization(){
        if (weaponsQuiz.puzzleComplete && shootingPuzzle.puzzleComplete){
            authorized = true;
        } else {
            authorized = false;
        }
    }

    private void TogglePuzzle(bool enabled) {
        spriteRenderer.enabled = enabled;
        characterControl.puzzleEnabled = enabled;

        if (enabled) {
            spriteRenderer.transform.position = player.transform.position;
        }
    }
}
