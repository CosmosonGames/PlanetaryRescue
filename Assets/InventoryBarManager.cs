using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBarManager : MonoBehaviour
{
    [Header("Inventory")]
    public GameObject inventoryBar;
    
    [Header("Character")]   
    public GameObject player;
    private CharacterControl characterControl;

    // Start is called before the first frame update
    void Start()
    {
        characterControl = player.GetComponent<CharacterControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (characterControl.puzzleEnabled) {
            inventoryBar.SetActive(false);
        } else {
            inventoryBar.SetActive(true);
        }
    }
}
