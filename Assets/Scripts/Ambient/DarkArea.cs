using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Morbius.Scripts.Items;
using Morbius.Scripts.Messages;

namespace Morbius.Scripts.Ambient
{
    [RequireComponent(typeof(NavMeshObstacle))]
    [RequireComponent(typeof(Collider))]
    public class DarkArea : MonoBehaviour
    {
        [SerializeField]
        private Item m_flashlightItem;
        [SerializeField]
        private GameObject m_player;

        private bool m_active;
        private bool m_entered;
        private ItemSaveState m_state;
        private NavMeshObstacle m_obstacle;
        private Collider m_collider;

        private void Awake()
        {
            m_active = false;
            m_entered = false;
            m_obstacle = GetComponent<NavMeshObstacle>();
            m_collider = GetComponent<Collider>();
        }

        private void Start()
        {
            if (m_flashlightItem != null)
            {
                m_state = ItemDatabase.GetItemStatus(m_flashlightItem);
            }
        }

        private void Update()
        {
            CheckActive();
            CheckPlayer();
        }

        private void CheckActive()
        {
            if (!m_active && m_state != null && m_state.Collected)
            {
                m_active = true;
                m_obstacle.enabled = false;
            }
            else if (m_active && m_state != null && !m_state.Collected)
            {
                m_active = false;
                m_obstacle.enabled = true;
            }
        }

        private void CheckPlayer()
        {
            if (!m_entered && m_player != null && m_collider.bounds.Contains(m_player.transform.position))
            {
                m_entered = true;
                MessageSystem.Execute<IDarkAreaMessage>((x, y) => x.OnDarkAreaEnter());
            }

            if (m_entered && m_player != null && !m_collider.bounds.Contains(m_player.transform.position))
            {
                m_entered = false;
                MessageSystem.Execute<IDarkAreaMessage>((x, y) => x.OnDarkAreaExit());
            }
        }

    }
}
