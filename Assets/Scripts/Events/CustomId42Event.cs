using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Morbius.Scripts.Dialog;
using Morbius.Scripts.Items;
using Morbius.Scripts.Level;
using Morbius.Scripts.Messages;

namespace Morbius.Scripts.Events
{
    [RequireComponent(typeof(DialogPlayer))]
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(ScenePortal))]
    public class CustomId42Event : DefaultEvent
    {
        private DialogPlayer m_dialogPlayer;
        private AudioSource m_audio;
        private ScenePortal m_portal;

        [SerializeField]
        private AudioClip m_audio1;
        [SerializeField]
        private AudioClip m_audio2;
        [SerializeField]
        private Item m_destroyItem;

        private void Awake()
        {
            m_dialogPlayer = GetComponent<DialogPlayer>();
            m_audio = GetComponent<AudioSource>();
            m_portal = GetComponent<ScenePortal>();
        }

        public override IEnumerator Execute(int eventId)
        {
            MessageSystem.Execute<IInputBlockerMessage>((x, y) => x.OnBlock());
            m_dialogPlayer.Play(0);
            yield return new WaitWhile(() => m_dialogPlayer.IsPlaying);

            ItemSaveState state = ItemDatabase.GetItemStatus(m_destroyItem);
            if (state != null)
            {
                state.Destroyed = true;
                ItemDatabase.SetItemStatus(m_destroyItem, state);
            }

            m_audio.PlayOneShot(m_audio1);
            yield return new WaitWhile(() => m_audio.isPlaying);

            m_dialogPlayer.Play(1);
            yield return new WaitWhile(() => m_dialogPlayer.IsPlaying);

            m_audio.PlayOneShot(m_audio2);
            yield return new WaitForSeconds(1.5f);

            m_dialogPlayer.Play(2);
            yield return new WaitWhile(() => m_dialogPlayer.IsPlaying);
            m_portal.Load();
            MessageSystem.Execute<IInputBlockerMessage>((x, y) => x.OnUnblock());
        }
    }

}
