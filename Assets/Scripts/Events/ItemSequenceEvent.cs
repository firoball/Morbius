using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Morbius.Scripts.Ambient;
using Morbius.Scripts.Items;
using Morbius.Scripts.Messages;

namespace Morbius.Scripts.Events
{
    [Serializable]
    public class ItemSequenceEvent : DefaultEvent
    {
        private float m_displayTime;

        private const float c_minDisplayTime = 1.5f;

        public override IEnumerator Execute(int eventId)
        {
            Debug.Log("sequence event " + eventId);
            Item item = ItemDatabase.GetItemById(eventId);
            ItemSaveState state = ItemDatabase.GetItemStatus(item);
            if (state != null && item.Sequences != null && Inventory.ItemInHand == null)
            {
                ItemSequence sequence = item.GetSequence(state.SequenceIndex);
                if (sequence != null)
                {
                    m_displayTime = Mathf.Max(sequence.Audio.length + 0.5f, c_minDisplayTime);
                    MessageSystem.Execute<IInfoTextMessage>((x, y) => x.OnShow(sequence.Description, m_displayTime));
                    AudioManager.ScheduleVoice(sequence.Audio);
                    //sequence may trigger additional events
                    EventManager.RaiseEvent(sequence.TriggerId);

                    if (!item.IsLastSequence(state.SequenceIndex))
                        state.SequenceIndex++;

                    ItemDatabase.SetItemStatus(item, state);
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

}
