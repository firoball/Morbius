using UnityEngine;
using System;

namespace Morbius.Scripts.Level
{
    [CreateAssetMenu]
    public class Chapter : ScriptableObject
    {
        [SerializeField]
        private string m_title;
        [SerializeField]
        private string[] m_text = new string[3];

        public string Title
        {
            get
            {
                return m_title;
            }

            set
            {
                m_title = value;
            }
        }

        public string Date
        {
            get
            {
                return m_text[0];
            }

            set
            {
                m_text[0] = value;
            }
        }

        public string City
        {
            get
            {
                return m_text[1];
            }

            set
            {
                m_text[1] = value;
            }
        }

        public string Street
        {
            get
            {
                return m_text[2];
            }

            set
            {
                m_text[2] = value;
            }
        }

        public string[] Text
        {
            get
            {
                return m_text;
            }

            set
            {
                m_text = Text;
            }
        }
    }
}
