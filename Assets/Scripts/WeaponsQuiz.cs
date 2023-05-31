using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsQuiz : MonoBehaviour
{
    GameObject selectedObject;
    Vector3 offset;

    public GameObject player;

    public GameObject parentObject;
    private SpriteRenderer parentSprite;

    public BoxCollider2D weapon1Collider;
    public BoxCollider2D weapon2Collider;
    public BoxCollider2D weapon3Collider;
    public BoxCollider2D weapon4Collider;
    
    public BoxCollider2D weaponSlot1Collider;
    public BoxCollider2D weaponSlot2Collider;
    public BoxCollider2D weaponSlot3Collider;
    public BoxCollider2D weaponSlot4Collider;

    public float checkInterval = 0.1f;
    private bool isVisible = false;

    private CharacterControl characterControl;

    public GameObject logicObject;
    private LogicManagerScript logic;

    public GameObject sheetsObject;
    private SheetsManager sheets;

    private bool debug;

    public bool puzzleComplete = false;
    private float startTime;
    public float timeTaken;
    
    private int numOpen = 0;

    private List<BoxCollider2D> moveableObjects = new List<BoxCollider2D>();

    public bool puzzleActive;

    public Collider2D hintsCollider;

    private IEnumerator CheckVisibility()
    {
        while (!puzzleComplete)
        {
            bool currentVisibility;
            currentVisibility = parentSprite.enabled;

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
        if (startTime == 0) {
            startTime = logic.currentTime;
        }
        characterControl.puzzleEnabled = true;
        PuzzleVisibility(true);
        numOpen ++;
        puzzleActive = true;
        Debug.Log(numOpen);
        AdjustLocation();
    }

    private void PuzzleVisibility(bool enable){ 
        foreach (Transform child in parentObject.transform)
        {
            child.gameObject.SetActive(true);
        }
        parentObject.SetActive(enable);
        characterControl.puzzleEnabled = enable;
        puzzleActive = false;
    }
 
    private void OnSpriteRendererDisabled()
    {
        PuzzleVisibility(false);

        if (debug)
        {
            Debug.Log("R1TablePuzzle sprites disabled");
        }

        characterControl.puzzleEnabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        logic = logicObject.GetComponent<LogicManagerScript>();
        sheets = sheetsObject.GetComponent<SheetsManager>();

        debug = logic.debug;
        parentSprite = parentObject.GetComponent<SpriteRenderer>();

        characterControl = player.GetComponent<CharacterControl>();

        moveableObjects.Add(weapon1Collider);
        moveableObjects.Add(weapon2Collider);
        moveableObjects.Add(weapon3Collider);
        moveableObjects.Add(weapon4Collider);

        StartCoroutine(CheckVisibility());
    }

    // Update is called once per frame
    void Update()
    {
        if (parentObject.activeInHierarchy && !puzzleComplete)
        {
            HandleMouse();
            CheckIfCorrect();
        }
    }

    void HandleMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D[] colliders = Physics2D.OverlapPointAll(mousePosition);

            Collider2D backgroundCollider = gameObject.GetComponent<Collider2D>();
            if (colliders.Contains(backgroundCollider) || colliders.Contains(hintsCollider))
            {
                foreach (Collider2D collider in colliders)
                {
                    GameObject targetObject = collider.gameObject;
                    if (!puzzleComplete && moveableObjects.Contains(collider))
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

    void CheckIfCorrect()
    {
        // Check if the sprites are in the correct positions & mouse is not dragging a sprite --> if so, load the next scene
        if (weapon1Collider.bounds.Intersects(weaponSlot1Collider.bounds) && weapon2Collider.bounds.Intersects(weaponSlot2Collider.bounds) && weapon3Collider.bounds.Intersects(weaponSlot3Collider.bounds) && weapon4Collider.bounds.Intersects(weaponSlot4Collider.bounds) && selectedObject == null)
        {
            //CHECK MARK
            float totalTime = logic.currentTime - startTime;
            sheets.AddPuzzleData(2, 1, (int)totalTime, numOpen);
            PuzzleVisibility(false);
            puzzleComplete = true;

            if (debug)
            {
                Debug.Log("User successfully unlocked Puzzle #2 in Room #2.");
                Debug.Log($"Time Taken: {timeTaken}");
                Debug.Log($"Number of Attempts: {numOpen}");
            }
        }

    }

    void AdjustLocation()
    {
        parentObject.transform.position = player.transform.position;
    }

    void LeavePuzzle()
    {
        OnSpriteRendererDisabled();
    }
}
