using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R1Table : MonoBehaviour
{
    public GameObject puzzle;
    private SpriteRenderer spriteRenderer;

    public GameObject player;
    private CharacterControl characterControl;

    // Start is called before the first frame update
    void Start()
    {
        characterControl = player.GetComponent<CharacterControl>();
        spriteRenderer = puzzle.GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }

    private void OnMouseUp()
    {
        Debug.Log("logged mouse up");
        Debug.Log(characterControl.puzzleEnabled);
        Debug.Log(spriteRenderer.enabled);
        if (!spriteRenderer.enabled && !characterControl.puzzleEnabled)
        {
            spriteRenderer.enabled = true;
        }
    }
}