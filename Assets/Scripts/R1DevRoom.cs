using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R1DevRoom : MonoBehaviour
{
    public GameObject puzzle;
    private SpriteRenderer spriteRenderer;

    public GameObject player;
    private CharacterControl characterControl;

    private R1PhotoDevelopingScript photoDeveloping;

    [Header("Logic")]
    public GameObject logicObject;
    private LogicManagerScript logic;

    public bool authorized = false;

    [Header("Inventory")]
    public GameObject inventory;
    public InventoryItemData chipItem;
    public InventoryItemData cardItem;

    [Header("Error Manager")]
    public GameObject errorManager;
    private ErrorManager errorManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        characterControl = player.GetComponent<CharacterControl>();
        spriteRenderer = puzzle.GetComponent<SpriteRenderer>();
        photoDeveloping = puzzle.GetComponent<R1PhotoDevelopingScript>();
        logic = logicObject.GetComponent<LogicManagerScript>();
        errorManagerScript = errorManager.GetComponent<ErrorManager>();
 
        spriteRenderer.enabled = false;
        Debug.Log($"Sprite Enabled: {spriteRenderer.enabled}");
    }

    private void OnMouseUp()
    {
        if (!spriteRenderer.enabled && InventorySystem.current.Get(chipItem) != null && InventorySystem.current.Get(cardItem) != null)
        {
            Debug.Log($"Sprite Enabled: {spriteRenderer.enabled}");
            Debug.Log($"Character Control Puzzle Enabled: {characterControl.puzzleEnabled}");
            if (!spriteRenderer.enabled && !characterControl.puzzleEnabled && !photoDeveloping.puzzleComplete)
            {
                spriteRenderer.enabled = true;
                authorized = true;
            }
        } else if (spriteRenderer.enabled || authorized){
            spriteRenderer.enabled = true;
        } else{
            MissingItem();
        }
    }

    private void MissingItem(){
        List<string> missingItems = new List<string>();

        if (InventorySystem.current.Get(cardItem) == null)
        {
            missingItems.Add(cardItem.name);
        }
        if (InventorySystem.current.Get(chipItem) == null)
        {
            missingItems.Add(chipItem.name);
        }

        if (logic.debug) {
            foreach (string item in missingItems) {
                Debug.Log($"Missing Item: {item}");
            }
        }   
        errorManagerScript.ShowError(1);
    }
}