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
    [ExecuteInEditMode]
    public class CameraPath : MonoBehaviour
    {
        [SerializeField]
        private Transform m_camera;
        [SerializeField]
        private Transform m_player;
        [SerializeField]
        private int m_steps = 5;

        private Vector3[] m_points;
        private int[] m_pointindex;
        private Vector3[] m_positions;

        private void Start()
        {
            Setup();
        }

        private void Update()
        {
            {
                if (m_camera && m_player)
                {
                    Vector3 target = GetNearestPosition(m_player.position);
                    target -= m_camera.position;
                    m_camera.Translate(target * Time.deltaTime, Space.World);
                }
            }
        }

#if UNITY_EDITOR
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
            m_points = new Vector3[size];
            m_pointindex = new int[2 * (size - 1)];
            for (int i = 0; i < size; i++)
            {
                m_points[i] = transform.GetChild(i).position;
            }

            m_positions = new Vector3[(size - 1) * m_steps];
            for (int i = 1; i < m_points.Length; i++)
            {
                for (int j = 0; j < m_steps; j++)
                {
                    float fac = Convert.ToSingle(j) / Convert.ToSingle(m_steps);
                    int index = j + ((i - 1) * m_steps);
                    m_positions[index] = Vector3.Lerp(m_points[i - 1], m_points[i], fac);
                }
            }
        }

#if UNITY_EDITOR
        private void DrawLines()
        {
            for (int i = 1; i < m_points.Length; i++)
            {
                int i0 = (2 * i) - 2;
                int i1 = (2 * i) - 1;
                m_pointindex[i0] = i - 1;
                m_pointindex[i1] = i;
            }

            Handles.color = Color.yellow;
            Handles.DrawLines(m_points, m_pointindex);
            Gizmos.color = Color.yellow;
            foreach (Vector3 point in m_positions)
            {
                Gizmos.DrawSphere(point, 0.05f);
            }
            foreach (Vector3 point in m_points)
            {
                Gizmos.DrawSphere(point, 0.2f);
            }
        }
#endif

        private Vector3 GetNearestPosition(Vector3 target)
        {
            Vector3 nearestPos = Vector3.zero;
            float nearestSqrDist = float.MaxValue;

            foreach (Vector3 position in m_positions)
            {
                float dist = Vector3.SqrMagnitude(target - position);
                if (dist  < nearestSqrDist)
                {
                    nearestSqrDist = dist;
                    nearestPos = position;
                }
            }

            return nearestPos;
        }

    }
}
