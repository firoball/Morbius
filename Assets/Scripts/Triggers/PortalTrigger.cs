﻿using System;
using System.Collections;
using UnityEngine;
using Morbius.Scripts.Level;

namespace Morbius.Scripts.Triggers
{
    [RequireComponent(typeof(ScenePortal))]
    public class PortalTrigger : BaseTrigger
    {
        private ScenePortal m_portal;

        [SerializeField]
        private string m_identifier;

        private void Start()
        {
            m_portal = GetComponent<ScenePortal>();
        }

        protected override void Clicked()
        {
            PortalInfo.Identifier = m_identifier;
            m_portal.Load();
        }

        protected override void Entered()
        {
            PortalInfo.Identifier = m_identifier;
            m_portal.Load();
        }
    }
}
