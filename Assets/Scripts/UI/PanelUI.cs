using UnityEngine;
using UnityEngine.UI;
using Morbius.Scripts.Messages;

namespace Morbius.Scripts.UI
{
    [RequireComponent(typeof(UIFader))]
    [RequireComponent(typeof(UISpinner))]
    public class PanelUI : MonoBehaviour, IPanelMessage
    {
        [SerializeField]
        private Sprite m_test;
        [SerializeField]
        private Image m_image;

        private UIFader m_fader;
        private UISpinner m_spinner;


        private void Awake()
        {
            m_fader = GetComponent<UIFader>();
            m_spinner = GetComponent<UISpinner>();
            MessageSystem.Register<IPanelMessage>(gameObject);

        }

        private void Start()
        {
            //demo
            //OnShow(m_test);
        }

        public void OnShow(Sprite sprite)
        {
            OnShow(sprite, false);
        }

        public void OnShow(Sprite sprite, bool nospin)
        {
            if (m_image != null && sprite != null)
            {
                m_image.sprite = sprite;
                m_fader.Show(false);
                m_spinner.Show(nospin);
            }
        }

        public void OnHide()
        {
            m_fader.Hide(false);
        }
    }
}
