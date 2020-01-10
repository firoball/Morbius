using UnityEngine;
using UnityEngine.UI;
using Morbius.Scripts.Items;

namespace Morbius.Scripts.Cursor
{
    [RequireComponent(typeof(Image))]
    public class CursorInventoryItem : MonoBehaviour, ICursorObject
    {
        private Item m_item;

        private void Start()
        {
            Image image = GetComponent<Image>();
            m_item = ItemDatabase.GetItemBySprite(image.sprite);
        }

        public CursorInfo GetCursorInfo()
        {
            if (!m_item)
            {
                return new CursorInfo();
            }
            else
            {
                CursorInfo info = new CursorInfo()
                {
                    Label = m_item.Label,
                    Icon = null,
                    IsGrabable = false,
                    IsDialog = false,
                    IsInteractable = false,
                    IsPortal = false
                };

                return info;
            }
        }
    }
}
