using UnityEngine;
using UnityEngine.UI;
using Morbius.Scripts.Messages;

namespace Morbius.Scripts.UI
{
    [RequireComponent(typeof(UIFader))]
    public class InfoTextUI : MonoBehaviour, IInfoTextMessage
    {
        [SerializeField]
        private Text m_text;

        private UIFader m_fader;
        private float m_timer;


        private void Awake()
        {
            m_fader = GetComponent<UIFader>();
            m_timer = 0.0f;
            MessageSystem.Register<IInfoTextMessage>(gameObject);
        }

        private void Update()
        {
            //timer is handled in Update instead of Coroutines in order to avoid triggering multiple coroutines
            if (m_timer > 0.0f)
            {
                m_timer = Mathf.Max(m_timer - Time.deltaTime, 0.0f);
                if (m_timer <= 0.0f)
                {
                    m_fader.Hide(false);
                }
            }
        }

        public void OnShow(string text, float duration)
        {
            if (m_text && text != null)
            {
                m_timer = Mathf.Max(0.0f, duration);
                m_text.text = text;
                m_fader.Show(false);
            }
        }

        public void OnShow(string text)
        {
            OnShow(text, 0.0f);
        }

        public void OnHide()
        {
            /* do not remove any elements on hide - this way everything fades out smoothly
             * make sure everything is reset on showing instead
             */
            m_timer = 0.0f;
            m_fader.Hide(false);
        }
    }
}
