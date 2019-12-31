using UnityEngine;

namespace Morbius.Scripts.Items
{
    [CreateAssetMenu]
    public class Combination : ScriptableObject
    {
        [SerializeField]
        private int m_id1;
        [SerializeField]
        private int m_id2;
        [SerializeField]
        private AudioClip m_audio;
        //private Item[] m_combinationItems;
        //private Item m_newItem;
        [SerializeField]
        private int m_morphId;
        [SerializeField]
        private int m_triggerId;
        [SerializeField]
        private string m_description;

        public int Id1
        {
            get
            {
                return m_id1;
            }

            set
            {
                m_id1 = value;
            }
        }

        public int Id2
        {
            get
            {
                return m_id2;
            }

            set
            {
                m_id2 = value;
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

        public int MorphId
        {
            get
            {
                return m_morphId;
            }

            set
            {
                m_morphId = value;
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
