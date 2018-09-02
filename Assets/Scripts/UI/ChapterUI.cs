using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Morbius.Scripts.UI
{
    [RequireComponent(typeof(UIFader))]
    [RequireComponent(typeof(AudioSource))]
    public class ChapterUI : MonoBehaviour, IPointerClickHandler, IChapterEventTarget
    {
        [SerializeField]
        private AudioClip m_letter;
        [SerializeField]
        private AudioClip m_space;
        [SerializeField]
        private AudioClip m_newline;
        [SerializeField]
        private Text m_title;
        [SerializeField]
        private Text m_text;

        private bool m_fastType = false;
        private string m_titleStr;
        private string[] m_textStr;
        private UIFader m_fader;
        private AudioSource m_audio;
        private GameObject m_sender;

        private const float c_typeDelay = 0.25f;
        private const float c_doneDelay = 1.0f;
        private const float c_fadeDelay = 0.3f;

        private void Start()
        {
            m_textStr = new string[] { "", "", "" };
            m_fader = GetComponent<UIFader>();
            m_audio = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (Input.anyKey && m_fader.IsEnabled())
            {
                m_fastType = true;
            }
        }

        public void OnPointerClick(PointerEventData data)
        {
            if (m_fader.IsEnabled())
            {
                m_fastType = true;
            }
        }

        public void OnShow(GameObject sender)
        {
            m_sender = sender;
            m_text.text = "";
            m_title.text = "";
            m_fader.Show(true);
            StartCoroutine(Typewriter());
        }

        public void OnHide()
        {
            m_fader.Hide(false);
        }

        public void OnSetText(string title, string[] text)
        {
            m_titleStr = title;
            m_textStr = text;
        }

        private IEnumerator Typewriter()
        {
            yield return StartCoroutine(AssembleTitle(m_titleStr));
            yield return StartCoroutine(AssembleText(m_textStr));

            yield return new WaitForSeconds(c_doneDelay);
            m_fader.Hide(false);
            yield return new WaitForSeconds(c_fadeDelay);
            ExecuteEvents.Execute<IChapterResultTarget>(m_sender, null, (x, y) => x.OnChapterDone());
        }

        private IEnumerator AssembleTitle(string source)
        {
            string fragment = "";
            for (int i = 0; i < source.Length && !m_fastType; i++)
            {
                fragment += source[i];
                m_title.text = fragment;
                if (source[i] == ' ')
                    m_audio.PlayOneShot(m_space);
                else
                    m_audio.PlayOneShot(m_letter);
                yield return new WaitForSeconds(c_typeDelay);
            }

            if (m_fastType)
            {
                m_title.text = source;
            }
            else
            {
                yield return new WaitForSeconds(c_typeDelay);
                m_audio.PlayOneShot(m_newline);
                yield return new WaitForSeconds(c_typeDelay);
            }
        }

        private IEnumerator AssembleText(string[] source)
        {
            string[] fragment = new string[] { "", "", "" };
            int count = Math.Min(source.Length, fragment.Length);

            for (int text = 0; text < count && !m_fastType; text++)
            {
                string line = source[text];
                for (int i = 0; i < line.Length && !m_fastType; i++)
                {
                    fragment[text] += line[i];
                    m_text.text = String.Join("\n", fragment);
                    if (line[i] == ' ')
                        m_audio.PlayOneShot(m_space);
                    else
                        m_audio.PlayOneShot(m_letter);
                    yield return new WaitForSeconds(c_typeDelay);

                }

                if (!m_fastType)
                {
                    yield return new WaitForSeconds(c_typeDelay);
                    if (text < count - 1)
                    {
                        m_audio.PlayOneShot(m_newline);
                        yield return new WaitForSeconds(c_typeDelay);
                    }
                }
            }

            if (m_fastType)
            {
                m_text.text = String.Join("\n", source);
            }
        }

    }
}
