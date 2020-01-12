using UnityEngine;
using Morbius.Scripts.Game;
using Morbius.Scripts.Items;
using Morbius.Scripts.Movement;

namespace Morbius.Scripts.Level
{
    public abstract class BaseTrigger : MonoBehaviour, IPlayerEnterEventTarget, IPlayerClickEventTarget
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
        private bool m_singleEvent = true;

        private void Start()
        {
            if (m_autoPlay && UpdateStatus())
            {
                AutoPlay();
            }
        }

        private bool UpdateStatus()
        {
            bool retval = true;

            if (m_singleEvent)
            {
                if (!GameStatus.IsSet(name))
                {
                    GameStatus.Set(name);
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
            return (status == null || status.Collected);
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
