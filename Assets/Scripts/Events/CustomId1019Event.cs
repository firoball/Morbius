using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Morbius.Scripts.Game;
using Morbius.Scripts.Items;

namespace Morbius.Scripts.Events
{
    public class CustomId1019Event : DefaultEvent
    {
        [SerializeField]
        private Item m_newspaper;
        [SerializeField]
        private Item m_laptopOff;
        [SerializeField]
        private Item m_laptopOn;

        public override IEnumerator Execute(int eventId)
        {
            if (!GameStatus.Data.GoodEnding)
            { 
                //trigger bad ending from newspaper
                if (m_newspaper)
                {
                    ItemSaveState state = ItemDatabase.GetItemStatus(m_newspaper);
                    state.Spawned = true;
                }
                //avoid trigger good ending from laptop
                if (m_laptopOn && m_laptopOff)
                {
                    m_laptopOn.Morph(m_laptopOff);
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

}
