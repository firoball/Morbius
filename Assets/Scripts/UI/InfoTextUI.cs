using UnityEngine;
using UnityEngine.UI;

namespace Morbius.Scripts.UI
{
    [RequireComponent(typeof(UIFader))]
    public class InfoTextUI : MonoBehaviour, IInfoTextEventTarget
    {
        [SerializeField]
        private Text m_text;

        private UIFader m_fader;


        private void Awake()
        {
            m_fader = GetComponent<UIFader>();
        }

        public void OnShow(string text)
        {
            if (m_text && text != null)
            {
                m_text.text = text;
                m_fader.Show(false);
            }
        }

        public void OnHide()
        {
            /* do not remove any elements on hide - this way everything fades out smoothly
             * make sure everything is reset on showing instead
             */
            m_fader.Hide(false);
        }
    }
}
