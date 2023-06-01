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

    public GameObject UnlockedSafe;
    private SpriteRenderer unlockedSafeSpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        characterControl = player.GetComponent<CharacterControl>();
        spriteRenderer = puzzle.GetComponent<SpriteRenderer>();
        tablePuzzle = puzzle.GetComponent<R1TablePuzzleScript>();
        logic = logicManager.GetComponent<LogicManagerScript>();
        unlockedSafeSpriteRenderer = UnlockedSafe.GetComponent<SpriteRenderer>();


        spriteRenderer.enabled = false;
    }

    private void OnMouseUp()
    {
        if (!spriteRenderer.enabled && !characterControl.puzzleEnabled && !tablePuzzle.puzzleComplete)
        {
            if (tablePuzzle.unlockComplete) {
                unlockedSafeSpriteRenderer.enabled = true;
            } else {
                spriteRenderer.enabled = true;
            }
            Debug.Log("enter 49 bp at r1t");
        }
    }
}