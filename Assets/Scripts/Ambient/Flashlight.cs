using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Morbius.Scripts.Items;
using Morbius.Scripts.Messages;

namespace Morbius.Scripts.Ambient
{
    [RequireComponent(typeof(Renderer))]
    public class Flashlight : MonoBehaviour, IDarkAreaMessage
    {
        [SerializeField]
        private GameObject m_armBone;
        [SerializeField]
        private Light m_spotLight;
        [SerializeField]
        private Item m_flashlightItem;

        private bool m_active;
        private float m_tilt;
        private ItemSaveState m_state;
        private Renderer m_renderer;

        private void Awake()
        {
            MessageSystem.Register<IDarkAreaMessage>(gameObject);
            m_renderer = GetComponent<Renderer>();
            m_renderer.enabled = false;
        }

        private void Start()
        {
            if (m_armBone != null)
            {
                m_tilt = m_armBone.transform.rotation.eulerAngles.z - 20.0f;
            }
            if (m_spotLight != null)
            {
                m_spotLight.enabled = false;
            }
            if (m_flashlightItem != null)
            {
                m_state = ItemDatabase.GetItemStatus(m_flashlightItem);
            }
        }

        private void Update()
        {
            m_renderer.enabled = m_state.Collected;
        }

        private void LateUpdate()
        {
            if (m_armBone != null && m_state.Collected)
            {
                Vector3 angle = m_armBone.transform.rotation.eulerAngles;
                angle.z *= 0.05f;
                angle.z += 20.0f;// m_tilt;
                m_armBone.transform.eulerAngles = angle;
            }
        }

        public void OnDarkAreaEnter()
        {
            if (m_state.Collected && m_spotLight != null)
            {
                m_spotLight.enabled = true;
            }
        }

        public void OnDarkAreaExit()
        {
            if (m_state.Collected && m_spotLight != null)
            {
                m_spotLight.enabled = false;
            }
        }
    }
}
