using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Morbius.Scripts.Dialog;
using Morbius.Scripts.Items;
using Morbius.Scripts.Level;
using Morbius.Scripts.Messages;

namespace Morbius.Scripts.Events
{
    public class ItemSpawnEvent : DefaultEvent
    {
        [SerializeField]
        private List<Item> m_spawnItems;

        private void Awake()
        {
            if (m_spawnItems == null)
            {
                m_spawnItems = new List<Item>();
            }
            if (m_spawnItems.Count == 0)
            {
                Debug.LogWarning("ItemSpawnEvent: no spawn items configured.");
            }
        }

        public override IEnumerator Execute(int eventId)
        {
            foreach (Item item in m_spawnItems)
            {
                ItemSaveState state = ItemDatabase.GetItemStatus(item);
                state.Spawned = true;
            }
            yield return new WaitForEndOfFrame();
        }
    }

}
