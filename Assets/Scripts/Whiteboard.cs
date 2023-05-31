using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whiteboard : MonoBehaviour
{
    public Sprite whiteboardWithPassword;
    public Sprite whiteboardWithoutPassword;

    private SpriteRenderer currentSprite;

    // Start is called before the first frame update
    void Start()
    {
        currentSprite = GetComponent<SpriteRenderer>();
    }

    void OnMouseUp() {
        if (currentSprite.sprite == whiteboardWithPassword)
        {
            currentSprite.sprite = whiteboardWithoutPassword;
        }
        else
        {
            currentSprite.sprite = whiteboardWithPassword;
        }
    }
}
