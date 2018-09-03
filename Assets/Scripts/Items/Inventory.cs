using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using Morbius.Scripts.UI;
using Morbius.Scripts.Cursor;

namespace Morbius.Scripts.Items
{
    public class Inventory : MonoBehaviour
    {
        private static Inventory s_singleton;
        private static List<Item> s_items;
        private static Item s_itemInHand;

        [SerializeField]
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
            }
            else
            {
                Debug.Log("Inventory: Multiple instances detected. Destroying...");
                Destroy(gameObject);
            }
        }

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
            ItemSaveState state = ItemManager.GetItemStatus(item);
            if (state.Removed || state.MorphItem)
            {
                destroy = true;
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
            //TODO inform Cursor
            CursorManager.UpdateCursorIcon(item.Icon);
            //when item is taken, inventory object is destroyed, hover exit event does not fire anymore. trigger manually
            CursorManager.UpdateCursorText(null);
            Remove(item);
        }

        private void FromHand()
        {
            if (s_itemInHand != null)
            {
                Add(s_itemInHand);
                s_itemInHand = null;
                //TODO inform Cursor
                CursorManager.UpdateCursorIcon(null);
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
                    ItemManager.Combine(s_itemInHand, item);
                }
                else
                {
                    s_singleton.ToHand(item);
                }
            }
        }

        //todo make event?
        public static void OnHoverEnter(Sprite sprite)
        {
            if (sprite && s_singleton)
            {
                Item item = s_items.Find(x => x.Icon == sprite);
                CursorManager.UpdateCursorItem(item.Label, true, true);
            }
        }

        //todo make event?
        public static void OnHoverExit(Sprite sprite)
        {
            if (sprite && s_singleton)
            {
                Item item = s_items.Find(x => x.Icon == sprite);
                CursorManager.SetDefaultCursor();
            }
        }
    }
}
