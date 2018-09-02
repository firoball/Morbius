using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Morbius.Scripts.Items
{
    [Serializable]
    public class ItemSequence
    {
        [SerializeField]
        private int m_triggerId;
        [SerializeField]
        private AudioClip m_audio;
        [SerializeField]
        private string m_description;

        public string Description
        {
            get
            {
                return m_description;
            }

            set
            {
                m_description = value;
            }
        }

        public AudioClip Audio
        {
            get
            {
                return m_audio;
            }

            set
            {
                m_audio = value;
            }
        }

        public int TriggerId
        {
            get
            {
                return m_triggerId;
            }

            set
            {
                m_triggerId = value;
            }
        }
    }
}
