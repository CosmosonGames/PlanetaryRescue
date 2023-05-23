using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class R1PaintingPopUp : MonoBehaviour
{
    public float checkInterval = 0.1f;
    private bool isVisible = false;
    public GameObject parentObject;
    public GameObject player;
    private SpriteRenderer parentSprite;
    private Collider2D backgroundCollider;
    private CharacterControl characterControl;

    public GameObject logicObject;
    private LogicManagerScript logic;

    private bool debug;

    private void Start()
    {
        parentSprite = parentObject.GetComponent<SpriteRenderer>();
        characterControl = player.GetComponent<CharacterControl>();

        logic = logicObject.GetComponent<LogicManagerScript>();
        debug = logic.debug;

        parentSprite.enabled = false;

        Collider2D backgroundCollider = gameObject.GetComponent<Collider2D>();

        AdjustScale();
        StartCoroutine(CheckVisibility());

    }

    private void Update()
    {
        if (isVisible)
        {
            HandleMouse();
        }
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

        if (debug)
        {
            Debug.Log(parentSprite.localBounds.size.x);
            Debug.Log("SpriteRenderer enabled");
        }

    }

    private void OnSpriteRendererDisabled()
    {
        characterControl.puzzleEnabled = false;

        if (debug)
        {
            Debug.Log("SpriteRenderer disabled");
        }
    }

    void AdjustScale()
    {
        // Set the size of the game object
        transform.localScale = new Vector3(0.36f, 0.72f, 1f);
    }

    void AdjustLocation()
    {
        parentObject.transform.position = player.transform.position;
    }

    void HandleMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D[] colliders = Physics2D.OverlapPointAll(mousePosition);

            if (!colliders.Contains(backgroundCollider))
            {
                LeavePuzzle();
            }
        }
    }

    void LeavePuzzle()
    {
        parentSprite.enabled = false;
    }
}