using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Morbius.Scripts.Ambient;
using Morbius.Scripts.Messages;

namespace Morbius.Scripts.Level
{
    public class ScenePortal : MonoBehaviour, IChapterResultMessage
    {
        [SerializeField]
        private string m_sceneName;
        [SerializeField]
        private Chapter m_chapter;

        private bool m_started;

        private const float c_uiDelay = 1.0f;
        private const float c_fadeSpeedSlow = 0.4f;
        private const float c_fadeSpeedFast = 3.0f;

        private void Awake()
        {
            MessageSystem.Register<IChapterResultMessage>(gameObject);
        }

        private void Start()
        {
            m_started = false;
            
        }

        public void Load()
        {
            if (string.IsNullOrWhiteSpace(m_sceneName) || !Application.CanStreamedLevelBeLoaded(m_sceneName))//!SceneManager.GetSceneByName(m_sceneName).IsValid())
            {
                Debug.LogWarning("ScenePortal.Load: <" + m_sceneName + "> is not a valid scene.");
                return;
            }

            //only load different scenes
            if (!m_started && SceneManager.GetActiveScene().name != m_sceneName)
            {
                m_started = true;
                StartCoroutine(Pixelate());
            }
        }

        private void LoadScene()
        {
            if (m_started)
                SceneManager.LoadScene(m_sceneName);
        }

        private void StartTypewriter()
        {
            if (m_chapter)
            {
                AudioManager.FadeAndStop(c_fadeSpeedFast);
                MessageSystem.Execute<IChapterMessage>((x, y) => x.OnSetText(m_chapter.Title, m_chapter.Text));
                MessageSystem.Execute<IChapterMessage>((x, y) => x.OnShow());
                MessageSystem.Execute<IInfoTextMessage>((x, y) => x.OnHide());
            }
            else
            {
                LoadScene();
            }
        }

        public void OnChapterDone()
        {
            LoadScene();
        }


        private IEnumerator Pixelate()
        {
            AudioManager.FadeAndStop(c_fadeSpeedSlow);
            MessageSystem.Execute<IPixelProgressMessage>((x, y) => x.OnPixelate());
            yield return new WaitForSeconds(c_uiDelay);
            StartTypewriter();
        }

    }
}
