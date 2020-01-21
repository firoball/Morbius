using UnityEngine;
using Morbius.Scripts.Messages;

namespace Morbius.Scripts.Items
{
    public class ConditionalItemInstance : ItemInstance
    {
        [SerializeField]
        private Item m_badItem;
        [SerializeField]
        private Item m_goodItem;
        [SerializeField]
        private Item m_requiredItem;

        private ItemSaveState m_requiredStatus;
        private bool m_lastSequence;

        protected override void Start()
        {
            if (!m_badItem || !m_goodItem)
            {
                Debug.LogWarning("ConditionalItemInstance: <" + name + "> has no item(s) assigned!");
                return;
            }
            m_item = m_badItem;

            m_requiredStatus = ItemDatabase.GetItemStatus(m_requiredItem);
            m_lastSequence = false;

            base.Start();
        }

        protected override void Update()
        {
            if (m_requiredStatus.Collected)
            {
                if (m_item != m_goodItem)
                {
                    m_item = m_goodItem;
                    //update status and last sequence detector for good item
                    m_status = ItemDatabase.GetItemStatus(m_item);
                    m_lastSequence = false;
                    MessageSystem.Execute<IUnlockTriggerMessage>((x, y) => x.OnLock());
                }
            }
            bool sequence = m_item.IsLastSequence(m_status.SequenceIndex);
            if (sequence && !m_lastSequence) 
            {
                MessageSystem.Execute<IUnlockTriggerMessage>((x, y) => x.OnUnlock());
            }
            m_lastSequence = sequence;

            base.Update();
        }


    }
}
