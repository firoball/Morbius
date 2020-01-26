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
    [RequireComponent(typeof(AudioSource))]
    [Serializable]
    public class ItemCombineEvent : DefaultEvent
    {
        private float m_displayTime;
        private AudioSource m_audio;

        private const float c_minDisplayTime = 1.5f;
        private const int c_eventCollectGroup = 1;
        private const int c_eventCustomGroup = 2;

        private void Awake()
        {
            m_audio = GetComponent<AudioSource>();
        }

        private void ShowFailure()
        {
            Failure failure = CombinationDatabase.GetFailure();

            if (failure.Audio != null)
            {
                m_displayTime = Mathf.Max(failure.Audio.length + 0.5f, c_minDisplayTime);
                MessageSystem.Execute<IInfoTextMessage>((x, y) => x.OnShow(failure.Description, m_displayTime));
                AudioManager.ScheduleVoice(failure.Audio);
            }
        }

        private void Combine(Combination combination, IEnumerable<Item> items)
        {
            foreach (Item item in items)
            {
                ItemSaveState state = ItemDatabase.GetItemStatus(item);
                if (state != null)
                {
                    //will this item be destroyed?
                    if (item.Destroyable)
                        state.Destroyed = true;
                }
            }

            /* depending on when morph id is set trigger id will be interpreted differently
             * 1) resultId is used for morph process
             * 2) resultId defines which item is created fand added to inventory
             * For a generic framework this should be cleanup as it complicates event system for no reason
             */
            if (combination.MorphId > 0)
            {
                //morph it
                Item item = ItemDatabase.GetItemById(combination.MorphId);
                Item morphItem = ItemDatabase.GetItemById(combination.TriggerId);
                item.Morph(morphItem);
                /*ItemSaveState state = ItemDatabase.GetItemStatus(item);
                if (state != null)
                {
                    state.MorphItem = ItemDatabase.GetItemById(combination.TriggerId);
                    //if old item was collected, also morphed one will be...
                    if (state.MorphItem != null)
                    {
                        ItemSaveState morphItemstate = ItemDatabase.GetItemStatus(state.MorphItem);
                        morphItemstate.Collected = state.Collected;
                    }
                }*/
            }
            else
            {
                //collect it
                EventManager.RaiseEvent(combination.TriggerId, c_eventCollectGroup);
            }
            //only custom events may be triggered as combination result
            EventManager.RaiseEvent(combination.TriggerId, c_eventCustomGroup);
            m_audio.Play();

            if (combination.Audio != null)
            {
                m_displayTime = Mathf.Max(combination.Audio.length + 0.5f, c_minDisplayTime);
                MessageSystem.Execute<IInfoTextMessage>((x, y) => x.OnShow(combination.Description, m_displayTime));
                AudioManager.ScheduleVoice(combination.Audio);
            }
        }

        public override IEnumerator Execute(int eventId)
        {
            Debug.Log("combine event " + eventId);
            if (Inventory.ItemInHand != null)
            {
                Item[] items = new Item[2];
                items[0] = ItemDatabase.GetItemById(eventId);
                items[1] = Inventory.ItemInHand;
                Combination combination = CombinationDatabase.GetCombination(items[0].Id, items[1].Id);

                Inventory.DropHandItem();

                if (combination != null)
                {
                    Combine(combination, items);
                }
                else
                {
                    ShowFailure();
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

}
