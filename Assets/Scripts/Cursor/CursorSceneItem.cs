using UnityEngine;
using Morbius.Scripts.Items;

namespace Morbius.Scripts.Cursor
{
    [RequireComponent(typeof(ItemInstance))]
    public class CursorSceneItem : MonoBehaviour, ICursorObject
    {
        private ItemInstance m_instance;

        private void Awake()
        {
            m_instance = GetComponent<ItemInstance>();
        }

        public CursorInfo GetCursorInfo()
        {
            Item item = m_instance.Item;
            if (!item)
            {
                return new CursorInfo();
            }
            else
            {
                ItemSaveState saveState = ItemManager.GetItemStatus(item);
                CursorInfo info = new CursorInfo()
                {
                    Label = item.Label,
                    Icon = null,
                    IsGrabable = item.IsReadyForCollection(saveState.SequenceIndex),
                    IsInteractable = true,
                    IsPortal = false
                };

                return info;
            }
        }
    }
}
