using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Morbius.Scripts.Dialog;
using Morbius.Scripts.Items;
using Morbius.Scripts.Level;
using Morbius.Scripts.Messages;

namespace Morbius.Scripts.Events
{
    [RequireComponent(typeof(ScenePortal))]
    public class PortalTriggerEvent : DefaultEvent, IUnlockTriggerMessage
    {
        [SerializeField]
        private bool m_isLocked;

        private ScenePortal m_scenePortal;

        private void Awake()
        {
            MessageSystem.Register<IUnlockTriggerMessage>(gameObject);
            m_scenePortal = GetComponent<ScenePortal>();
        }

        public void OnUnlock()
        {
            m_isLocked = false;
        }

        public void OnLock()
        {
            m_isLocked = true;
        }

        public override IEnumerator Execute(int eventId)
        {
            Inventory.DropHandItem();
            if (!m_isLocked)
            {
                m_scenePortal.Load();
                EventManager.CancelEvents();
            }
            yield return new WaitForEndOfFrame();
        }
    }

}
