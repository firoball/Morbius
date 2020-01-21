using UnityEngine;
using UnityEngine.UI;
using Morbius.Scripts.Messages;

namespace Morbius.Scripts.UI
{
    [RequireComponent(typeof(RawImage))]
    [RequireComponent(typeof(Canvas))]
    public class BlockerUI : MonoBehaviour, IInputBlockerMessage
    {
        private RawImage m_image;
        private Canvas m_canvas;

        private void Awake()
        {
            m_image = GetComponent<RawImage>();
            m_canvas = GetComponent<Canvas>();
            m_canvas.enabled = false;
            MessageSystem.Register<IInputBlockerMessage>(gameObject);
        }

        public void OnBlock()
        {
            m_image.enabled = true;
            m_image.raycastTarget = true;
            m_canvas.enabled = true;
        }

        public void OnUnblock()
        {
            m_canvas.enabled = false;
            m_image.enabled = false;
            m_image.raycastTarget = false;
        }
    }
}
