using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Morbius.Scripts.Items;

namespace Morbius.Scripts.Events
{
    public class ItemMorphEvent : DefaultEvent
    {
        [SerializeField]
        private Item m_targetItem;
        [SerializeField]
        private Item m_morphItem;

        public override IEnumerator Execute(int eventId)
        {
            m_targetItem?.Morph(m_morphItem);
            yield return new WaitForEndOfFrame();
        }
    }

}
