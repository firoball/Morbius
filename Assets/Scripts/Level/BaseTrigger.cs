using UnityEngine;
using Morbius.Scripts.Game;
using Morbius.Scripts.Movement;

namespace Morbius.Scripts.Level
{
    public abstract class BaseTrigger : MonoBehaviour, IPlayerEnterEventTarget, IPlayerClickEventTarget
    {
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
            if(m_autoPlay && UpdateStatus())
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

        public void OnPlayerEnter()
        {
            if (m_onEnter && UpdateStatus())
            {
                Entered();
            }
        }

        public void OnPlayerClick()
        {
            if (m_onClick && UpdateStatus())
            {
                Clicked();
            }
        }

        protected virtual void Entered()
        {
            Debug.LogWarning("PlayerEntered() not implemented.");
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
