using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Morbius.Scripts.Items;
using Morbius.Scripts.Messages;

namespace Morbius.Scripts.UI
{
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(UIFader))]
    public class InventoryUI : MonoBehaviour, IButtonEventTarget, IInventoryMessage
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
        }

        private void Start()
        {
            Button button = GetComponent<Button>();
            button.onClick.AddListener(() => Inventory.DropHandItem());

            MessageSystem.Register<IInventoryMessage>(gameObject);
            Inventory.RegisterUI(gameObject);
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
