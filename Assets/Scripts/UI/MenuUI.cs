using UnityEngine;
using UnityEngine.UI;
using System;
using Morbius.Scripts.Game;
using Morbius.Scripts.Messages;

namespace Morbius.Scripts.UI
{
    [RequireComponent(typeof(UIFader))]
    public class MenuUI : MonoBehaviour, IInputBlockerMessage
    {

        private UIFader m_fader;

        private void Awake()
        {
            m_fader = GetComponent<UIFader>();
            MessageSystem.Register<IInputBlockerMessage>(gameObject);
        }

        public void OnBlock()
        {
            m_fader.Hide(false);
        }

        public void OnUnblock()
        {
            m_fader.Show(false);
        }
    }
}
