using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Morbius.Scripts.Ambient;
using Morbius.Scripts.Items;
using Morbius.Scripts.UI;

namespace Morbius.Scripts.Events
{
    [Serializable]
    public class ItemSequenceEvent : DefaultEvent
    {
        [SerializeField]
        private GameObject m_receiver;

        private float m_displayTime;

        private const float c_minDisplayTime = 1.5f;

        private void Update()
        {
            Timer();
        }

        private void Timer()
        {
            //timer is handled in Update instead of Coroutines in order to avoid triggering multiple coroutines
            if (m_displayTime > 0.0f)
            {
                m_displayTime = Mathf.Max(m_displayTime - Time.deltaTime, 0.0f);
                if (m_displayTime <= 0.0f)
                {
                    ExecuteEvents.Execute<IInfoTextEventTarget>(m_receiver, null, (x, y) => x.OnHide());
                }
            }
        }

        public override IEnumerator Execute(int eventId)
        {
            Item item = ItemDatabase.GetItemById(eventId);
            ItemSaveState state = ItemDatabase.GetItemStatus(item);
            ItemSequence sequence = item.GetSequence(state.SequenceIndex);

            m_displayTime = Mathf.Max(sequence.Audio.length + 0.5f, c_minDisplayTime);
            ExecuteEvents.Execute<IInfoTextEventTarget>(m_receiver, null, (x, y) => x.OnShow(sequence.Description));
            AudioManager.ScheduleVoice(sequence.Audio);
            yield return new WaitForSeconds(m_displayTime);
        }
    }

}
