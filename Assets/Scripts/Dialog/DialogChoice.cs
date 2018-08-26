using System;
using UnityEngine;

namespace Morbius.Scripts.Dialog
{
    [Serializable]
    public class DialogChoice
    {
        [SerializeField]
        private string m_text;
        [SerializeField]
        private string m_result;
        [SerializeField]
        private DialogElement m_next;

        public string Text
        {
            get
            {
                return m_text;
            }

            set
            {
                m_text = value;
            }
        }

        public string Result
        {
            get
            {
                return m_result;
            }

            set
            {
                m_result = value;
            }
        }

        public DialogElement Next
        {
            get
            {
                return m_next;
            }

            set
            {
                m_next = value;
            }
        }

    }
}
