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
            //Debug.Log("collect event " + eventId);
            Item item = ItemDatabase.GetItemById(eventId);
            ItemSaveState state = ItemDatabase.GetItemStatus(item);
            if (state != null && item.IsReadyForCollection(state.SequenceIndex) && Inventory.ItemInHand == null)
            {
                Inventory.Collect(item);

                state.Collected = true;
                m_audio.Play();
            }
            yield return new WaitForEndOfFrame();
        }
    }

}
