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
        private List<DefaultEvent> m_events;
        [SerializeField]
        private List<DefaultEvent> m_customEvents;


        private static EventManager s_singleton;

        void Awake()
        {
            if (s_singleton == null)
            {
                s_singleton = this;
                //DontDestroyOnLoad(gameObject); -- subscribers may be specific scene instances, create configured prefab instead
                if (m_events == null)
                {
                    m_events = new List<DefaultEvent>();
                }
                if (m_customEvents == null)
                {
                    m_customEvents = new List<DefaultEvent>();
                }
            }
            else
            {
                Debug.Log("EventManager: Multiple instances detected. Destroying...");
                Destroy(this);
            }
        }

        public static void RegisterEvent(DefaultEvent ev)
        {
            if (s_singleton)
            {
                if (!s_singleton.m_customEvents.Contains(ev))
                {
                    s_singleton.m_customEvents.Add(ev);
                }
            }
        }

        public static void RaiseEvent(int eventId)
        {
            if (s_singleton)
            {
                s_singleton.TriggerEvent(eventId, false);
            }
        }

        public static void RaiseCustomEvent(int eventId)
        {
            if (s_singleton)
            {
                s_singleton.TriggerEvent(eventId, true);
            }
        }

        private void TriggerEvent(int eventId, bool custom)
        {
            List<DefaultEvent> selectedEvents;
            //event trigger has to be non-blocking otherwise ItemInHand could cause weird behavior
            if (!custom)
            {
                selectedEvents = m_events.Where(x =>
                ((x != null) && (eventId >= x.MinId) && (eventId <= x.MaxId))).ToList();
                foreach (DefaultEvent ev in selectedEvents)
                {
                    if (ev.AllowExecution())
                    {
                        StartCoroutine(ev.Execute(eventId));
                    }
                }
            }

            selectedEvents = m_customEvents.Where(x =>
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
