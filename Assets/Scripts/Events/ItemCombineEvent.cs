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
            Inventory.DropHandItem();

            foreach (Item item in items)
            {
                ItemSaveState state = ItemDatabase.GetItemStatus(item);
                if (state != null)
                {
                    //will this item be destroyed?
                    if (item.Destroyable)
                        state.Destroyed = true;

                    ItemDatabase.SetItemStatus(item, state);
                }
            }


            //when no morph id was given, trigger event
            //if (combination.MorphId == 0)
            //{
            //EventManager.RaiseEvent(combination.TriggerId);
            //}
            //otherwise morph item with morphId to triggerId
            //this is not nice, but that's how it was done in the original game
            //else
            if (combination.MorphId > 0)
            {
                Item item = ItemDatabase.GetItemById(combination.MorphId);
                ItemSaveState state = ItemDatabase.GetItemStatus(item);
                if (state != null)
                {
                    state.MorphItem = ItemDatabase.GetItemById(combination.TriggerId);
                    ItemDatabase.SetItemStatus(item, state);
                }
            }
            //only custom events may be triggered as combination result
            EventManager.RaiseCustomEvent(combination.TriggerId);
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
