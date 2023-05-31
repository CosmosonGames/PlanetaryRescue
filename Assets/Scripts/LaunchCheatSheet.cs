using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchCheatSheet : MonoBehaviour
{

    public GameObject player;
    private CharacterControl characterControl;

    public GameObject cheatsheet;
    private SpriteRenderer cheatsheetSpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        characterControl = player.GetComponent<CharacterControl>();
        cheatsheetSpriteRenderer = cheatsheet.GetComponent<SpriteRenderer>();

        cheatsheetSpriteRenderer.enabled = false;
    }

    private void OnMouseUp()
    {
        characterControl = player.GetComponent<CharacterControl>();
        Debug.Log(characterControl);
        Debug.Log(characterControl.puzzleEnabled);
        if (!cheatsheetSpriteRenderer.enabled && !characterControl.puzzleEnabled)
        {
            ToggleCheatSheet(true);
            cheatsheetSpriteRenderer.transform.position = player.transform.position;
        } else {
            ToggleCheatSheet(false);
        }
    }

    private void Update() {
        if (cheatsheetSpriteRenderer && Input.GetMouseButtonDown(0)) {
            ToggleCheatSheet(false);
        }
    }

    private void ToggleCheatSheet(bool enabled) {
        cheatsheetSpriteRenderer.enabled = enabled;
        characterControl.puzzleEnabled =  enabled;
        characterControl.examineEnabled =  enabled;

    }
}
