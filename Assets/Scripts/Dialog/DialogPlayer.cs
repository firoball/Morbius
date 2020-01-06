using System.Collections;
using UnityEngine;
using Morbius.Scripts.Ambient;
using Morbius.Scripts.Messages;

namespace Morbius.Scripts.Dialog
{
    public class DialogPlayer : MonoBehaviour, IDialogResultMessage
    {
        [SerializeField]
        private GameObject m_dialogObject;

        private bool m_stopped;
        private bool m_isPlaying;
        private Dialog m_dialog;

        private const float c_minDisplayTime = 1.5f;
        private const float c_dialogPauseTime = 0.5f;

        public bool IsPlaying { get => m_isPlaying; }

        private void Awake()
        {
            //Dialog requires instance in scene...
            GameObject obj = Instantiate(m_dialogObject);
            obj.transform.SetParent(transform);
            m_dialog = obj.GetComponent<Dialog>();
            m_stopped = true;
            m_isPlaying = false;
        }

        private void Start()
        {
            MessageSystem.Register<IDialogResultMessage>(gameObject);
        }

        public void Play()
        {
            if (m_dialog)
            {
                m_dialog.Restart();
                m_stopped = false;
                m_isPlaying = true;
                Execute(m_dialog);
            }
            else
            {
                Debug.LogWarning("DialogPlayer: Dialog Component not found");
            }
        }

        public void Stop()
        {
            m_stopped = true;
        }

        private void Execute(Dialog dialog)
        {
            if (dialog.IsFinished() || m_stopped)
            {
                m_isPlaying = false;
                MessageSystem.Execute<IDialogMessage>((x, y) => x.OnHide());
                return;
            }

            DialogElement element = dialog.CurrentElement;
            if (element.IsChoice())
            {
                DialogChoices choices = element as DialogChoices;
                ShowChoices(choices);
            }
            else
            {
                DialogText text = element as DialogText;
                ShowText(text);
            }
        }

        private void ShowText(DialogText text)
        {
            MessageSystem.Execute<IDialogMessage>((x, y) => x.OnShowText(text.Speaker, text.Text));
            AudioManager.ScheduleVoice(text.Clip);

            float delay = Mathf.Max(c_minDisplayTime, text.Clip.length + c_dialogPauseTime);
            StartCoroutine(Delay(delay));
        }

        private void ShowChoices (DialogChoices choices)
        {
            string[] decisions = choices.GetChoices();
            MessageSystem.Execute<IDialogMessage>((x, y) => x.OnShowDecision(decisions));
        }

        private void Proceed()
        {
            m_dialog.Proceed();
            Execute(m_dialog);
        }

        public void OnDecision(int index)
        {
            if (m_dialog.CurrentElement.IsChoice())
            {
                DialogChoices choices = m_dialog.CurrentElement as DialogChoices;
                choices.SelectChoice(index);
                Proceed();
            }
        }

        private IEnumerator Delay(float delay)
        {
            yield return new WaitForSeconds(delay);
            Proceed();
        }

    }
}
