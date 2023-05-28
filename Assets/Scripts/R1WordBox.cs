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

    // Start is called before the first frame update
    void Start()
    {
        characterControl = player.GetComponent<CharacterControl>();
        spriteRenderer = puzzle.GetComponent<SpriteRenderer>();
        anagrams = puzzle.GetComponent<R1Anagrams>();
        spriteRenderer.enabled = false;
    }

    private void OnMouseUp()
    {
        if (!spriteRenderer.enabled && !characterControl.puzzleEnabled && !anagrams.puzzleComplete)
        {
            spriteRenderer.enabled = true;
        }
    }
}