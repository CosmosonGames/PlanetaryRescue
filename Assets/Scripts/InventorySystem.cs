using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    private Dictionary<InventoryItemData, InventoryItem> m_itemDictionary;

    public List<InventoryItem> inventory { get; private set; }

    public static InventorySystem current;
    public GameObject m_slotPrefab;
    public event Action onInventoryChangedEvent;

    public GameObject inventoryDock;
    private HorizontalLayoutGroup inventoryDockLayoutGroup;

    [Header("Misc")]
    public Sprite StackBoxBackground;

    private List<InventoryItem> drawnInventory;

    public InventoryItemData testItem;

    private void Update(){
        DrawInventory();
    }

    private void Awake()
    {
        inventory = new List<InventoryItem>();
        m_itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();

    }

    private void Start()
    {
        current = this;
        current.onInventoryChangedEvent += OnUpdateInventory;
        drawnInventory = new List<InventoryItem>();
        inventoryDockLayoutGroup = inventoryDock.GetComponent<HorizontalLayoutGroup>();
        inventoryDockLayoutGroup.enabled = false;
    }

    private void OnUpdateInventory()
    {
        DrawInventory();
    }

    public void DrawInventory()
    {
        if (inventoryDock.transform.childCount != 0 || inventory.Count != 0)
        {
            inventoryDockLayoutGroup.enabled = true;
        } else {
            inventoryDockLayoutGroup.enabled = false;
            return;
        }

        foreach (InventoryItem item in current.inventory)
        {
            if (!drawnInventory.Contains(item)) {
                AddInventorySlot(item);
            } 
        }

        List<InventoryItem> itemsToRemove = new List<InventoryItem>();
        foreach (InventoryItem item in drawnInventory)
        {
            if (!current.inventory.Contains(item))
            {
                Debug.Log(item.Data.name);
                itemsToRemove.Add(item);
                Debug.Log(current.inventory.Count);
            } 
        }
        foreach (InventoryItem item in itemsToRemove)
        {
            Debug.Log(item.Data.name);
            drawnInventory.Remove(item);
            RemoveInventorySlot(item);
        }

        if (inventory.Count == 0)
        {
            inventoryDockLayoutGroup.enabled = false;
        } 
    }

    public void AddInventorySlot(InventoryItem item)
    {
        GameObject obj = Instantiate(m_slotPrefab);
        obj.name = item.Data.name;
        obj.transform.SetParent(transform, false);
        obj.transform.localScale = new Vector3(1f, 1f, 1f);
        obj.SetActive(true);

        Vector3 objPosition = obj.transform.position;
        objPosition.z = 0f;

        obj.transform.position = objPosition;

        Transform icon = obj.transform.Find("Icon");
        Transform label = obj.transform.Find("Label");
        Transform stackBox = obj.transform.Find("Stack Box");
        Transform number = stackBox.transform.Find("Number");

        Image iconImage = icon.GetComponent<Image>();
        iconImage.sprite = item.Data.icon;

        TextMeshProUGUI labelText = label.GetComponent<TextMeshProUGUI>();
        labelText.text = item.Data.displayName;

        Image stackBoxImage = stackBox.GetComponent<Image>();
        stackBoxImage.sprite = StackBoxBackground;

        TextMeshProUGUI numberText = number.GetComponent<TextMeshProUGUI>();
        numberText.text = item.StackSize.ToString();

        obj.transform.SetParent(inventoryDock.transform, true);
        drawnInventory.Add(item);
    }

    public void RemoveInventorySlot(InventoryItem item)
    {
        Debug.Log(item.Data.name);
        Transform itemTransform = inventoryDock.transform.Find(item.Data.name);
        if (itemTransform != null)
        {
            Destroy(itemTransform.gameObject);
        } 
    }

    public InventoryItem Get(InventoryItemData referenceData)
    {
        if (m_itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            return value;
        }else
        {
            return null;
        }
    }

    public void Add(InventoryItemData referenceData)
    {
        if (m_itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        { 
            value.AddToStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(referenceData);
            inventory.Add(newItem);
            m_itemDictionary.Add(referenceData, newItem);
        }
        onInventoryChangedEvent.Invoke();
    }

    public void Remove(InventoryItemData referenceData)
    {
        if (m_itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            Debug.Log(value.Data.name);
            value.RemoveFromStack();
            Debug.Log(value.StackSize);

            if (value.StackSize== 0)
            {
                inventory.Remove(value);
                Debug.Log(value.Data.name);
                Debug.Log(inventory.Count);
                m_itemDictionary.Remove(referenceData);
            }
        }
        onInventoryChangedEvent.Invoke();
    }

    [Serializable]
    public class InventoryItem
    {
        public InventoryItemData Data { get; private set; }
        public int StackSize { get; private set; }

        public InventoryItem(InventoryItemData source)
        {
            Data = source;
            AddToStack();
        }

        public void AddToStack()
        {
            StackSize++;
        }

        public void RemoveFromStack()
        {
            StackSize--;
        }
    }
        
    private Image m_icon;
    private TextMeshProUGUI m_label;
    private GameObject m_stackObj;
    private TextMeshProUGUI m_stackLabel;

    public void Set(InventoryItem item)
    {
        m_icon.sprite = item.Data.icon;
        m_label.text = item.Data.displayName;
        if (item.StackSize <= 1)
        {
            m_stackObj.SetActive(false);
            return;
        }

        m_stackLabel.text = item.StackSize.ToString();
    }

}