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

    private bool puzzleComplete = false;

    public GameObject logicObject;
    private LogicManagerScript logic;

    [Header("Movable Objects")]
    public GameObject camera1;
    public GameObject camera2;
    public GameObject filmCanister;
    public GameObject cameraFilm1;
    public GameObject cameraFilm2;
    public GameObject blankPolaroid1;
    public GameObject blankPolaroid2;

    private BoxCollider2D camera1Collider;
    private BoxCollider2D camera2Collider;
    private BoxCollider2D filmCanisterCollider;
    private BoxCollider2D cameraFilm1Collider;
    private BoxCollider2D cameraFilm2Collider;
    private BoxCollider2D blankPolaroid1Collider;
    private BoxCollider2D blankPolaroid2Collider;

    [Header("Collider Game Objects")]
    public GameObject topline;
    public GameObject bottomLine;

    public GameObject exPolaroid1;
    public GameObject exPolaroid2;
    public GameObject exPolaroid3;

    public GameObject topShelf;
    public GameObject bottomShelf;

    public GameObject counterTop;

    private PolygonCollider2D toplineCollider;
    private PolygonCollider2D bottomLineCollider;
    private BoxCollider2D exPolaroid1Collider;
    private BoxCollider2D exPolaroid2Collider;
    private BoxCollider2D exPolaroid3Collider;
    private PolygonCollider2D topShelfCollider;
    private PolygonCollider2D bottomShelfCollider;
    private PolygonCollider2D counterTopCollider;

    private bool debug;

    private float startTime = 0f;
    public float timeTaken = 0f;

    // Start is called before the first frame update
    void Start()
    {
        parentSprite = parentObject.GetComponent<SpriteRenderer>();
        characterControl = player.GetComponent<CharacterControl>();
        logic = logicObject.GetComponent<LogicManagerScript>();

        camera1Collider = camera1.GetComponent<BoxCollider2D>();
        camera2Collider = camera2.GetComponent<BoxCollider2D>();
        filmCanisterCollider = filmCanister.GetComponent<BoxCollider2D>();
        cameraFilm1Collider = cameraFilm1.GetComponent<BoxCollider2D>();
        cameraFilm2Collider = cameraFilm2.GetComponent<BoxCollider2D>();
        blankPolaroid1Collider = blankPolaroid1.GetComponent<BoxCollider2D>();
        blankPolaroid2Collider = blankPolaroid2.GetComponent<BoxCollider2D>();

        toplineCollider = topline.GetComponent<PolygonCollider2D>();
        bottomLineCollider = bottomLine.GetComponent<PolygonCollider2D>();
        exPolaroid1Collider = exPolaroid1.GetComponent<BoxCollider2D>();
        exPolaroid2Collider = exPolaroid2.GetComponent<BoxCollider2D>();
        exPolaroid3Collider = exPolaroid3.GetComponent<BoxCollider2D>();
        topShelfCollider = topShelf.GetComponent<PolygonCollider2D>();
        bottomShelfCollider = bottomShelf.GetComponent<PolygonCollider2D>();
        counterTopCollider = counterTop.GetComponent<PolygonCollider2D>();

        debug = logic.debug;

        StartCoroutine(CheckVisibility());

    }

    private IEnumerator CheckVisibility()
    {
        while (true && !puzzleComplete)
        {
            bool currentVisibility = parentSprite.enabled;

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

        if (startTime == 0f){
            startTime = logic.currentTime;
        }
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

    void CheckIfCorrect()
    {
        // Check if the sprites are in the correct positions & mouse is not dragging a sprite --> if so, load the next scene
        if ((camera1Collider.bounds.Intersects(topShelfCollider.bounds) || camera1Collider.bounds.Intersects(bottomShelfCollider.bounds)) && (camera2Collider.bounds.Intersects(topShelfCollider.bounds) || camera2Collider.bounds.Intersects(bottomShelfCollider.bounds)) && filmCanisterCollider.bounds.Intersects(counterTopCollider.bounds) && cameraFilm1Collider.bounds.Intersects(counterTopCollider.bounds) && cameraFilm2Collider.bounds.Intersects(counterTopCollider.bounds) && (blankPolaroid1Collider.bounds.Intersects(toplineCollider.bounds) || blankPolaroid1Collider.bounds.Intersects(bottomLineCollider.bounds)) && (blankPolaroid2Collider.bounds.Intersects(toplineCollider.bounds) || blankPolaroid2Collider.bounds.Intersects(bottomLineCollider.bounds)))
        {
            //CHECK MARK

            LeavePuzzle();
            puzzleComplete = true;

            timeTaken = startTime - logic.currentTime;

            if (debug)
            {
                Debug.Log("User successfully completed Table Puzzle in Room #2.");
                Debug.Log($"Time Taken: {timeTaken.ToString()}");
            }
        }
    }

}
