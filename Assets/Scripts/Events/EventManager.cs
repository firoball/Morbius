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
                //DontDestroyOnLoad(gameObject); -- subscribers may be specific scene instances, create configured prefab instead
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
            if (s_singleton)
            {
                s_singleton.TriggerEvent(eventId);
            }
        }

        private void TriggerEvent(int eventId)
        {
            List<DefaultEvent> selectedEvents = m_events.Where(x =>
            ((x != null) && (eventId >= x.MinId) && (eventId <= x.MaxId))).ToList();
            //StartCoroutine(Execute(selectedEvents, eventId));
            //event trigger has to be non-blocking otherwise ItemInHand could cause weird behavior
            foreach (DefaultEvent ev in selectedEvents)
            {
                StartCoroutine(ev.Execute(eventId));
            }
        }

        /*private IEnumerator Execute(List<DefaultEvent> events, int eventId)
        {
            //TODO: trigger Lock Event
            foreach (DefaultEvent ev in events)
            {
                yield return StartCoroutine(ev.Execute(eventId));
            }
            //TODO: trigger Unlock Event
        }*/
    }
}
