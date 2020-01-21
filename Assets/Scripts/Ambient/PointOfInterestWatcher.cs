using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Morbius.Scripts.Ambient
{
    public class PointOfInterestWatcher : MonoBehaviour
    {

        static List<Transform> s_pointOfInterests;

        [SerializeField]
        private Transform m_rootBone;
        [SerializeField]
        private Transform m_neckBone;
        [SerializeField]
        private float m_watchDistance = 5.0f;

        private Transform m_lookAt;
        private Quaternion m_lastRotation;
        private Quaternion m_lookAtRotation;
        private float m_blendFactor;

        private void Awake()
        {
            if (s_pointOfInterests == null)
            {
                s_pointOfInterests = new List<Transform>();
            }
            if (m_neckBone)
            {
                m_lastRotation = m_neckBone.rotation;
            }
            m_blendFactor = 0.0f;
            m_lookAtRotation = Quaternion.identity;
            m_lastRotation = Quaternion.identity;
        }

        private void OnDestroy()
        {
            if (s_pointOfInterests != null && s_pointOfInterests.Count > 0)
            {
                s_pointOfInterests.Clear();
            }
        }

        private void Update()
        {
            //sort POIs by distance
            m_lookAt = s_pointOfInterests.OrderBy(x => Vector3.SqrMagnitude(x.transform.position - transform.position)).FirstOrDefault();
            //is closest POI close enough?
            if (m_lookAt != null && Vector3.SqrMagnitude(m_lookAt.position - transform.position) > m_watchDistance * m_watchDistance)
            {
                m_lookAt = null;
            }
        }

        private void LateUpdate()
        {
            if (m_neckBone && m_rootBone)
            {
                Quaternion neutralRotation = m_neckBone.rotation;
                if (m_lookAt)
                {
                    //look at target
                    Quaternion lookDirection = Quaternion.LookRotation(m_lookAt.position - m_neckBone.position);
                    m_neckBone.rotation = Quaternion.Slerp(m_lastRotation, lookDirection, Mathf.Clamp01(5.0f * Time.deltaTime));
                    m_lastRotation = m_neckBone.rotation;

                    //correct parent rotation
                    m_neckBone.Rotate(m_rootBone.localEulerAngles);

                    //limit head rotation
                    float limiterY = m_neckBone.localEulerAngles.y;
                    float limiterZ = m_neckBone.localEulerAngles.z;
                    while (limiterY >= 180.0f) limiterY -= 360.0f;
                    while (limiterY < -180.0f) limiterY += 360.0f;
                    if (Mathf.Abs(limiterY) > 70.0f || Mathf.Abs(limiterZ) > 55.0f)
                    {
                        //head rotation limits reached - reduce blend factor
                        m_blendFactor = Mathf.Clamp01(m_blendFactor - 3.0f * Time.deltaTime);
                    }
                    else
                    {
                        //looking at object - increase blend factor
                        m_blendFactor = Mathf.Clamp01(m_blendFactor + 3.0f * Time.deltaTime);
                    }

                    m_lookAtRotation.eulerAngles = m_neckBone.eulerAngles;
                }
                else
                {
                    //no target to look at - reduce blend factor
                    m_blendFactor = Mathf.Clamp01(m_blendFactor - 3.0f * Time.deltaTime);
                }
                m_neckBone.rotation = Quaternion.Slerp(neutralRotation, m_lookAtRotation, m_blendFactor);
            }
        }



        public static void Register (Transform poi)
        {
            if (s_pointOfInterests != null && !s_pointOfInterests.Find(x => x == poi))
            {
                s_pointOfInterests.Add(poi);
            }
        }

        public static void UnRegister(Transform poi)
        {
            if (s_pointOfInterests!= null)
            {
                s_pointOfInterests.Remove(poi);
            }
        }
    }
}
