using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Morbius.Scripts.Dialog;
using Morbius.Scripts.Messages;

namespace Morbius.Scripts.Events
{
    [RequireComponent(typeof(DialogPlayer))]
    [RequireComponent(typeof(AudioSource))]
    public class CustomId41Event : DefaultEvent
    {
        private DialogPlayer m_dialogPlayer;
        private AudioSource m_audio;

        [SerializeField]
        private AudioClip m_audio1;
        [SerializeField]
        private AudioClip m_audio2;

        private void Awake()
        {
            m_dialogPlayer = GetComponent<DialogPlayer>();
            m_audio = GetComponent<AudioSource>();
        }

        public override IEnumerator Execute(int eventId)
        {
            MessageSystem.Execute<IInputBlockerMessage>((x, y) => x.OnBlock());
            m_dialogPlayer.Play(0);
            yield return new WaitWhile(() => m_dialogPlayer.IsPlaying);

            m_audio.PlayOneShot(m_audio1);
            yield return new WaitWhile(() => m_audio.isPlaying);

            m_dialogPlayer.Play(1);
            yield return new WaitWhile(() => m_dialogPlayer.IsPlaying);

            m_audio.PlayOneShot(m_audio2);
            yield return new WaitWhile(() => m_audio.isPlaying);
            MessageSystem.Execute<IInputBlockerMessage>((x, y) => x.OnUnblock());
        }
    }

}
