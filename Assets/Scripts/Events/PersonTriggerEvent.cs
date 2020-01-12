using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Morbius.Scripts.Dialog;
using Morbius.Scripts.Items;
using Morbius.Scripts.Messages;

namespace Morbius.Scripts.Events
{
    [RequireComponent(typeof(DialogPlayer))]
    public class PersonTriggerEvent : DefaultEvent
    {
        [SerializeField]
        private bool m_unlockGate = false;

        private DialogPlayer m_dialogPlayer;
        private bool m_dialogPlayed;

        private void Awake()
        {
            m_dialogPlayer = GetComponent<DialogPlayer>();
            m_dialogPlayed = false;
        }

        public override IEnumerator Execute(int eventId)
        {
            Inventory.DropHandItem();
            if (!m_dialogPlayed)
            {
                m_dialogPlayed = true;
                m_dialogPlayer.Play();
                EventManager.CancelEvents();
                if (m_unlockGate)
                    MessageSystem.Execute<IUnlockTriggerMessage>((x, y) => x.OnUnlock());
            }
            yield return new WaitForEndOfFrame();
        }
    }

}
