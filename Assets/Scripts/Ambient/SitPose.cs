using UnityEngine;
using Morbius.Scripts.Messages;

namespace Morbius.Scripts.Ambient
{
    public class SitPose : MonoBehaviour, ISitMessage
    {

        [SerializeField]
        private Transform m_leftLegBone;
        [SerializeField]
        private Transform m_rightLegBone;
        [SerializeField]
        private bool m_always = false;

        private bool m_sitting;
        private Vector3 m_targetPosition;
        private Quaternion m_targetRotation;

        private const float c_sitAngle = -80.0f;

        private void Awake()
        {
            if (!m_always)
            {
                MessageSystem.Register<ISitMessage>(gameObject);
                m_sitting = false;
            }
            else
            {
                m_targetPosition = transform.position;
                m_targetRotation = transform.rotation;
                m_sitting = true;
            }
        }

        private void LateUpdate()
        {
            if (m_sitting && m_leftLegBone && m_rightLegBone)
            {
                Vector3 euler = new Vector3(0, 0, c_sitAngle);
                m_leftLegBone.eulerAngles = m_leftLegBone.TransformDirection(euler);
                m_rightLegBone.eulerAngles = m_rightLegBone.TransformDirection(euler);

                transform.position = m_targetPosition;
                transform.rotation = m_targetRotation;
            }
        }

        public void OnSit(Transform target)
        {
            m_targetPosition = target.position;
            m_targetRotation = target.rotation;
            m_sitting = true;
        }

        public void OnStand(Transform target)
        {
            transform.position = target.position;
            transform.rotation = target.rotation;
            m_sitting = false;
        }
    }
}
