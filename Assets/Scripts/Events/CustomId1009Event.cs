using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Morbius.Scripts.Items;
using Morbius.Scripts.Messages;

namespace Morbius.Scripts.Events
{
    [RequireComponent(typeof(AudioSource))]
    public class CustomId1009Event : DefaultEvent
    {
        private AudioSource m_audio;

        [SerializeField]
        private ParticleSystem m_particleSystem;
        [SerializeField]
        private AudioClip m_audioClip;
        [SerializeField]
        private Item m_targetItem;
        [SerializeField]
        private Item m_morphItem;

        private void Awake()
        {
            m_audio = GetComponent<AudioSource>();
        }

        public override IEnumerator Execute(int eventId)
        {
            MessageSystem.Execute<IInputBlockerMessage>((x, y) => x.OnBlock());

            yield return new WaitForSeconds(1.0f);

            m_particleSystem?.Play();
            m_audio.PlayOneShot(m_audioClip);
            yield return new WaitWhile(() => m_audio.isPlaying);
            m_particleSystem?.Stop();

            m_targetItem.Morph(m_morphItem);

            yield return new WaitForSeconds(0.5f);

            MessageSystem.Execute<IInputBlockerMessage>((x, y) => x.OnUnblock());
            yield return null;
        }
    }

}
