using System.Collections;
using UnityEngine;
using Morbius.Scripts.Ambient;
using Morbius.Scripts.Messages;
using Morbius.Scripts.Util;

namespace Morbius.Scripts.Dialog
{
    public class DialogPlayer : MonoBehaviour, IDialogResultMessage
    {
        [SerializeField]
        private GameObject[] m_dialogObjects;

        private bool m_stopped;
        private bool m_isPlaying;
        private Dialog[] m_dialogs;
        private Dialog m_dialog;
        private string m_speaker;

        private const float c_minDisplayTime = 1.5f;
        private const float c_dialogPauseTime = 0.5f;

        public bool IsPlaying { get => m_isPlaying; }
        public string Speaker { get => m_speaker; }

        private void Awake()
        {
            //Dialog requires instance in scene...
            m_dialogs = new Dialog[m_dialogObjects.Length];
            for (int i = 0; i < m_dialogObjects.Length; i++)
            { 
                GameObject obj = Instantiate(m_dialogObjects[i]);
                obj.transform.SetParent(transform);
                m_dialogs[i] = obj.GetComponent<Dialog>();
            }
            m_stopped = true;
            m_isPlaying = false;
            MessageSystem.Register<IDialogResultMessage>(gameObject);
        }

        public void Play()
        {
            Play(0);
        }

        public void Play(int index)
        {
            if (index < 0 || index >= m_dialogs.Length)
            {
                Debug.LogWarning("DialogPlayer: Play() index out of bounds");
                return;
            }

            m_dialog = m_dialogs[index];

            //TODO: security protection against multiple Play() calls while dialog is still running
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
            m_speaker = string.Empty;
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
            m_speaker = text.Speaker;
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
            if (!m_isPlaying) return;

            if (m_dialog.CurrentElement.IsChoice())
            {
                DialogChoices choices = m_dialog.CurrentElement as DialogChoices;
                choices.SelectChoice(index);
                Proceed();
            }
        }

        private IEnumerator Delay(float delay)
        {
            yield return new WaitForSeconds(0.3f);
            yield return new WaitForSecondsAnyKey(delay);
            AudioManager.StopAudio();
            Proceed();
        }

    }
}
