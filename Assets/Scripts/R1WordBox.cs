using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R1WordBox : MonoBehaviour
{
    public GameObject puzzle;
    private SpriteRenderer spriteRenderer;

    public GameObject player;
    private CharacterControl characterControl;

    private R1Anagrams anagrams;

    [Header("Logic")]
    public GameObject logicObject;
    private LogicManagerScript logic;

    [Header("Inventory")]
    public GameObject inventory;
    private InventorySystem InventorySystem;
    public InventoryItemData embeddedCardItem;

    [Header("Error Manager")]
    public GameObject errorManager;
    private ErrorManager errorManagerScript;

    private bool isAuthorized = false;

    // Start is called before the first frame update
    void Start()
    {
        characterControl = player.GetComponent<CharacterControl>();
        spriteRenderer = puzzle.GetComponent<SpriteRenderer>();
        anagrams = puzzle.GetComponent<R1Anagrams>();
        logic = logicObject.GetComponent<LogicManagerScript>();
        errorManagerScript = errorManager.GetComponent<ErrorManager>();
        spriteRenderer.enabled = false;
    }

    private void OnMouseUp()
    {
        if (logic.debug) {
            Debug.Log("Detected mouse up on word box");
        }
        if (InventorySystem.current.Get(embeddedCardItem) != null || isAuthorized)
        {
            isAuthorized = true;
            if (logic.debug) {
                Debug.Log($"Sprite Enabled: {spriteRenderer.enabled}");
                Debug.Log($"Character Control Puzzle Enabled: {characterControl.puzzleEnabled}");
            }
            if (!spriteRenderer.enabled && !characterControl.puzzleEnabled && !anagrams.puzzleComplete)
            {
                spriteRenderer.enabled = true;
            }
        } else {
            MissingItem();
        }
    }

    private void MissingItem(){
        errorManagerScript.ShowError(2);
        if (logic.debug) {
            Debug.Log($"Missing Item: {embeddedCardItem.name}");
        }   
    }
}