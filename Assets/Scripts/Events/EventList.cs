using System;
using System.Collections.Generic;
using UnityEngine;

namespace Morbius.Scripts.Events
{
    //Unity requires a setup like this in order to show everything in inspector
    [Serializable]
    public class EventList
    {
        [SerializeField]
        private List<DefaultEvent> m_events;

        public List<DefaultEvent> Events { get => m_events; }
    }
}
