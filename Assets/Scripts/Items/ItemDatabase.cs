using UnityEngine;
using System;
using System.Collections.Generic;
using Morbius.Scripts.Items;

public class ItemDatabase : MonoBehaviour
{
    private static ItemDatabase s_singleton;
    private static Dictionary<Item, ItemSaveState> s_itemStates = new Dictionary<Item, ItemSaveState>();

    [SerializeField]
    private List<Item> m_items = new List<Item>();

    public List<Item> Items
    {
        get
        {
            return m_items;
        }

        set
        {
            m_items = value;
        }
    }

    void Awake()
    {
        if (s_singleton == null)
        {
            s_singleton = this;
            Initialize();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("ItemDatabase: Multiple instances detected. Destroying...");
            Destroy(this);
        }

    }

    public static Item GetItemById(int id)
    {
        if (s_singleton)
        {
            return s_singleton.m_items.Find(x => x.Id == id);
        }
        else
        {
            return null;
        }
    }

    private static void RegisterItem(Item item)
    {
        ItemSaveState state = new ItemSaveState();
        if (!s_itemStates.ContainsKey(item))
        {
            s_itemStates.Add(item, state);
        }
    }

    public static ItemSaveState GetItemStatus(Item item)
    {
        ItemSaveState saveState;
        if (s_itemStates.TryGetValue(item, out saveState))
        {
            return saveState;
        }
        else
        {
            return null;
        }
    }

    public static void SetItemStatus(Item item, ItemSaveState saveState)
    {
        if (s_itemStates.ContainsKey(item))
        {
            s_itemStates[item] = saveState;
        }
    }

    public void Initialize()
    {
        s_itemStates.Clear();
        foreach (Item item in m_items)
        {
            RegisterItem(item);
        }
    }

    public static int GetEncodedEventId(Item item)
    {
        int status = 0;

        if (s_itemStates.ContainsKey(item))
        {
            status = 100000 + (s_itemStates[item].SequenceIndex * 10000) + item.Id;
        }

        return status;
    }

    public static ItemSaveState DecodeEventId(int status, out Item item)
    {
        ItemSaveState saveState;
        int identifier = status / 100000;
        if (identifier == 1)
        {
            int sequenceIndex = (status - (identifier * 100000)) / 10000;
            int itemId = status - (identifier * 100000) - (sequenceIndex * 10000);
            item = GetItemById(itemId);
            saveState = GetItemStatus(item);
        }
        else
        {
            item = null;
            saveState = new ItemSaveState();
        }

        return saveState;
    }

}
