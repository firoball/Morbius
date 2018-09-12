using UnityEngine;

namespace Morbius.Scripts.Items
{
    public class Failure : ScriptableObject
    {
        [SerializeField]
        private AudioClip m_audio;
        [SerializeField]
        private string m_description;

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
    }
}
