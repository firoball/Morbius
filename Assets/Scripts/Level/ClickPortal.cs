using System;
using System.Collections;
using UnityEngine;
using Morbius.Scripts.Events;
using Morbius.Scripts.Movement;

namespace Morbius.Scripts.Level
{
    [RequireComponent(typeof(ScenePortal))]
    public class ClickPortal : MonoBehaviour, IPlayerClickEventTarget
    {
        private ScenePortal m_portal;

        [SerializeField]
        private string m_identifier;

        private void Start()
        {
            m_portal = GetComponent<ScenePortal>();
        }

        public void OnPlayerClick()
        {
            PortalInfo.Identifier = m_identifier;
            m_portal.Load();
        }
    }
}
