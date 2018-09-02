using System.Collections;
using UnityEngine;

namespace Morbius.Scripts.Events
{
    public abstract class DefaultEvent : MonoBehaviour
    {
        [SerializeField]
        private int m_minId;
        [SerializeField]
        private int m_maxId;

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
            yield return new WaitForSeconds(1.0f);
        }
    }

}
