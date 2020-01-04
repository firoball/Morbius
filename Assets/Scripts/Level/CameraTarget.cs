using UnityEngine;
using System;

namespace Morbius.Scripts.Level
{
    public class CameraTarget : MonoBehaviour
    {
        [SerializeField]
        private Transform m_target;

        private void Update()
        {
            if (m_target != null)
                transform.LookAt(m_target);
        }
    }
}
