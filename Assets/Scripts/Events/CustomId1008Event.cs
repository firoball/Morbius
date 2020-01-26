using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Morbius.Scripts.Dialog;
using Morbius.Scripts.Items;
using Morbius.Scripts.Messages;
using Morbius.Scripts.Movement;

namespace Morbius.Scripts.Events
{
    [RequireComponent(typeof(DialogPlayer))]
    public class CustomId1008Event : DefaultEvent
    {
        private DialogPlayer m_dialogPlayer;
        private AudioSource m_audio;

        [SerializeField]
        private PlayerNavigator m_navigator;
        [SerializeField]
        private Transform m_sitPosition;
        [SerializeField]
        private Transform m_standPosition;
        [SerializeField]
        private Item m_food;
        [SerializeField]
        private Item m_fakeFood;

        private void Awake()
        {
            m_dialogPlayer = GetComponent<DialogPlayer>();
        }

        public override IEnumerator Execute(int eventId)
        {
            MessageSystem.Execute<IInputBlockerMessage>((x, y) => x.OnBlock());

            if (m_food && m_fakeFood)
            {
                ItemSaveState status = ItemDatabase.GetItemStatus(m_food);
                status.MorphItem = m_fakeFood;
            }
            MessageSystem.Execute<ISitMessage>((x, y) => x.OnSit(m_sitPosition));
            yield return new WaitForSeconds(2.0f);

            m_dialogPlayer.Play(0);
            yield return new WaitWhile(() => m_dialogPlayer.IsPlaying);

            yield return new WaitForSeconds(0.5f);
            MessageSystem.Execute<ISitMessage>((x, y) => x.OnStand(m_standPosition));

            MessageSystem.Execute<IInputBlockerMessage>((x, y) => x.OnUnblock());
            yield return null;
        }
    }

}
