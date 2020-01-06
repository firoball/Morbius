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
    public class DialogTriggerEvent : DefaultEvent
    {
        private DialogPlayer m_dialogPlayer;

        private void Awake()
        {
            m_dialogPlayer = GetComponent<DialogPlayer>();
        }

        public override IEnumerator Execute(int eventId)
        {
            Debug.Log("dialog event " + eventId);
            Inventory.DropHandItem();
            m_dialogPlayer.Play();
            yield return new WaitForEndOfFrame();
        }
    }

}
