using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R1Table : MonoBehaviour
{
    public GameObject puzzle;
    private SpriteRenderer spriteRenderer;

    public GameObject player;
    private CharacterControl characterControl;

    private R1TablePuzzleScript tablePuzzle;

    public GameObject logicManager;
    private LogicManagerScript logic;

    // Start is called before the first frame update
    void Start()
    {
        characterControl = player.GetComponent<CharacterControl>();
        spriteRenderer = puzzle.GetComponent<SpriteRenderer>();
        tablePuzzle = puzzle.GetComponent<R1TablePuzzleScript>();
        logic = logicManager.GetComponent<LogicManagerScript>();


        spriteRenderer.enabled = false;
    }

    private void OnMouseUp()
    {
        if (logic.debug){
            Debug.Log("logged mouse up");
            Debug.Log(characterControl.puzzleEnabled);
            Debug.Log(spriteRenderer.enabled);
        }

        if (!spriteRenderer.enabled && !characterControl.puzzleEnabled && !tablePuzzle.puzzleComplete)
        {
            spriteRenderer.enabled = true;
        }
    }
}