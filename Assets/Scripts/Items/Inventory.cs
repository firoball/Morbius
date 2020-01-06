using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using Morbius.Scripts.Events;
using Morbius.Scripts.UI;

namespace Morbius.Scripts.Items
{
    public class Inventory : MonoBehaviour
    {
        private static Inventory s_singleton;
        private static List<Item> s_items;
        private static Item s_itemInHand;

        //[SerializeField]
        private GameObject m_inventoryUI;

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
                ExecuteEvents.Execute<IInventoryEventTarget>(m_inventoryUI, null, (x, y) => x.OnAdd(item.Icon));
            }
        }

        private void Remove(Item item)
        {
            if (s_items.Remove(item) && item.Icon)
            {
                ExecuteEvents.Execute<IInventoryEventTarget>(m_inventoryUI, null, (x, y) => x.OnRemove(item.Icon));
            }

        }

        private void UpdateUI(GameObject ui)
        {
            s_singleton.m_inventoryUI = ui;
            foreach(Item item in s_items)
            {
                ExecuteEvents.Execute<IInventoryEventTarget>(m_inventoryUI, null, (x, y) => x.OnAdd(item.Icon));
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

        public static void Clear()
        {
            s_items.Clear();
            s_itemInHand = null;
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

        public static void RegisterUI(GameObject ui)
        {
            if (s_singleton)
            {
                s_singleton.UpdateUI(ui);
            }
        }

        public static Item Find(Sprite sprite)
        {
            if (sprite == null)
                return null;
            else
                return s_items.Find(x => x.Icon == sprite);
        }

    }
}
