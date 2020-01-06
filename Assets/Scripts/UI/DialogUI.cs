using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Morbius.Scripts.Messages;

namespace Morbius.Scripts.UI
{
    [RequireComponent(typeof(UIFader))]
    public class DialogUI : MonoBehaviour, IDialogMessage
    {
        [SerializeField]
        private Text m_speaker;
        [SerializeField]
        private Text m_message;
        [SerializeField]
        private DialogDecisionHandler m_decisions;

        private GameObject m_sender;
        private UIFader m_fader;


        private void Awake()
        {
            m_fader = GetComponent<UIFader>();
        }

        private void Start()
        {
            MessageSystem.Register<IDialogMessage>(gameObject);
        }

        public void OnShowText(string speaker, string text)
        {
            if (m_decisions)
            {
                m_decisions.Clear();
            }

            if (m_speaker && m_message && speaker != null && text != null)
            {
                m_speaker.enabled = true;
                m_speaker.text = speaker;
                m_message.enabled = true;
                m_message.text = text;
                m_fader.Show(false);
            }
        }

        public void OnShowDecision(GameObject sender, string[] decisions)
        {
            if (m_speaker)
            {
                m_speaker.enabled = false;
            }

            if (m_message)
            {
                m_message.enabled = false;
            }

            if (decisions != null && m_decisions)
            {
                m_decisions.Clear();
                foreach (string decision in decisions)
                {
                    m_decisions.AddDecision(decision);
                }
                m_fader.Show(false);
                m_sender = sender;
            }
        }

        public void OnHide()
        {
            /* do not remove any elements on hide - this way everything fades out smoothly
             * make sure everything is reset on showing instead
             */
            m_fader.Hide(false);
        }

        public void DecisionNotification(int index)
        {
/*            if (m_sender)
            {
                ExecuteEvents.Execute<IDialogResultMessage>(m_sender, null, (x, y) => x.OnDecision(index));
            }*/
            MessageSystem.Execute<IDialogResultMessage>((x, y) => x.OnDecision(index));
        }

    }
}
