using UnityEngine;
using UnityEngine.UI;
using Morbius.Scripts.Messages;

namespace Morbius.Scripts.UI
{
    public class BlockerUI : MonoBehaviour, IInputBlockerMessage
    {
        private RawImage m_image;

        private void Awake()
        {
            m_image = GetComponent<RawImage>();
            MessageSystem.Register<IInputBlockerMessage>(gameObject);
        }

        public void OnBlock()
        {
            m_image.enabled = true;
            m_image.raycastTarget = true;
        }

        public void OnUnblock()
        {
            m_image.enabled = false;
            m_image.raycastTarget = false;
        }
    }
}
