using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Morbius.Scripts.UI
{
    [RequireComponent(typeof(Slider))]
    public class SliderVisualizerUI : MonoBehaviour
    {
        [SerializeField]
        private bool m_normalize = true;

        private Slider m_slider;

        public Slider Slider
        {
            get
            {
                return m_slider;
            }
        }

        private void Awake()
        {
            m_slider = GetComponent<Slider>();
        }

        public void SetMax()
        {
            m_slider.value = m_slider.maxValue;
        }

        public void SetMin()
        {
            m_slider.value = m_slider.minValue;
        }

        public float GetValue()
        {
            float retval = 0.0f;
            if (m_slider)
            {
                retval = m_slider.value;
                if (m_normalize)
                {
                    retval /= m_slider.maxValue;
                }
            }

            return retval;
        }

        public void SetValue(float value)
        {
            if (m_normalize)
            {
                m_slider.value = Mathf.Clamp01(value) * m_slider.maxValue;
            }
            else
            {
                m_slider.value = value;
            }
        }

    }
}
