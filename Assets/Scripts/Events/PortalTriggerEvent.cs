using System;
using System.Collections;
using UnityEngine;
using Morbius.Scripts.Game;
using Morbius.Scripts.Items;
using Morbius.Scripts.Level;
using Morbius.Scripts.Messages;

namespace Morbius.Scripts.Events
{
    [RequireComponent(typeof(ScenePortal))]
    public class PortalTriggerEvent : DefaultEvent, IUnlockTriggerMessage
    {
        [SerializeField]
        protected bool m_isLocked;

        private ScenePortal m_scenePortal;

        protected void Awake()
        {
            MessageSystem.Register<IUnlockTriggerMessage>(gameObject);
            m_scenePortal = GetComponent<ScenePortal>();
            if (GameStatus.IsSet(name, false))
            {
                m_isLocked = false;
            }
        }

        public void OnUnlock()
        {
            m_isLocked = false;
            if (!GameStatus.IsSet(name, false))
            {
                GameStatus.Set(name, false);
            }
        }

        public void OnLock()
        {
            m_isLocked = true;
            GameStatus.Unset(name, false);
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
