using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class R1PhotoDevelopingScript : MonoBehaviour
{
    public GameObject parentObject;
    private SpriteRenderer parentSprite;

    public GameObject player;
    private CharacterControl characterControl;

    public float checkInterval = 0.1f;
    private bool isVisible = true;

    GameObject selectedObject;
    Vector3 offset;

    [SerializeField]
    List<GameObject> DoNotMove;

    [SerializeField]
    List<GameObject> moveableObjects;

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
        if (parentSprite.enabled)
        {
            HandleMouse();
        }
    }

    void HandleMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D[] colliders = Physics2D.OverlapPointAll(mousePosition);

            Collider2D backgroundCollider = gameObject.GetComponent<Collider2D>();
            if (colliders.Contains(backgroundCollider))
            {
                foreach (Collider2D collider in colliders)
                {
                    GameObject targetObject = collider.gameObject;
                    if (!DoNotMove.Contains(targetObject) && moveableObjects.Contains(targetObject))
                    {
                        selectedObject = targetObject.transform.gameObject;
                        offset = selectedObject.transform.position - mousePosition;
                        break;
                    }
                }

            }
            else
            {
                LeavePuzzle();

            }
        }
        if (selectedObject)
        {
            // Check if the mouse position is over a collider
            Collider2D[] colliders = Physics2D.OverlapCircleAll(mousePosition, 0.1f);
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.name == parentObject.name)
                {
                    // Adjust the position of the object to stay within the collider
                    Vector3 newPosition = mousePosition + offset;
                    Vector3 colliderPosition = collider.transform.position;
                    Vector3 colliderSize = collider.bounds.size;
                    float xMin = colliderPosition.x - colliderSize.x / 2 + selectedObject.GetComponent<SpriteRenderer>().bounds.size.x / 2;
                    float xMax = colliderPosition.x + colliderSize.x / 2 - selectedObject.GetComponent<SpriteRenderer>().bounds.size.x / 2;
                    float yMin = colliderPosition.y - colliderSize.y / 2 + selectedObject.GetComponent<SpriteRenderer>().bounds.size.y / 2;
                    float yMax = colliderPosition.y + colliderSize.y / 2 - selectedObject.GetComponent<SpriteRenderer>().bounds.size.y / 2;
                    newPosition.x = Mathf.Clamp(newPosition.x, xMin, xMax);
                    newPosition.y = Mathf.Clamp(newPosition.y, yMin, yMax);
                    selectedObject.transform.position = newPosition;
                    break;
                }
            }
        }
        if (Input.GetMouseButtonUp(0) && selectedObject)
        {
            selectedObject = null;
        }

    }

    void LeavePuzzle()
    {
        parentSprite.enabled = false;

    }

}
