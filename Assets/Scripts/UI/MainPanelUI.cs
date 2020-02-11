using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Morbius.Scripts.UI
{
    [RequireComponent(typeof(UIFader))]
    [RequireComponent(typeof(Button))]
    public class MainPanelUI : MonoBehaviour
    {
        private UIFader m_fader;
        private Button m_button;
        private bool m_pressed;

        private void Awake()
        {
            m_fader = GetComponent<UIFader>();
            m_button = GetComponent<Button>();
            m_pressed = false;
        }

        private void Update()
        {
            if (!m_pressed && Input.anyKeyDown)
            {
                m_pressed = true;
                m_fader.Hide(false);
                StartCoroutine(Delay());
            }
        }

        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(1.0f);
            m_button.onClick.Invoke();
            yield return null;
        }
    }
}
