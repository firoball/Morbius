using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Morbius.Scripts.Dialog;
using Morbius.Scripts.Items;

namespace Morbius.Scripts.Events
{
    [RequireComponent(typeof(DialogPlayer))]
    public class DialogPortalTriggerEvent : PortalTriggerEvent
    {
        private DialogPlayer m_dialogPlayer;

        private new void Awake()
        {
            base.Awake();
            m_dialogPlayer = GetComponent<DialogPlayer>();
        }

        public override IEnumerator Execute(int eventId)
        {
            Inventory.DropHandItem();
            if (!m_isLocked)
            {
                EventManager.CancelEvents();
                m_dialogPlayer.Play(0);
                yield return new WaitWhile(() => m_dialogPlayer.IsPlaying);
                yield return base.Execute(eventId);
            }
            yield return null;
        }
    }

}
