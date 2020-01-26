using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Morbius.Scripts.Dialog;
using Morbius.Scripts.Messages;
using Morbius.Scripts.Movement;

namespace Morbius.Scripts.Events
{
    [RequireComponent(typeof(DialogPlayer))]
    public class CustomId1010Event : DefaultEvent
    {
        private DialogPlayer m_dialogPlayer;
        
        [SerializeField]
        private PlayerNavigator m_navigator;
        [SerializeField]
        private Transform m_sitPosition;
        [SerializeField]
        private Transform m_standPosition;

        private void Awake()
        {
            m_dialogPlayer = GetComponent<DialogPlayer>();
        }

        public override IEnumerator Execute(int eventId)
        {
            if (m_navigator)
            {
                MessageSystem.Execute<IInputBlockerMessage>((x, y) => x.OnBlock());
                m_navigator.SetDestination(m_standPosition.position, true);
                yield return new WaitWhile(() => m_navigator.IsMoving());

                MessageSystem.Execute<ISitMessage>((x, y) => x.OnSit(m_sitPosition));
                yield return new WaitForSeconds(1.0f);

                m_dialogPlayer.Play(0);
                yield return new WaitWhile(() => m_dialogPlayer.IsPlaying);

                yield return new WaitForSeconds(0.5f);
                MessageSystem.Execute<ISitMessage>((x, y) => x.OnStand(m_standPosition));

                MessageSystem.Execute<IUnlockTriggerMessage>((x, y) => x.OnUnlock());
                MessageSystem.Execute<IInputBlockerMessage>((x, y) => x.OnUnblock());
            }
            yield return null;
        }
    }

}
