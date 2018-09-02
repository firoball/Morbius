using System;
using System.Collections.Generic;
using UnityEngine;

namespace Morbius.Scripts.Items
{
    [CreateAssetMenu]
    public class Item : ScriptableObject
    {
        [SerializeField]
        private int m_id;
        [SerializeField]
        private string m_label;
        [SerializeField]
        private bool m_collectable;
        [SerializeField]
        private bool m_destroyable;
        [SerializeField]
        private GameObject m_prefab;
        [SerializeField]
        private Sprite m_icon;
        [SerializeField]
        private List<ItemSequence> m_sequences;

        public int Id
        {
            get
            {
                return m_id;
            }

            set
            {
                m_id = value;
            }
        }

        public string Label
        {
            get
            {
                return m_label;
            }

            set
            {
                m_label = value;
            }
        }

        public bool Collectable
        {
            get
            {
                return m_collectable;
            }

            set
            {
                m_collectable = value;
            }
        }

        public bool Destroyable
        {
            get
            {
                return m_destroyable;
            }

            set
            {
                m_destroyable = value;
            }
        }

        public GameObject Prefab
        {
            get
            {
                return m_prefab;
            }

            set
            {
                m_prefab = value;
            }
        }

        public Sprite Icon
        {
            get
            {
                return m_icon;
            }

            set
            {
                m_icon = value;
            }
        }

        public List<ItemSequence> Sequences
        {
            get
            {
                return m_sequences;
            }

            set
            {
                m_sequences = value;
            }
        }

        public bool IsLastSequence(int sequenceCount)
        {
            return (sequenceCount >= m_sequences.Count);
        }

        public bool IsReadyForCollection(int sequenceCount)
        {
            return ((sequenceCount >= m_sequences.Count) && m_collectable);
        }

        public ItemSequence GetSequence(int sequenceCount)
        {
            //limit sequence counter so last sequence in list is always repeated 
            sequenceCount = Math.Max(0, Math.Min(sequenceCount, m_sequences.Count - 1));
            return m_sequences[sequenceCount];
        }
    }
}
