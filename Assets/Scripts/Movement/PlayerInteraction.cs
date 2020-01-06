using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Morbius.Scripts.Movement
{

    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField]
        private PlayerNavigator m_navigator;

        [SerializeField]
        private Collider m_target;
        [SerializeField]
        private Collider m_nearCollider;
        private float m_pressedTime;
        private Vector3 m_point;

        void Start()
        {
            m_target = null;
            m_nearCollider = null;
            m_pressedTime = 0.0f;
            m_point = Vector3.zero;
        }

        void Update()
        {
            //TODO: add touch support
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;

                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 10000))
                {
                    m_navigator?.SetDestination(hit.point);
                    if (hit.transform != null)
                    {
                        m_target = GetTargetCollider(hit.transform.gameObject);
                        /* trigger click event in case player is still near to target. 
                         * Otherwise player would have to move out of trigger range again first
                         */
                        if (m_target == m_nearCollider && m_target != null)
                        {
                            m_navigator?.Stop();
                            ExecuteEvents.Execute<IPlayerClickEventTarget>(m_target.gameObject, null, (x, y) => x.OnPlayerClick());
                        }
                    }
                    else
                    {
                        m_target = null;
                    }
                    m_point = hit.point;
                }
            }

            bool isRunning = false;

            if (Input.GetMouseButton(0))
            {
                if (m_pressedTime >= 0.3f)
                {
                    isRunning = true;
                }
                else
                {
                    m_pressedTime += Time.deltaTime;
                }
            }
            else
            {
                m_pressedTime = 0.0f;
            }

            m_navigator?.SetRunning(isRunning);
        }

        private Collider GetTargetCollider(GameObject target)
        {
            Collider collider;

            //any interactive object requires to have a trigger collider
            GameObject root = target.transform.root.gameObject;
            Collider[] colliders = root.GetComponents<Collider>();
            collider = colliders.Where(x => x.isTrigger).FirstOrDefault();

            return collider;
        }

        private void OnTriggerExit(Collider other)
        {
            // make sure exit fires only for collider which is marked as near for player
            GameObject root = other.transform.root.gameObject;

            Collider collider = root.GetComponents<Collider>().Where(x => x.isTrigger).FirstOrDefault();
            if (m_nearCollider != null && collider == m_nearCollider)
            {
                m_nearCollider = null;
                ExecuteEvents.Execute<IPlayerExitEventTarget>(root, null, (x, y) => x.OnPlayerExit());
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            /* parameter is first collider of gameobject, not the one which actually triggered.
             * pick correct collider instead. Awesome, Unity!
             * in case no trigger collider is found, keep last one
             */
            GameObject root = other.transform.root.gameObject;
            Collider collider = root.GetComponents<Collider>().Where(x => x.isTrigger).FirstOrDefault();
            if (collider != null)
            {
                m_nearCollider = collider;
            }

            if (m_target != null && other == m_target)
            {
                m_navigator?.Stop();
                ExecuteEvents.Execute<IPlayerClickEventTarget>(root, null, (x, y) => x.OnPlayerClick());
            }
            ExecuteEvents.Execute<IPlayerEnterEventTarget>(root, null, (x, y) => x.OnPlayerEnter());
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(m_point, 0.1f);
            Handles.Label(m_point, "Target");
        }
#endif

    }
}