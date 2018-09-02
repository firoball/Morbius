using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Morbius.Scripts.Events
{
    [Serializable]
    public class ItemCollectEvent : DefaultEvent
    {
        public override IEnumerator Execute(int eventId)
        {
            yield return new WaitForSeconds(1.0f);
        }
    }

}
