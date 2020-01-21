using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Morbius.Scripts.UI
{
    class UISpinner : MonoBehaviour
    {
        [SerializeField]
        private float m_spinSpeed = 45.0f;
        [SerializeField]
        private RectTransform m_transform;

        private bool m_fadeIn = false;
        private float m_angle;

        private const float c_startAngle = 1024.0f;

        private void Awake()
        {
            m_angle = c_startAngle;
        }

        void Update()
        {
            if (m_fadeIn && m_transform != null)
            {
                m_angle = Mathf.Max(0.0f, m_angle - (m_spinSpeed * Time.deltaTime));
                m_transform.eulerAngles = new Vector3(0.0f, 0.0f, m_angle);

                m_transform.localScale = Vector3.one - (Vector3.one * m_angle / c_startAngle);
                if (m_transform.transform.localScale.x >= 1.0f)
                {
                    m_transform.rotation = Quaternion.identity;
                    m_transform.localScale = Vector3.one;
                    m_fadeIn = false;
                }

            }
        }

        public void Show(bool immediately)
        {
            if (m_transform != null)
            {
                if (immediately)
                {
                    m_transform.rotation = Quaternion.identity;
                    m_transform.localScale = Vector3.one;
                }
                else
                {
                    m_angle = c_startAngle;
                    m_transform.eulerAngles = new Vector3(0.0f, 0.0f, m_angle);
                    m_transform.localScale = Vector3.zero;
                    m_fadeIn = true;
                }
            }
        }
    }
}
