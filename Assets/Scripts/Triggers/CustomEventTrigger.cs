using System.Collections;
using UnityEngine;
using Morbius.Scripts.Events;

namespace Morbius.Scripts.Triggers
{
    public class CustomEventTrigger: BaseTrigger
    {
        [SerializeField]
        private DefaultEvent m_event;
        [SerializeField]
        private int m_eventId;


        protected override void Entered()
        {
            Execute();
        }

        protected override void Clicked()
        {
            Execute();
        }

        protected override void AutoPlay()
        {
            Execute();
        }

        private void Execute()
        {
            if (m_event)
            {
                StartCoroutine(m_event.Execute(m_eventId));
            }
        }

    }
}
