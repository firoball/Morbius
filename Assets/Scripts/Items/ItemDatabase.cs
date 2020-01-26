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

    private void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            foreach (KeyValuePair<Item, ItemSaveState> kvp in s_itemStates)
            {
                Debug.Log(kvp.Key.Id + ":" + kvp.Key.name + " D:" + kvp.Value.Destroyed + " C:" + kvp.Value.Collected + " M:" + kvp.Value.MorphItem);
            }
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

    public static Item GetItemBySprite(Sprite sprite)
    {
        if (s_singleton)
        {
            return s_singleton.m_items.Find(x => x.Icon == sprite);
        }
        else
        {
            return null;
        }
    }

    private static void RegisterItem(Item item)
    {
        if (!s_itemStates.ContainsKey(item))
        {
            ItemSaveState state = new ItemSaveState();
            s_itemStates.Add(item, state);
        }
    }

    public static ItemSaveState GetItemStatus(Item item)
    {
        if (item == null) return null;

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

    public static void Initialize()
    {
        s_itemStates.Clear();
        if (s_singleton)
        {
            foreach (Item item in s_singleton.m_items)
            {
                RegisterItem(item);
            }
        }
    }

}
