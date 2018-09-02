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
        private List<GameObject> m_subscribers;

        [SerializeField]
        private List<DefaultEvent> m_events;

        private static EventManager s_singleton;

        void Awake()
        {
            if (s_singleton == null)
            {
                s_singleton = this;
                //DontDestroyOnLoad(gameObject); -- subscribers may be specific scene instances
                if (m_subscribers == null)
                {
                    m_subscribers = new List<GameObject>();
                }
                if (m_events == null)
                {
                    m_events = new List<DefaultEvent>();
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
                if (!s_singleton.m_events.Contains(ev))
                {
                    s_singleton.m_events.Add(ev);
                }
            }
        }

        public static void RaiseEvent(int eventId)
        {
            Debug.Log("triggered event: " + eventId);
            if (s_singleton)
            {
                List<DefaultEvent> selectedEvents = s_singleton.m_events.Where(x =>
                ((x != null) && (eventId >= x.MinId) && (eventId <= x.MaxId))).ToList();
                s_singleton.StartCoroutine(s_singleton.Execute(selectedEvents, eventId));
            }
        }

        private IEnumerator Execute(List<DefaultEvent> events, int eventId)
        {
            //TODO: trigger Lock Event
            foreach (DefaultEvent ev in events)
            {
                //should be started as coroutine here or in event itself
                yield return s_singleton.StartCoroutine(ev.Execute(eventId));
            }
            //TODO: trigger Unlock Event
        }
    }
}
