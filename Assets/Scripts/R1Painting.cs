using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R1Painting : MonoBehaviour
{
    public CentralControlScript canvasControlScript;
    public GameObject sprite;
    
    private Transform canvasRectTransform;
    private SpriteRenderer spriteRenderer;

    public int x = 1;
    public int y = 1;

    // Start is called before the first frame update
    void Start()
    {
        canvasRectTransform = sprite.GetComponent<Transform>();
        spriteRenderer = sprite.GetComponent<SpriteRenderer>();
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
        if (spriteRenderer.enabled && Input.GetKey(KeyCode.F))
        {
            //canvasControlScript.adjustCanvas(canvasRectTransform, spriteRenderer, x, y);
        }
    }
}
