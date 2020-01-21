using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Morbius.Scripts.Dialog;
using Morbius.Scripts.Messages;
using Morbius.Scripts.Util;

namespace Morbius.Scripts.Events
{
    [RequireComponent(typeof(DialogPlayer))]
    [RequireComponent(typeof(AudioSource))]
    public class CustomId1001Event : DefaultEvent
    {
        private DialogPlayer m_dialogPlayer;
        private AudioSource m_audio;

        [SerializeField]
        private AudioClip m_audioClip;
        [SerializeField]
        private Transform m_sitPosition;
        [SerializeField]
        private Sprite m_sprite;

        private void Awake()
        {
            m_dialogPlayer = GetComponent<DialogPlayer>();
            m_audio = GetComponent<AudioSource>();
        }

        private void Start()
        {
            //Autostart Event
            EventManager.RaiseEvent(1001);
        }

        public override IEnumerator Execute(int eventId)
        {
            MessageSystem.Execute<IInputBlockerMessage>((x, y) => x.OnBlock());

            MessageSystem.Execute<ISitMessage>((x, y) => x.OnSit(m_sitPosition));
            yield return new WaitForSeconds(1.0f);
            MessageSystem.Execute<IPanelMessage>((x, y) => x.OnShow(m_sprite));

            yield return new WaitForSeconds(1.5f);
            yield return new WaitForSecondsAnyKey(10.0f);
            MessageSystem.Execute<IPanelMessage>((x, y) => x.OnHide());
            yield return new WaitForSeconds(2.0f);

            m_dialogPlayer.Play(0);
            yield return new WaitWhile(() => m_dialogPlayer.IsPlaying);

            m_dialogPlayer.Play(1);
            yield return new WaitWhile(() => m_dialogPlayer.IsPlaying);

            m_audio.PlayOneShot(m_audioClip);
            yield return new WaitWhile(() => m_audio.isPlaying);
            yield return new WaitForSeconds(1.0f);

            m_dialogPlayer.Play(2);
            yield return new WaitWhile(() => m_dialogPlayer.IsPlaying);

            yield return new WaitForSeconds(0.5f);
            MessageSystem.Execute<ISitMessage>((x, y) => x.OnStand());

            MessageSystem.Execute<IInputBlockerMessage>((x, y) => x.OnUnblock());
        }
    }

}
