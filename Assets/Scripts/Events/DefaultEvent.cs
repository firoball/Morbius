using System.Collections;
using UnityEngine;
using Morbius.Scripts.Game;

namespace Morbius.Scripts.Events
{
    public abstract class DefaultEvent : MonoBehaviour
    {
        [SerializeField]
        private int m_minId;
        [SerializeField]
        private int m_maxId;
        [SerializeField]
        private bool m_singleEvent = false;

        public int MinId
        {
            get
            {
                return m_minId;
            }

            set
            {
                m_minId = value;
            }
        }

        public int MaxId
        {
            get
            {
                return m_maxId;
            }

            set
            {
                m_maxId = value;
            }
        }

        public virtual IEnumerator Execute(int id)
        {
            Debug.LogWarning(this + ": Execute not implemented.");
            yield return new WaitForEndOfFrame();
        }

        public bool AllowExecution()
        {
            bool retval = true;

            if(m_singleEvent)
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

    }

}
