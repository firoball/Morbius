using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Morbius.Scripts.Dialog;
using Morbius.Scripts.Level;
using Morbius.Scripts.Messages;
using Morbius.Scripts.Movement;
using Morbius.Scripts.Util;


namespace Morbius.Scripts.Events
{
    [RequireComponent(typeof(ScenePortal))]
    public class CustomId1020Event : DefaultEvent
    {
        private ScenePortal m_portal;

        [SerializeField]
        private Transform m_sitPosition;
        [SerializeField]
        private Transform m_standPosition;
        [SerializeField]
        private PlayerNavigator m_navigator;
        [SerializeField]
        private Sprite m_spriteNewspaper;
        [SerializeField]
        private Sprite m_spriteEmail;

        private void Awake()
        {
            m_portal = GetComponent<ScenePortal>();
        }

        public override IEnumerator Execute(int eventId)
        {
            MessageSystem.Execute<IInputBlockerMessage>((x, y) => x.OnBlock());

            m_navigator.SetDestination(m_standPosition.position, true);
            yield return new WaitWhile(() => m_navigator.IsMoving());

            MessageSystem.Execute<ISitMessage>((x, y) => x.OnSit(m_sitPosition));
            yield return new WaitForSeconds(1.0f);

            MessageSystem.Execute<IPanelMessage>((x, y) => x.OnShow(m_spriteEmail, true));
            yield return new WaitForSeconds(1.5f);
            yield return new WaitForSecondsAnyKey(30.0f);
            MessageSystem.Execute<IPanelMessage>((x, y) => x.OnHide());
            yield return new WaitForSeconds(2.0f);

            MessageSystem.Execute<IPanelMessage>((x, y) => x.OnShow(m_spriteNewspaper));
            yield return new WaitForSeconds(1.5f);
            yield return new WaitForSecondsAnyKey(30.0f);
            MessageSystem.Execute<IPanelMessage>((x, y) => x.OnHide());
            yield return new WaitForSeconds(2.0f);

            m_portal.Load();

            MessageSystem.Execute<IInputBlockerMessage>((x, y) => x.OnUnblock());
            yield return null;
        }
    }

}
