using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Morbius.Scripts.Ambient;
using Morbius.Scripts.Shaders;
using Morbius.Scripts.UI;

namespace Morbius.Scripts.Level
{
    public class ScenePortal : MonoBehaviour, IChapterResultTarget
    {
        [SerializeField]
        private int m_sceneId;
        [SerializeField]
        private Chapter m_chapter;
        [SerializeField]
        private GameObject m_camera;
        [SerializeField]
        private GameObject m_chapterUI;

        private bool m_started;

        private const float c_uiDelay = 1.0f;
        private const float c_fadeSpeedSlow = 0.4f;
        private const float c_fadeSpeedFast = 3.0f;

        private void Start()
        {
            m_sceneId = Math.Min(Math.Max(0, m_sceneId), SceneManager.sceneCountInBuildSettings - 1);
            m_started = false;
        }

        public void Load()
        {
            //only load different scenes
            if (!m_started && SceneManager.GetActiveScene().buildIndex != m_sceneId)
            {
                m_started = true;
                StartCoroutine(Pixelate());
            }
        }

        private void LoadScene()
        {
            SceneManager.LoadScene(m_sceneId);
        }

        private void StartTypewriter()
        {
            if (m_chapter)
            {
                AudioManager.FadeAndStop(c_fadeSpeedFast);
                ExecuteEvents.Execute<IChapterEventTarget>(m_chapterUI, null, (x, y) => x.OnSetText(m_chapter.Title, m_chapter.Text));
                ExecuteEvents.Execute<IChapterEventTarget>(m_chapterUI, null, (x, y) => x.OnShow(gameObject));
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
            ExecuteEvents.Execute<IPixelProgressEventTarget>(m_camera, null, (x, y) => x.OnPixelate());
            yield return new WaitForSeconds(c_uiDelay);
            StartTypewriter();
        }

    }
}
