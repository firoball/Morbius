using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Morbius.Scripts.Items;

namespace Morbius.Scripts.Events
{
    [Serializable]
    public class ItemCollectEvent : DefaultEvent
    {
        public override IEnumerator Execute(int eventId)
        {
            Item item = ItemDatabase.GetItemById(eventId);
            Inventory.Collect(item);
            ItemManager.CollectEvent(item);
            yield return new WaitForSeconds(0.01f);
        }
    }

}
