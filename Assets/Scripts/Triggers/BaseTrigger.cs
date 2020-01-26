using System.Collections;
using UnityEngine;
using Morbius.Scripts.Game;
using Morbius.Scripts.Items;
using Morbius.Scripts.Messages;
using Morbius.Scripts.Movement;

namespace Morbius.Scripts.Triggers
{
    public abstract class BaseTrigger : MonoBehaviour, IPlayerEnterEventTarget, IPlayerClickEventTarget, IUnlockTriggerMessage
    {
        [SerializeField]
        private Item m_requiredItem;
        [SerializeField]
        private bool m_onEnter = false;
        [SerializeField]
        private bool m_onClick = false;
        [SerializeField]
        private bool m_autoPlay = true;
        [SerializeField]
        private float m_autoStartDelay = 1.0f;
        [SerializeField]
        private bool m_isLocked = false;
        [SerializeField]
        private bool m_singleEvent = true;
        [SerializeField]
        private bool m_global = false;

        private void Start()
        {
            MessageSystem.Register<IUnlockTriggerMessage>(gameObject);
            if (m_autoPlay && UpdateStatus())
            {
                StartCoroutine(DelayedAutoPlay());
            }
        }

        private bool UpdateStatus()
        {
            bool retval = true;

            if (m_singleEvent)
            {
                if (!GameStatus.IsSet(name, m_global))
                {
                    GameStatus.Set(name, m_global);
                }
                else
                {
                    retval = false;
                }
            }
            return retval;
        }

        private bool CheckAccess()
        {
            ItemSaveState status = ItemDatabase.GetItemStatus(m_requiredItem);
            bool itemAccess = (status == null || status.Collected);
            return (itemAccess && !m_isLocked);
        }

        private IEnumerator DelayedAutoPlay()
        {
            yield return new WaitForSeconds(m_autoStartDelay);
            AutoPlay();
            yield return new WaitForEndOfFrame();
        }

        public void OnPlayerEnter()
        {
            if (m_onEnter && CheckAccess() && UpdateStatus())
            {
                Entered();
            }
        }

        public void OnPlayerClick()
        {
            if (m_onClick && CheckAccess() && UpdateStatus())
            {
                Clicked();
            }
        }

        public void OnUnlock()
        {
            m_isLocked = false;
        }

        public void OnLock()
        {
            m_isLocked = true;
        }

        protected virtual void Entered()
        {
            Debug.LogWarning("Entered() not implemented.");
        }

        protected virtual void Clicked()
        {
            Debug.LogWarning("Clicked() not implemented.");
        }

        protected virtual void AutoPlay()
        {
            Debug.LogWarning("AutoPlay() not implemented.");
        }

    }
}
