using System;
using System.Collections;
using UnityEngine;
using Morbius.Scripts.Movement;

namespace Morbius.Scripts.Level
{
    [RequireComponent(typeof(ScenePortal))]
    public class TouchPortal : BaseTrigger
    {
        private ScenePortal m_portal;

        private void Start()
        {
            m_portal = GetComponent<ScenePortal>();
        }

        protected override void Entered()
        {
            m_portal.Load();
        }
    }
}
