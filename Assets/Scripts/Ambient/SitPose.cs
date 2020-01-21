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

        private bool m_sitting;
        private Transform m_originalTransform;
        private Vector3 m_originalPosition;
        private Quaternion m_originalRotation;
        private Vector3 m_targetPosition;
        private Quaternion m_targetRotation;

        private const float c_sitAngle = -80.0f;

        private void Awake()
        {
            MessageSystem.Register<ISitMessage>(gameObject);
            m_sitting = false;
        }

        private void LateUpdate()
        {
            if (m_sitting && m_leftLegBone && m_rightLegBone)
            {
                Vector3 euler = new Vector3(0, 0, c_sitAngle);
                m_leftLegBone.eulerAngles = euler;
                m_rightLegBone.eulerAngles = euler;

                transform.position = m_targetPosition;
                transform.rotation = m_targetRotation;
            }
        }

        public void OnSit(Transform target)
        {
            OnSit();
            m_targetPosition = target.position;
            m_targetRotation = target.rotation;
        }

        public void OnSit()
        {
            m_originalPosition = transform.position;
            m_originalRotation = transform.rotation;
            m_sitting = true;
        }

        public void OnStand()
        {
            transform.position = m_originalPosition;
            transform.rotation = m_originalRotation;
            m_sitting = false;
        }
    }
}
