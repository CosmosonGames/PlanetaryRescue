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

    [Header("Misc")]
    public Sprite StackBoxBackground;

    private List<InventoryItem> drawnInventory;

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
    }

    private void OnUpdateInventory()
    {
        DrawInventory();
    }

    public void DrawInventory()
    {
        foreach (InventoryItem item in current.inventory)
        {
            if (!drawnInventory.Contains(item)) {
                AddInventorySlot(item);
            }
        }
    }

    public void AddInventorySlot(InventoryItem item)
    {
        GameObject obj = Instantiate(m_slotPrefab);
        obj.transform.SetParent(transform, false);
        obj.transform.localScale = new Vector3(1f, 1f, 1f);

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

    public InventoryItem Get(InventoryItemData referenceData)
    {
        if (m_itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            int number = value.StackSize;
            Debug.Log(number);
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
            value.RemoveFromStack();

            if (value.StackSize== 0)
            {
                inventory.Remove(value);
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