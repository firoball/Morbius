using UnityEngine;
using System.Collections.Generic;
using Morbius.Scripts.Items;

public class ItemList : MonoBehaviour
{
    private static ItemList s_singleton;

    [SerializeField]
    private List<Item> m_itemDatabase = new List<Item>();

    public List<Item> ItemDatabase
    {
        get
        {
            return m_itemDatabase;
        }

        set
        {
            m_itemDatabase = value;
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
        foreach(Item item in m_itemDatabase)
        {
            ItemManager.RegisterItem(item);
        }
    }
}
