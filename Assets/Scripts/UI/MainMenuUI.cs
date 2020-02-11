using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Morbius.Scripts.Level;

namespace Morbius.Scripts.UI
{
    [RequireComponent(typeof(UIFader))]
    public class MainMenuUI : MonoBehaviour
    {
        private UIFader m_fader;

        [SerializeField]
        private Button m_quitButton;

        private void Awake()
        {
            m_fader = GetComponent<UIFader>();
            if (Application.platform == RuntimePlatform.WebGLPlayer && m_quitButton)
            {
                Destroy(m_quitButton.gameObject);
            }
        }

        public void OnMenuOpen()
        {
            m_fader.Show(false);
        }

        public void OnMenuClose()
        {
            m_fader.Hide(false);
        }

        public void OnQuit()
        {
            m_fader.Hide(false);
            StartCoroutine(Delay());
        }

        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(0.5f);
            Application.Quit();
            yield return null;
        }
    }
}
