using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Morbius.Scripts.Items;

namespace Morbius.Scripts.Events
{
    [RequireComponent(typeof(AudioSource))]
    [Serializable]
    public class ItemCollectEvent : DefaultEvent
    {
        private AudioSource m_audio;

        private void Awake()
        {
            m_audio = GetComponent<AudioSource>();
        }

        public override IEnumerator Execute(int eventId)
        {
            Item item = ItemDatabase.GetItemById(eventId);
            Inventory.Collect(item);
            m_audio.Play();
            //TODO trigger inventory UI event!?
            yield return new WaitForSeconds(0.01f);
        }
    }

}
