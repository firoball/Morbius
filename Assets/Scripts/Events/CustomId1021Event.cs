using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Morbius.Scripts.Dialog;
using Morbius.Scripts.Level;
using Morbius.Scripts.Messages;
using Morbius.Scripts.Util;


namespace Morbius.Scripts.Events
{
    [RequireComponent(typeof(DialogPlayer))]
    [RequireComponent(typeof(ScenePortal))]
    public class CustomId1021Event : DefaultEvent
    {
        private DialogPlayer m_dialogPlayer;
        private ScenePortal m_portal;

        [SerializeField]
        private Sprite m_sprite;

        private void Awake()
        {
            m_dialogPlayer = GetComponent<DialogPlayer>();
            m_portal = GetComponent<ScenePortal>();
        }

        public override IEnumerator Execute(int eventId)
        {
            MessageSystem.Execute<IInputBlockerMessage>((x, y) => x.OnBlock());

            MessageSystem.Execute<IPanelMessage>((x, y) => x.OnShow(m_sprite));

            yield return new WaitForSeconds(1.5f);
            yield return new WaitForSecondsAnyKey(30.0f);
            MessageSystem.Execute<IPanelMessage>((x, y) => x.OnHide());
            yield return new WaitForSeconds(2.0f);

            m_dialogPlayer.Play(0);
            yield return new WaitWhile(() => m_dialogPlayer.IsPlaying);

            m_portal.Load();

            MessageSystem.Execute<IInputBlockerMessage>((x, y) => x.OnUnblock());
            yield return null;
        }
    }

}
