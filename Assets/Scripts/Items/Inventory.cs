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

        //TODO: add morphed items back to inventory!
        private void Update()
        {
            if (!m_initialized)
            {
                UpdateUI();
                m_initialized = true;
            }

            for (int i = s_items.Count - 1; i >= 0; i--)
            {
                Item item = s_items[i];
                bool destroy;
                destroy = UpdateStatus(item);
                if (destroy)
                {
                    Remove(item);
                }
            }
        }

        private bool UpdateStatus(Item item)
        {
            bool destroy = false;
            ItemSaveState state = ItemDatabase.GetItemStatus(item);
            if (state.Destroyed || state.MorphItem)
            {
                destroy = true;
                //TODO: move to proper place?
                if (state.MorphItem != null)
                {
                    Add(state.MorphItem);
                }
            }
            return destroy;
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
            if (!s_items.Contains(item) && item.Icon)
            {
                s_items.Add(item);
                MessageSystem.Execute<IInventoryMessage>((x, y) => x.OnAdd(item.Icon));
            }
        }

        private void Remove(Item item)
        {
            if (s_items.Remove(item) && item.Icon)
            {
                MessageSystem.Execute<IInventoryMessage>((x, y) => x.OnRemove(item.Icon));
            }

        }

        private void UpdateUI()
        {
            foreach(Item item in s_items)
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

    }
}
