using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Morbius.Scripts.Events
{
    public class EventManager : MonoBehaviour
    {
        [SerializeField]
        private List<EventList> m_eventGroups;

        private static EventManager s_singleton;

        void Awake()
        {
            if (s_singleton == null)
            {
                s_singleton = this;
                if (m_eventGroups == null)
                {
                    m_eventGroups = new List<EventList>();
                }
            }
            else
            {
                Debug.Log("EventManager: Multiple instances detected. Destroying...");
                Destroy(this);
            }
        }

        public static void RaiseEvent(int eventId)
        {
            if (s_singleton)
            {
                s_singleton.TriggerEvent(eventId, 0);
            }
        }

        public static void RaiseEvent(int eventId, int eventGroupId)
        {
            if (s_singleton)
            {
                s_singleton.TriggerEvent(eventId, eventGroupId);
            }
        }

        private void TriggerEvent(int eventId, int eventGroupId)
        {
            if (eventGroupId >= 0 && eventGroupId < m_eventGroups.Count)
            {
                List<DefaultEvent> sourceEvents = m_eventGroups[eventGroupId].Events;
                //event trigger has to be non-blocking otherwise ItemInHand could cause weird behavior
                List<DefaultEvent> selectedEvents = sourceEvents.Where(x =>
                ((x != null) && (eventId >= x.MinId) && (eventId <= x.MaxId))).ToList();
                foreach (DefaultEvent ev in selectedEvents)
                {
                    if (ev.AllowExecution())
                    {
                        StartCoroutine(ev.Execute(eventId));
                    }
                }
            }
        }

    }
}
