using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Morbius.Scripts.Messages;

namespace Morbius.Scripts.Movement
{

    public class PlayerInteraction : MonoBehaviour, ICursorUIMessage
    {
        [SerializeField]
        private PlayerNavigator m_navigator;

        private Collider m_target;
        private List<Collider> m_nearColliders;
        private float m_pressedTime;
        private Vector3 m_point;
        [SerializeField]
        private bool m_enabled;

        private void Awake()
        {
            m_target = null;
            m_nearColliders = new List<Collider>();
            m_pressedTime = 0.0f;
            m_point = Vector3.zero;
            m_enabled = true;
            MessageSystem.Register<ICursorUIMessage>(gameObject);

        }

        private void Update()
        {
            if (m_enabled)
            {
                //TODO: add touch support
                //if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
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
                            if (m_target != null && m_nearColliders.Contains(m_target))
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
            }
            else
            {
                m_navigator?.Stop();
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

            //cleanup of collider log (collectable items may get removed while player is near)
            m_nearColliders.RemoveAll(x => x == null);
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
            if (collider != null && m_nearColliders.Contains(collider))
            {
                m_nearColliders.Remove(collider);
            }
            ExecuteEvents.Execute<IPlayerExitEventTarget>(root, null, (x, y) => x.OnPlayerExit());
        }

        private void OnTriggerEnter(Collider other)
        {
            /* parameter is first collider of gameobject, not the one which actually triggered.
             * pick correct collider instead. Awesome, Unity!
             * in case no trigger collider is found, keep last one
             */
            GameObject root = other.transform.root.gameObject;
            Collider collider = root.GetComponents<Collider>().Where(x => x.isTrigger).FirstOrDefault();
            if (collider != null && !m_nearColliders.Contains(collider))
            {
                m_nearColliders.Add(collider);
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

        public void OnUIEnter()
        {
            m_enabled = false;
        }

        public void OnUIExit()
        {
            m_enabled = true;
        }

    }
}