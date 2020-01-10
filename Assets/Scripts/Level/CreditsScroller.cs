using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using Morbius.Scripts.Ambient;
using Morbius.Scripts.UI;

namespace Morbius.Scripts.Level
{
    [RequireComponent(typeof(UIFader))]
    public class CreditsScroller : MonoBehaviour
    {
        [SerializeField]
        private float m_scrollSpeed;
        [SerializeField]
        private float m_fastScrollFactor;
        [SerializeField]
        private RectTransform m_creditsUI;
        [SerializeField]
        private ScenePortal m_scenePortal;

        private float m_minY;
        private float m_maxY;
        private float m_y;
        private float m_height;
        private bool m_done;
        private bool m_fastScroll;
        private bool m_fastScrollOld;
        private UIFader m_fader;

        private void Start()
        {
            m_fader = GetComponent<UIFader>();
            m_fader.Show(false);

            if (m_creditsUI)
            {
                m_height = m_creditsUI.rect.height;
                //m_creditsUI.offsetMax = new Vector2(m_creditsUI.offsetMax.x, m_creditsUI.offsetMax.y);
                //m_creditsUI.offsetMin = new Vector2(m_creditsUI.offsetMin.x, m_creditsUI.offsetMin.y);
                m_minY = m_creditsUI.offsetMax.y;
                m_maxY = m_height + m_minY;
                m_y = m_minY;

                m_done = false;
                m_fastScroll = false;
                m_fastScrollOld = false;
            }
        }

        private void Update()
        {
            if (m_creditsUI)
            {
                float speed = m_scrollSpeed;
                if (m_fastScroll)
                    speed *= m_fastScrollFactor;
                m_y = Mathf.Min(m_maxY, Mathf.Max(m_y + speed * Time.deltaTime, m_minY));
                m_creditsUI.offsetMax = new Vector2(m_creditsUI.offsetMax.x, m_y);
                m_creditsUI.offsetMin = new Vector2(m_creditsUI.offsetMin.x, m_y - m_height);
                if (m_y >= m_maxY && !m_done)
                {
                    TriggerDone();
                }
            }

            if (Input.GetButton("Cancel"))
            {
                TriggerDone();
            }
            else if (Input.anyKey && m_fader.IsEnabled())
            {
                m_fastScroll = true;
            }
            else
            {
                m_fastScroll = false;
            }

            if (m_fastScroll != m_fastScrollOld)
            {
                if (m_fastScroll)
                {
                    AudioManager.PitchMusic(m_fastScrollFactor);
                }
                else
                {
                    AudioManager.PitchMusic(1.0f);
                }
            }
            m_fastScrollOld = m_fastScroll;
        }

        private void TriggerDone()
        {
            m_done = true;
            if (m_scenePortal)
            {
                StartCoroutine(Done());
            }
        }

        private IEnumerator Done()
        {
            AudioManager.FadeAndStop(0.3f);
            yield return new WaitForSeconds(0.2f);
            m_fader.Hide(false);
            yield return new WaitForSeconds(2.0f);
            m_scenePortal.Load();
        }

    }
}
