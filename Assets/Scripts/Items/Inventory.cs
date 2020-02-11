using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Morbius.Scripts.Events;
using Morbius.Scripts.Messages;

namespace Morbius.Scripts.Items
{
    public class Inventory : MonoBehaviour
    {
        private static Inventory s_singleton;
        private static List<Item> s_items;
        private static Item s_itemInHand;

        private bool m_initialized;

        public static Item ItemInHand
        {
            get
            {
                return s_itemInHand;
            }
        }

        private void Awake()
        {
            if (s_singleton == null)
            {
                s_singleton = this;
                s_items = new List<Item>();
                m_initialized = false;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Debug.Log("Inventory: Multiple instances detected. Destroying...");
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (!m_initialized)
            {
                UpdateUI();
                m_initialized = true;
            }

            for (int i = s_items.Count - 1; i >= 0; i--)
            {
                UpdateStatus(s_items[i]);
            }
        }

        private void UpdateStatus(Item item)
        {
            ItemSaveState state = ItemDatabase.GetItemStatus(item);
            if (state.MorphItem != null)
            {
                Remove(item);
                Add(state.MorphItem);
            }
            else if (state.Destroyed)
            {
                Remove(item);
            }
            else if (state.Collected)
            {
                Add(item);
            }
            else
            {

            }
        }

        private void ToHand(Item item)
        {
            if (s_itemInHand != null)
            {
                Add(s_itemInHand);
            }
            s_itemInHand = item;
            Remove(item);
        }

        private void FromHand()
        {
            if (s_itemInHand != null)
            {
                Add(s_itemInHand);
                s_itemInHand = null;
            }

        }

        private void Add(Item item)
        {
            if (!s_items.Contains(item) && item && item.Icon)
            {
                s_items.Add(item);
                MessageSystem.Execute<IInventoryMessage>((x, y) => x.OnAdd(item.Icon));
            }
        }

        private void Remove(Item item)
        {
            if (s_items.Remove(item) && item && item.Icon)
            {
                MessageSystem.Execute<IInventoryMessage>((x, y) => x.OnRemove(item.Icon));
            }

        }

        private void UpdateUI()
        {
            foreach (Item item in s_items)
            {
                MessageSystem.Execute<IInventoryMessage>((x, y) => x.OnAdd(item.Icon));
            }
        }

        public static void Collect(Item item)
        {
            if (s_singleton)
            {
                s_singleton.Add(item);
            }
        }

        public static void DropHandItem()
        {
            if (s_singleton)
            {
                s_singleton.FromHand();
            }
        }

        //TODO use event system? this should not be exposed to everyone but just to UI
        public static void Interact(Sprite sprite)
        {
            if (sprite && s_singleton)
            {
                Item item = s_items.Find(x => x.Icon == sprite);
                if (s_itemInHand)
                {
                    EventManager.RaiseEvent(item.Id);
                }
                else
                {
                    s_singleton.ToHand(item);
                }
            }
        }

        public static void Setup()
        {
            if (s_singleton)
            {
                s_singleton.m_initialized = false;
            }
        }

        public static void Initialize()
        {
            if (s_singleton)
            {
                s_items.Clear();
                s_itemInHand = null;
            }
        }

        public static void Save(int slot)
        {
            DropHandItem();
            if (s_items != null)
            {
                PlayerPrefs.SetInt("Morbius.IN." + slot + ".C", s_items.Count);
                for (int i = 0; i < s_items.Count; i++)
                {
                    PlayerPrefs.SetInt("Morbius.IN." + slot + "." + i, s_items[i].Id);
                }
            }
        }

        public static void Load(int slot)
        {
            Initialize();
            if (s_items != null)
            {
                int count = PlayerPrefs.GetInt("Morbius.IN." + slot + ".C", 0);
                for (int i = 0; i < count; i++)
                {
                    int id = PlayerPrefs.GetInt("Morbius.IN." + slot + "." + i, 0);
                    Item item = ItemDatabase.GetItemById(id);
                    s_items.Add(item);
                }
            }
        }

        public static void Delete(int slot)
        {
            int count = PlayerPrefs.GetInt("Morbius.IN." + slot + ".C", 0);
            PlayerPrefs.DeleteKey("Morbius.IN." + slot + ".C");
            for (int i = 0; i < count; i++)
            {
                PlayerPrefs.DeleteKey("Morbius.IN." + slot + "." + i);
            }
        }
    }
}
