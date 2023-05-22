using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R1Painting : MonoBehaviour
{
    public GameObject puzzle;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = puzzle.GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }

    private void OnMouseUp()
    {
        Debug.Log("Triggered mouse up");
        if (!spriteRenderer.enabled)
        {
            spriteRenderer.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
