using UnityEngine;

namespace Morbius.Scripts.Dialog
{
    public class DialogText : DialogElement
    {
        [SerializeField]
        private AudioClip m_clip;
        [SerializeField]
        private string m_speaker;
        [SerializeField]
        private string m_text;
        [SerializeField]
        private DialogElement m_next;

        public AudioClip Clip
        {
            get
            {
                return m_clip;
            }

            set
            {
                m_clip = value;
            }
        }

        public string Speaker
        {
            get
            {
                return m_speaker;
            }

            set
            {
                m_speaker = value;
            }
        }

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

        public override DialogElement Next
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
