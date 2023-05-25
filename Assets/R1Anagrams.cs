using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class R1Anagrams : MonoBehaviour
{
    public GameObject letterA;
    public GameObject letterK;
    public GameObject letterP1;
    public GameObject letterP2;
    public GameObject letterQ;
    public GameObject letterS;
    public GameObject letterV;
    public GameObject letterX;

    List<Transform> transforms = new List<Transform>();
    List<GameObject> letters;

    GameObject selectedObject;
    Vector3 offset;

    public GameObject player;

    public GameObject parentObject;
    private SpriteRenderer parentSprite;
    private CharacterControl characterControl;

    public float checkInterval = 0.1f;
    private bool isVisible = true;

    [SerializeField]
    public List<GameObject> DoNotMove;

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
        Debug.Log("sprite enabled");
    }

    private void OnSpriteRendererDisabled()
    {
        foreach (SpriteRenderer child in parentSprite.GetComponentsInChildren<SpriteRenderer>())
        {
            child.enabled = false;
        }
        parentSprite.enabled = false;
        characterControl.puzzleEnabled = false;
        Debug.Log("sprite disabled");
    }

    // Start is called before the first frame update
    void Start()
    {
        letters = new List<GameObject> { letterA, letterK, letterP1, letterP2, letterQ, letterS, letterV, letterX };

        parentSprite = parentObject.GetComponent<SpriteRenderer>();
        parentSprite.enabled = false;

        foreach (GameObject letter in letters)
        {
            transforms.Add(letter.GetComponent<Transform>());
        }

        characterControl = player.GetComponent<CharacterControl>();

        PositionObjects();

        StartCoroutine(CheckVisibility());
    }

    // Update is called once per frame
    void Update()
    {
        if (parentSprite.enabled)
        {
            HandleMouse();
        }
    }

    void PositionObjects()
    {
        System.Random random = new System.Random();

        // Shuffle the list using the Fisher-Yates shuffle algorithm
        for (int i = letters.Count - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            GameObject temp = letters[i];
            letters[i] = letters[j];
            letters[j] = temp;
        }

        int count = 0;
        foreach (GameObject obj in letters)
        {
            obj.transform.position = transforms[count].position;
            obj.transform.rotation = transforms[count].rotation;
            obj.transform.localScale = transforms[count].localScale;
            count++;
        }

        parentObject.transform.localScale = new Vector3(0.8f, 0.8f, 1f); 

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
                    if (!DoNotMove.Contains(targetObject) && letters.Contains(targetObject))
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
            selectedObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
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

    private void LeavePuzzle()
    {
        OnSpriteRendererDisabled();
    }

    private void AdjustLocation()
    {
        parentObject.transform.position = player.transform.position;
    }
}
