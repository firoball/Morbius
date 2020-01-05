using System;
using System.Collections;
using UnityEngine;
using Morbius.Scripts.Movement;

namespace Morbius.Scripts.Level
{
    [RequireComponent(typeof(ScenePortal))]
    public class TouchPortal : MonoBehaviour, IPlayerEnterEventTarget
    {
        private ScenePortal m_portal;

        private void Start()
        {
            m_portal = GetComponent<ScenePortal>();
        }

        public void OnPlayerEnter()
        {
            m_portal.Load();
        }
    }
}
