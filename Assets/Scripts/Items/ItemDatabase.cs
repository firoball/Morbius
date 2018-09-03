using UnityEngine;
using System.Collections.Generic;
using Morbius.Scripts.Items;

public class ItemDatabase : MonoBehaviour
{
    private static ItemDatabase s_singleton;

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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("ItemList: Multiple instances detected. Destroying...");
            Destroy(this);
        }

    }

    private void Start()
    {
        foreach(Item item in m_items)
        {
            ItemManager.RegisterItem(item);
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
}
