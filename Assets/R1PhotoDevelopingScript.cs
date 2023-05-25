using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R1PhotoDevelopingScript : MonoBehaviour
{
    public GameObject parentObject;
    private SpriteRenderer parentSprite;

    public GameObject player;
    private CharacterControl characterControl;

    public float checkInterval = 0.1f;
    private bool isVisible = true;

    // Start is called before the first frame update
    void Start()
    {
        parentSprite = parentObject.GetComponent<SpriteRenderer>();
        characterControl = player.GetComponent<CharacterControl>();
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
        foreach (SpriteRenderer child in parentSprite.GetComponentsInChildren<SpriteRenderer>())
        {
            child.enabled = true;
        }

        AdjustLocation();
    }

    private void OnSpriteRendererDisabled()
    {
        foreach (SpriteRenderer child in parentSprite.GetComponentsInChildren<SpriteRenderer>())
        {
            child.enabled = false;
        }
        characterControl.puzzleEnabled = false;
    }

    void AdjustLocation()
    {
        parentObject.transform.position = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
