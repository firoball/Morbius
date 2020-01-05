using System;
using UnityEngine;

namespace Morbius.Scripts.Level
{
    public class StartPosition : MonoBehaviour
    {
        [SerializeField]
        private string m_identifier;
        [SerializeField]
        private Transform m_player;

        private void Awake()
        {
            if (m_player == null)
            {
                Debug.LogWarning("StartPosition: Player reference not set.");
            }
        }
        private void Start()
        {
            if (!string.IsNullOrEmpty(m_identifier) && m_identifier == PortalInfo.Identifier && m_player != null)
            {
                m_player.position = transform.position;
                m_player.rotation = transform.rotation;
            }
        }

    }
}
