using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R1PaintingPopUp : MonoBehaviour
{
    public float checkInterval = 0.1f;
    private bool isVisible = true;
    public GameObject parentObject;
    public GameObject player;
    private SpriteRenderer parentSprite;

    private CharacterControl characterControl;

    private void Start()
    {
        parentSprite = parentObject.GetComponent<SpriteRenderer>();
        characterControl = parentObject.GetComponent<CharacterControl>();

        AdjustScale();
        StartCoroutine(CheckVisibility());

    }

    private IEnumerator CheckVisibility()
    {
        while (true)
        {
            bool currentVisibility = parentSprite.isVisible;

            if (currentVisibility != isVisible)
            {
                isVisible = currentVisibility;

                if (isVisible)
                {
                    characterControl.puzzleEnabled = true;
                    OnSpriteRendererEnabled();
                }
                else
                {
                    OnSpriteRendererDisabled();
                }
            }

            yield return new WaitForSeconds(checkInterval);
        }
    }

    private void OnSpriteRendererEnabled()
    {
        AdjustLocation();
        Debug.Log(parentSprite.localBounds.size.x);

        Debug.Log("SpriteRenderer enabled");
    }

    private void OnSpriteRendererDisabled()
    {
        characterControl.puzzleEnabled = false;
        Debug.Log("SpriteRenderer disabled");
    }

    void AdjustScale()
    { 
        float widthPercentage = 1f;
        float heightPercentage = 1f;

        // Get the screen size in pixels
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;

        // Gtet the wanted sprite size in pixels
        float wantedX = screenWidth * widthPercentage;
        float wantedY = screenHeight * heightPercentage;

        // Get the sprite sizes

        float currentXScale = parentSprite.transform.localScale.x;
        float currentYScale = parentSprite.transform.localScale.y;

        Debug.Log($"Current X Scale: {currentXScale}");
        Debug.Log($"Current Y Scale: {currentYScale}");

        Debug.Log($"Sprite Rect Width: {parentSprite.sprite.rect.width}");

        float spriteWidth = parentSprite.sprite.rect.width / currentXScale;
        float spriteHeight = parentSprite.sprite.rect.height/ currentYScale;

        Debug.Log($"Sprite Width: {spriteWidth}");
        Debug.Log($"Sprite Height: {spriteHeight}");

        float actualXScale = wantedX / spriteWidth;
        float actualYScale = wantedY / spriteHeight;

        Debug.Log($"Actual X Scale: {actualXScale}");
        Debug.Log($"Actual Y Scale: {actualYScale}");

        Debug.Log($"Lossy Scale {parentSprite.transform.lossyScale}");
        Debug.Log($"Screen Size - width: {Screen.width}");

        Debug.Log($"True Local Scal: {transform.localScale}");
        Debug.Log($"'Local Scale' {transform.InverseTransformVector(transform.lossyScale)}");

        Debug.Log($"Sprite Bounds - X: {parentSprite.bounds.size.x}");

        Vector3 attemptedLocalScale = new Vector3(actualXScale, actualYScale, 1f);
        Debug.Log($"Global --> Local Scale: {transform.InverseTransformVector(attemptedLocalScale)}");

        // Set the size of the game object
        transform.localScale = new Vector3(actualXScale, actualYScale, 1f);

        Debug.Log("Give me breakpoint...");
    }

    void AdjustLocation()
    {
        parentObject.transform.position = player.transform.position;
    }
}
