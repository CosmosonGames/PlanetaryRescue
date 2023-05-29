using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class R1Anagrams : MonoBehaviour
{
    public GameObject letterA;
    private BoxCollider2D letterACollider;
    public GameObject letterB;
    private BoxCollider2D letterBCollider;

    public GameObject letterS;
    private BoxCollider2D letterSCollider;

    public GameObject letterG;
    private BoxCollider2D letterGCollider;

    public GameObject letterH;
    private BoxCollider2D letterHCollider;

    public GameObject letterI;
    private BoxCollider2D letterICollider;

    public GameObject letterN;
    private BoxCollider2D letterNCollider;

    public GameObject letterO;
    private BoxCollider2D letterOCollider;

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

    [Header("Slots")]
    public GameObject slot1;
    private BoxCollider2D slot1Collider;
    public GameObject slot2;
    private BoxCollider2D slot2Collider;

    public GameObject slot3;
    private BoxCollider2D slot3Collider;
    public GameObject slot4;
    private BoxCollider2D slot4Collider;

    [Header("Loading Bar")]
    public GameObject ComputerLoading;
    public GameObject antiLoadingBar;
    private RectTransform antiLoadingBarTransform;

    public TextMeshPro percentageText;
    public float loadingBarSpeed = 0.1f;

    private bool loadingBarAnimation = false;
    private bool partOneComplete = false; 
    private bool partTwoComplete = false;

    [Header("Inventory")]
    public GameObject inventory;
    private InventorySystem InventorySystem;

    public InventoryItemData UncodedKeycard;

    public InventoryItemData Keycard;

    private IEnumerator CheckVisibility()
    {
        while (true)
        {
            bool currentVisibility;
            if (!partOneComplete){ 
                currentVisibility = parentSprite.enabled;
            } else {
                currentVisibility = ComputerLoading.activeInHierarchy;
            }

            if (currentVisibility != isVisible && (!puzzleComplete || !partTwoComplete))
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

    private IEnumerator LoadingAnimation() {
        loadingBarAnimation = true;
        float xPos = 0f;
        float scale = 0.25f;
        float max = 1.6f;
        float percentage = 0f;
        //float unitScale = scale / max;

        System.Random rnd = new System.Random();
        while (xPos < max){
            float addition = (float)rnd.NextDouble() * (0.32f-0.16f) + 0.16f;
            if (xPos + addition > 1.6f){
                xPos = 1.6f;
            } else {
                xPos += addition;
            }

            percentage = xPos / max;

            Vector3 pos = antiLoadingBarTransform.localPosition;
            Vector3 currentScale = antiLoadingBarTransform.localScale;
            antiLoadingBarTransform.localPosition = new Vector3(xPos, pos.y, pos.z);
            antiLoadingBarTransform.localScale = new Vector3(scale * (1-percentage), currentScale.y, currentScale.z);

            percentageText.text = (int)(percentage * 100) + "%";
            yield return new WaitForSeconds(loadingBarSpeed);
        }
        if (logic.debug) {
            Debug.Log("completed loading animation");
        }
        loadingBarAnimation = false;
    }

    private void OnSpriteRendererEnabled()
    {
        if (!partOneComplete){ 
            VisibilityAnagaram(true);
            numOpen++;

            if (startTime == 0){
                startTime = logic.currentTime;
            }
        } else {
            VisibilityComputerLoading(true);
        }

        Debug.Log("sprite enabled");
    }

    private void VisibilityAnagaram(bool visibility){
        foreach (SpriteRenderer child in parentSprite.GetComponentsInChildren<SpriteRenderer>())
        {
            child.enabled = visibility;
        }
        parentSprite.enabled = visibility;

        if (visibility){
            parentObject.transform.position = player.transform.position;
        }

    }

    private void VisibilityComputerLoading(bool visibility) {
        int childCount = ComputerLoading.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = ComputerLoading.transform.GetChild(i);
            child.gameObject.SetActive(visibility);
        }
        ComputerLoading.SetActive(visibility);

        if (visibility){ 
            ComputerLoading.transform.position = player.transform.position;
        }
    }

    private void OnSpriteRendererDisabled()
    {
        if (!partOneComplete){ 
            VisibilityAnagaram(false);
        } else {
            VisibilityComputerLoading(false);
        }
        characterControl.puzzleEnabled = false;

        if (partOneComplete && puzzleComplete && !loadingBarAnimation){
            partTwoComplete = true;
        }
        Debug.Log("sprite disabled");
    }

    // Start is called before the first frame update
    void Start()
    {
        letters = new List<GameObject> { letterA, letterB, letterS, letterG, letterH, letterI, letterN, letterO };
        letterACollider = letterA.GetComponent<BoxCollider2D>();
        letterBCollider = letterB.GetComponent<BoxCollider2D>();
        letterSCollider = letterS.GetComponent<BoxCollider2D>();
        letterGCollider = letterG.GetComponent<BoxCollider2D>();
        letterHCollider = letterH.GetComponent<BoxCollider2D>();
        letterICollider = letterI.GetComponent<BoxCollider2D>();
        letterNCollider = letterN.GetComponent<BoxCollider2D>();
        letterOCollider = letterO.GetComponent<BoxCollider2D>();

        sheets = sheetsObject.GetComponent<SheetsManager>();
        slot1Collider = slot1.GetComponent<BoxCollider2D>();
        slot2Collider = slot2.GetComponent<BoxCollider2D>();
        slot3Collider = slot3.GetComponent<BoxCollider2D>();
        slot4Collider = slot4.GetComponent<BoxCollider2D>();

        antiLoadingBarTransform = antiLoadingBar.GetComponent<RectTransform>();

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
            CheckCompletion();
        } else if (partOneComplete && !loadingBarAnimation && ComputerLoading.activeInHierarchy){
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
            Collider2D computerLoadingCollider = ComputerLoading.GetComponent<Collider2D>();
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

            } else if (ComputerLoading.activeInHierarchy && colliders.Contains(computerLoadingCollider))
            {
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
        if (slot1Collider.bounds.Intersects(letterGCollider.bounds) && slot2Collider.bounds.Intersects(letterICollider.bounds) && slot3Collider.bounds.Intersects(letterOCollider.bounds) && slot4Collider.bounds.Intersects(letterNCollider.bounds) && selectedObject == null){
            timeTaken = Time.time - startTime;
            sheets.addRoomData(1, 3, (int)timeTaken, numOpen);

            VisibilityAnagaram(false);
            puzzleComplete = true;

            if (logic.debug)
            {
                Debug.Log("User successfully completed Anagram in Room #1.");
                Debug.Log($"Time Taken: {timeTaken}");
                Debug.Log($"Number of Attempts: {numOpen}");
            }

            PartTwo();
            partOneComplete = true;
        }
    }

    private void PartTwo() {
        VisibilityAnagaram(false);
        VisibilityComputerLoading(true);

        ComputerLoading.transform.position = player.transform.position;

        StartCoroutine(LoadingAnimation());
    }
}
