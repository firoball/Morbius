using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Morbius.Scripts.Items;
using Morbius.Scripts.Cursor;

namespace Morbius.Scripts.UI
{
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(UIFader))]
    public class InventoryUI : MonoBehaviour, IButtonEventTarget, /*IHoverEventTarget, */IInventoryEventTarget
    {
        [SerializeField]
        private GameObject m_cursor;
        [SerializeField]
        private GameObject m_inventory;
        [SerializeField]
        private GameObject m_itemPrefab;

        private List<Image> m_items;
        private UIFader m_fader;
        private GameObject m_receiver;

        private void Awake()
        {
            m_items = new List<Image>();
            m_fader = GetComponent<UIFader>();
            Button button = GetComponent<Button>();
            button.onClick.AddListener(() => Inventory.DropHandItem());
        }

        public void OnButtonNotification(GameObject sender)
        {
            Image image = sender.GetComponent<Image>();
            if (image)
            {
                //TODO use event?
                Inventory.Interact(image.sprite);
            }
        }

       /* public void OnHoverBeginNotification(GameObject sender)
        {
            Image image = sender.GetComponent<Image>();
            if (image && m_cursor)
            {
                //get name from itemmanager
                //should not be done here but in manager object
                //ExecuteEvents.Execute<IAnimatedCursorEventTarget>(m_cursor, null, (x, y) => x.OnSetText(image.name));
                Inventory.OnHoverEnter(image.sprite);
            }
        }

        public void OnHoverEndNotification(GameObject sender)
        {
            Image image = sender.GetComponent<Image>();
            if (image && m_cursor)
            {
                //should not be done here but in manager object
                //ExecuteEvents.Execute<IAnimatedCursorEventTarget>(m_cursor, null, (x, y) => x.OnSetText(null));
                Inventory.OnHoverExit(image.sprite);
            }
        }*/

        public void OnAdd(Sprite item)
        {
            if (item && !m_items.Find(x => x.sprite == item))
            {
                GameObject objItem = Instantiate(m_itemPrefab, transform);
                Image image = objItem.GetComponent<Image>();
                if (image)
                {
                    image.sprite = item;
                    objItem.name = item.name;
                    m_items.Add(image);
                }
                else
                {
                    Debug.LogWarning("OnAdd: Invalid Item Prefab");
                }

            }
        }

        public void OnRemove(Sprite item)
        {
            Image result = m_items.Find(x => x.sprite == item);
            if (result)
            {
                Destroy(result.gameObject);
                m_items.Remove(result);
            }
        }

        public void OnRegisterReceiver(GameObject receiver)
        {
            m_receiver = receiver;
        }

        public void OnShow()
        {
            m_fader.Show(false);
        }

        public void OnHide()
        {
            m_fader.Hide(false);
        }

        public void Clear()
        {
            foreach (Image image in m_items)
            {
                Destroy(image.gameObject);
            }
            m_items.Clear();
        }

    }
}
