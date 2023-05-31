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
    private bool isVisible = false;

    GameObject selectedObject;
    Vector3 offset;

    [SerializeField]
    List<GameObject> DoNotMove;

    [SerializeField]
    List<GameObject> moveableObjects;

    public bool puzzleComplete = false;

    public GameObject logicObject;
    private LogicManagerScript logic;

    [Header("Movable Objects")]
    public GameObject keycard;
    private SpriteRenderer keycardSprite;
    public GameObject chip;
    private SpriteRenderer chipSprite;

    [Header("Unmovable Objects")]
    public GameObject newCard;
    private SpriteRenderer newCardSprite;

    private BoxCollider2D newCardCollider;
    private BoxCollider2D chipCollider;
    private BoxCollider2D keycardCollider;

    [Header("Collider Game Objects")]
    public GameObject keycardSlot;
    public GameObject chipSlot;

    public GameObject newCardSlot;

    private BoxCollider2D keycardSlotCollider;
    private BoxCollider2D chipSlotCollider;
    private BoxCollider2D newCardSlotCollider;

    [Header("Logic")]

    private bool debug;

    private float startTime = 0f;
    public float timeTaken = 0f;

    public int numOpen;

    public GameObject sheetsObject;
    private SheetsManager sheets;

    [Header("Inventory")]
    public GameObject inventory;
    private InventorySystem inventorySystem;

    public InventoryItemData uncodedKeycard;
    public InventoryItemData cardItem;
    public InventoryItemData chipItem;
    private bool itemAdded = false;
    private bool pulledOut = false;
    
    public bool puzzleActive = false;

    public Collider2D hintsCollider;

    // Start is called before the first frame update
    void Start()
    {
        parentSprite = parentObject.GetComponent<SpriteRenderer>();
        characterControl = player.GetComponent<CharacterControl>();
        logic = logicObject.GetComponent<LogicManagerScript>();
        sheets = sheetsObject.GetComponent<SheetsManager>();
        inventorySystem = inventory.GetComponent<InventorySystem>();

        newCardCollider = newCard.GetComponent<BoxCollider2D>();
        chipCollider = chip.GetComponent<BoxCollider2D>();
        keycardCollider = keycard.GetComponent<BoxCollider2D>();

        keycardSlotCollider = keycardSlot.GetComponent<BoxCollider2D>();
        chipSlotCollider = chipSlot.GetComponent<BoxCollider2D>();
        newCardSlotCollider = newCardSlot.GetComponent<BoxCollider2D>();

        keycardSprite = keycard.GetComponent<SpriteRenderer>();
        chipSprite = chip.GetComponent<SpriteRenderer>();

        newCardSprite = newCard.GetComponent<SpriteRenderer>();
        newCardSprite.enabled = false;

        debug = logic.debug;

        StartCoroutine(CheckVisibility());
        Debug.Log("Visibility check started");
    }

    private IEnumerator CheckVisibility()
    {
        while (!puzzleComplete)
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
        if (!pulledOut) {
            PullFromInventory();
        }

        foreach (SpriteRenderer child in parentSprite.GetComponentsInChildren<SpriteRenderer>())
        {
            if (!itemAdded && child.gameObject == newCard) {
                child.enabled = false;
            } else {
                child.enabled = true;
            }
        }
        parentSprite.enabled = true;

        numOpen ++;
        if (logic.debug){
            Debug.Log("Photo Developing Puzzle Enabled");
        }

        AdjustLocation();

        if (startTime == 0f){
            startTime = logic.currentTime;
        }
    }

    private void OnSpriteRendererDisabled()
    {
        newCardSprite.enabled = false;
        foreach (SpriteRenderer child in parentSprite.GetComponentsInChildren<SpriteRenderer>())
        {
            child.enabled = false;
        }
        parentSprite.enabled = false;

        characterControl.puzzleEnabled = false;
    }

    void AdjustLocation()
    {
        parentObject.transform.position = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (parentSprite.enabled && pulledOut)
        {
            puzzleActive = true;
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
        OnSpriteRendererDisabled();
        if (itemAdded) {
            keycard.SetActive(false);
            chip.SetActive(false);
            newCard.SetActive(false);
        }
    }

    void CheckIfCorrect()
    {
        float keycardMinX = -1.762f;
        float keycardMaxX = -1.41f;
        float keycardMaxY = 0.264f;
        float keycardMinY = -0.064f;

        float chipMinX = -1.109f;
        float chipMaxX = -0.876f;
        float chipMaxY = 0.312f;
        float chipMinY = 0.094f;

        // Check if the sprites are in the correct positions & mouse is not dragging a sprite --> if so, load the next scene
        if ((keycardCollider.bounds.Intersects(keycardSlotCollider.bounds) && chipCollider.bounds.Intersects(chipSlotCollider.bounds)) || (keycard.transform.localPosition.x >= keycardMinX && keycard.transform.localPosition.x <= keycardMaxX && keycard.transform.localPosition.y >= keycardMinY && keycard.transform.localPosition.y <= keycardMaxY && chip.transform.localPosition.x >= chipMinX && chip.transform.localPosition.x <= chipMaxX && chip.transform.localPosition.y >= chipMinY && chip.transform.localPosition.y <= chipMaxY) && selectedObject == null)
        {
            timeTaken = Time.time - startTime;
            sheets.AddPuzzleData(1, 2, (int)timeTaken, numOpen);
            ActivateNewCard();
            puzzleComplete = true;

            if (debug)
            {
                Debug.Log("User successfully completed Photo Dev in Room #1.");
                Debug.Log($"Time Taken: {timeTaken}");
                Debug.Log($"Number of Attempts: {numOpen}");
            }

        }
    }

    void ActivateNewCard() {
        newCardSprite.enabled = true;
        AddToInventory();
        itemAdded = true;

        chip.SetActive(false);
        keycard.SetActive(false);
    }

    void AddToInventory() {
        if (InventorySystem.current.Get(uncodedKeycard) == null){
            InventorySystem.current.Add(uncodedKeycard);
            Debug.Log($"Added {uncodedKeycard.name} to inventory...");
        } else {
            if (debug){
                Debug.Log($"{uncodedKeycard.name} already in inventory...");
            }
        }
    }

    void PullFromInventory() {
        if (InventorySystem.current.Get(chipItem) != null && InventorySystem.current.Get(cardItem) != null){
            InventorySystem.current.Remove(chipItem);
            chipSprite.enabled = true;
            InventorySystem.current.Remove(cardItem);

            keycardSprite.enabled = true;
            pulledOut = true;

            if (debug){ 
                Debug.Log($"Removed {chipItem.name} and {cardItem.name} from inventory...");
            }
        } else {
            if (debug){
                Debug.Log($"{chipItem.name} and {cardItem.name} not in inventory...");
            }
        }
    }
}
