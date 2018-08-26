using UnityEngine;
using System.Collections.Generic;

namespace Morbius.Scripts.Dialog
{
    public class Dialog : MonoBehaviour
    {

        [SerializeField]
        private DialogElement m_head;

        private DialogElement m_currentElement;

        public DialogElement CurrentElement
        {
            get
            {
                return m_currentElement;
            }
        }

        public DialogElement Head
        {
            get
            {
                return m_head;
            }

            set
            {
                m_head = value;
            }
        }

        private void Awake()
        {
            Restart();
        }

        public void Restart()
        {
            m_currentElement = m_head;
        }

        public bool IsFinished()
        {
            return m_currentElement == null;
        }

        public void Proceed()
        {
            if (m_currentElement)
            {
                m_currentElement = m_currentElement.Next;
            }
        }
    }
}
