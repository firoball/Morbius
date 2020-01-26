using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Morbius.Scripts.Level
{
    public class CameraIntroPath : MonoBehaviour
    {
        [SerializeField]
        private Transform m_camera;
        [SerializeField]
        private float m_speed = 4.0f;

        private Vector3[] m_points;
        private int m_step;
        private float m_currentDistance;
        private float m_segmentDistance;
#if UNITY_EDITOR
        private int[] m_pointindex;
#endif

        private void Start()
        {
            Setup();
            m_step = 0;
            m_currentDistance = 0.0f;
            if (m_speed > 0.0f && m_points.Length >= 2)
            {
                m_segmentDistance = Vector3.Distance(m_points[m_step], m_points[m_step + 1]);
            }
        }

        private void Update()
        {
            if (m_camera && m_points.Length >= 2)
            {
                m_currentDistance += Time.deltaTime * m_speed;
                float factor = m_currentDistance / m_segmentDistance;
                m_camera.position = Vector3.Lerp(m_points[m_step], m_points[m_step + 1], Mathf.Clamp01(factor));
                if (factor >= 1.0f)
                {
                    if (m_step < m_points.Length - 2)
                    {
                        m_currentDistance -= m_segmentDistance;
                        m_step++;
                        m_segmentDistance = Vector3.Distance(m_points[m_step], m_points[m_step + 1]);
                    }
                    else
                    {
                        m_camera.position = m_points[m_step + 1];
                        enabled = false;
                    }
                }

            }
        }

#if UNITY_EDITOR
        [ExecuteInEditMode]
        private void OnDrawGizmos()
        {
            if (Application.isEditor)
            {
                Setup();
                DrawLines();
            }
        }
#endif

        private void Setup()
        {
            int size = transform.childCount;
            if (size == 0) return;

            m_points = new Vector3[size];
            for (int i = 0; i < size; i++)
            {
                m_points[i] = transform.GetChild(i).position;
            }

#if UNITY_EDITOR
            m_pointindex = new int[2 * (size - 1)];
            for (int i = 1; i < m_points.Length; i++)
            {
                int i0 = (2 * i) - 2;
                int i1 = (2 * i) - 1;
                m_pointindex[i0] = i - 1;
                m_pointindex[i1] = i;
            }
#endif
        }

#if UNITY_EDITOR
        private void DrawLines()
        {
            Handles.color = Color.yellow;
            Handles.DrawLines(m_points, m_pointindex);
            Gizmos.color = Color.yellow;
            foreach (Vector3 point in m_points)
            {
                Gizmos.DrawSphere(point, 0.2f);
            }
        }
#endif

    }
}
