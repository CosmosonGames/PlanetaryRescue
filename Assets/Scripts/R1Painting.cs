using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R1Painting : MonoBehaviour
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
        if (!spriteRenderer.enabled && !characterControl.puzzleEnabled)
        {
            spriteRenderer.enabled = true;
        }
    }
}
