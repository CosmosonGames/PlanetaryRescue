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

    private float startTime = 0f;
    public float timeTaken = 0f;

    public GameObject logicObject;
    private LogicManagerScript logic;

    public bool puzzleComplete = false;
    private int numOpen;
    public float timeToComplete = 0f;

    public GameObject sheetsObject;
    private SheetsManager sheets;

    private IEnumerator CheckVisibility()
    {
        while (true)
        {
            bool currentVisibility = parentSprite.enabled;

            if (currentVisibility != isVisible && !puzzleComplete)
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
        parentSprite.enabled = true;

        numOpen++;
        AdjustLocation();
        Debug.Log("sprite enabled");

        if (startTime == 0){
            startTime = logic.currentTime;
        }
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
        sheets = sheetsObject.GetComponent<SheetsManager>();

        logic = logicObject.GetComponent<LogicManagerScript>();
        parentSprite = parentObject.GetComponent<SpriteRenderer>();
        parentSprite.enabled = false;

        for (int i = letters.Count -1 ; i>=0; i--)
        {
            transforms.Add(letters[i].GetComponent<Transform>());
            Debug.Log(letters[i].name);
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
        // Broken randomization that's really annoying
        /* System.Random random = new System.Random();

        // Shuffle the list using the Fisher-Yates shuffle algorithm
        for (int i = transforms.Count -1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            Transform temp = transforms[i];
            transforms[i] = transforms[j];
            transforms[j] = temp;
        } 

        // Set the new positions of the game objects based on the shuffled transforms
        for (int i = letters.Count - 1; i>=0; i--)
        {
            GameObject obj = letters[i];
            obj.transform.position = transforms[i].position;
            Debug.Log(obj.name + " " + obj.transform.position);
        } */

        parentObject.transform.localScale = new Vector3(2.05f, 2.05f, 1f); 

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

    private void CheckCompletion(){
        //FORM SPAR
        if (false){
            timeTaken = Time.time - startTime;
            sheets.addRoomData(1, 1, (int)timeTaken, numOpen);

            LeavePuzzle();
            puzzleComplete = true;

            if (logic.debug)
            {
                Debug.Log("User successfully completed Photo Dev in Room #1.");
                Debug.Log($"Time Taken: {timeTaken}");
                Debug.Log($"Number of Attempts: {numOpen}");
            }
        }
    }
}
