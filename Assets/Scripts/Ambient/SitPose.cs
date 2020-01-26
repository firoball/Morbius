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
