using System;
using UnityEngine;

namespace Morbius.Scripts.Level
{
    public class CameraSwitcher : MonoBehaviour
    {
        [SerializeField]
        private Camera m_camera;
        [SerializeField]
        private Transform m_target;
        [SerializeField]
        private GameObject m_ignoreRenderObject;

        private float m_transitionFactor;
        private bool m_enabled;
        private Vector3 m_cameraPosition;
        private Quaternion m_cameraRotation;
        private bool m_ignoring;

        private void Awake()
        {
            m_transitionFactor = 0.0f;
            m_enabled = false;
            m_ignoring = false;
        }

        private void LateUpdate()
        {
            if (m_camera != null && m_target != null)
            {
                if (m_enabled)
                {
                    m_transitionFactor += Time.deltaTime;
                    if (m_transitionFactor >= 1.0f)
                        IgnoreRender(true);
                }
                else
                {
                    m_transitionFactor -= Time.deltaTime;
                    IgnoreRender(false);
                }
                m_transitionFactor = Mathf.Clamp01(m_transitionFactor);

                if (m_transitionFactor > 0.0f)
                {
                    m_camera.transform.position = Vector3.Lerp(m_cameraPosition, m_target.position, m_transitionFactor);
                    m_camera.transform.rotation = Quaternion.Slerp(m_cameraRotation, m_target.rotation, m_transitionFactor);
                }
            }

        }

        private void IgnoreRender(bool ignore)
        {
            if (m_ignoreRenderObject != null && ignore != m_ignoring)
            {
                m_ignoring = ignore;
                Renderer[] renderers = m_ignoreRenderObject.GetComponentsInChildren<Renderer>();
                foreach (Renderer renderer in renderers)
                {
                    renderer.enabled = !ignore;
                }

            }
        }

        public void SetFixedCamera(bool enabled)
        {
            m_enabled = enabled;
            if (enabled)
            {
                //store camera properties as camera position is not always updated...
                m_cameraPosition = m_camera.transform.position;
                m_cameraRotation = m_camera.transform.rotation;
            }
        }
    }
}
