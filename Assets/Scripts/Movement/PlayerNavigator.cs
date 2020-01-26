using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Morbius.Scripts.Movement
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerNavigator : MonoBehaviour
    {
        [SerializeField]
        private float m_walkSpeed = 1.0f;
        [SerializeField]
        private float m_runSpeed = 2.0f;

        private NavMeshAgent m_agent;
        private Animator m_animator;
        private bool m_isRunning;
        private float m_blendFac;
        private Quaternion m_rotation;
        private Vector3 m_destination;
        private bool m_autoPilot;

        private void Awake()
        {
            m_isRunning = false;
            m_blendFac = 0.0f;
            m_autoPilot = false;

            //compensate different scene scaling
            m_walkSpeed *= transform.localScale.y;
            m_runSpeed *= transform.localScale.y;
        }

        private void Start()
        {
            m_agent = GetComponent<NavMeshAgent>();
            m_animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            if (m_animator)
            {
                bool isMoving = (m_agent.velocity.sqrMagnitude > 0.05f);
                m_animator.SetBool("walk", (isMoving && !m_isRunning));
                m_animator.SetBool("run", (isMoving && m_isRunning));
            }
            if (m_agent.isStopped)
            {
                m_autoPilot = false;
                m_blendFac = Mathf.Clamp01(m_blendFac + 3.0f * Time.deltaTime);
                if (m_agent.destination != transform.position)
                {
                    Quaternion rotation = Quaternion.LookRotation(m_agent.destination - transform.position);
                    transform.rotation = Quaternion.Slerp(m_rotation, rotation, m_blendFac);
                }
            }
            else
            {
                m_blendFac = 0.0f;
                m_rotation = transform.rotation;
            }
        }

        private void LateUpdate()
        {
            float rotY = transform.eulerAngles.y;
            transform.eulerAngles = new Vector3(0, rotY, 0);
        }

        public void SetDestination(Vector3 destination, bool autopilot)
        {
            m_autoPilot = autopilot;
            SetDestination(destination);
        }

        public void SetDestination(Vector3 destination)
        {
            m_agent.isStopped = false;
            m_agent.destination = destination;
            m_destination = destination;
        }

        public void SetRunning(bool run)
        {

            if (run)
            {
                m_agent.speed = m_runSpeed;
            }
            else
            {
                m_agent.speed = m_walkSpeed;
            }
            m_isRunning = run;
        }

        public void Stop()
        {
            if (!m_autoPilot)
            {
                m_agent.isStopped = true;
            }
        }

        public bool IsMoving()
        {
            bool reached = (m_agent.remainingDistance != Mathf.Infinity) && (m_agent.remainingDistance < 0.1f);
            return !reached;
        }

    }
}