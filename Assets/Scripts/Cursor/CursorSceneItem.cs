using UnityEngine;
using Morbius.Scripts.Items;

namespace Morbius.Scripts.Cursor
{
    [RequireComponent(typeof(ItemInstance))]
    public class CursorSceneItem : MonoBehaviour, ICursorObject
    {
        private ItemInstance m_instance;

        //executing too early, will cause outdated cursor info after item morph
        /*private void Start()
        {
            m_instance = GetComponent<ItemInstance>();
        }*/

        public CursorInfo GetCursorInfo()
        {
            //moved here from Start()
            if (!m_instance)
                m_instance = GetComponent<ItemInstance>();

            Item item = m_instance.Item;
            if (!item)
            {
                return new CursorInfo();
            }
            else
            {
                ItemSaveState saveState = ItemDatabase.GetItemStatus(item);
                CursorInfo info = new CursorInfo()
                {
                    Label = item.Label,
                    Icon = null,
                    IsGrabable = item.IsReadyForCollection(saveState.SequenceIndex),
                    IsDialog = false,
                    IsInteractable = true,
                    IsPortal = false
                };

                return info;
            }
        }
    }
}
