using UnityEngine;

namespace Morbius.Scripts.Ambient
{
    [RequireComponent(typeof(Light))]
    public class DiscoLight : MonoBehaviour
    {
        [SerializeField]
        private float m_colorSpeed = 0.1f;
        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float m_colorOffset = 0.0f;
        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float m_minHue = 0.0f;
        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float m_maxHue = 1.0f;
        [SerializeField]
        private bool m_pingPong = false;
        [SerializeField]
        private float m_rotationSpeed = 0.0f;

        private float m_hue;
        private Light m_light;
        private float m_pingPongFac;

        void Start()
        {
            m_hue = m_colorOffset;
            m_light = GetComponent<Light>();
            m_pingPongFac = 1.0f;
        }

        void Update()
        {
            transform.Rotate(Vector3.up, m_rotationSpeed * Time.deltaTime, Space.World);
            m_hue += Time.deltaTime * m_colorSpeed * m_pingPongFac;
            if (m_pingPong)
            {
                if (m_hue > m_maxHue)
                {
                    m_hue = m_maxHue;
                    m_pingPongFac *= -1.0f;
                }
                if (m_hue < m_minHue)
                {
                    m_hue = m_minHue;
                    m_pingPongFac *= -1.0f;
                }
            }
            else
            {
                if (m_hue > m_maxHue) m_hue = m_minHue + (m_hue - m_maxHue);
                if (m_hue < m_minHue) m_hue = m_maxHue - (m_minHue - m_hue);
            }
            m_light.color = Color.HSVToRGB(m_hue, 1.0f, 1.0f);
        }
    }
}
