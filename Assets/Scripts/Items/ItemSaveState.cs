using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Morbius.Scripts.Items
{
    public class ItemSaveState
    {
        private bool m_removed;
        private bool m_collected;
        private bool m_spawned;
        private int m_sequenceIndex;
        private Item m_morphItem;

        public ItemSaveState()
        {
            m_removed = false;
            m_collected = false;
            m_spawned = false;
            m_sequenceIndex = 0;
            m_morphItem = null;
        }

        public bool Destroyed
        {
            get
            {
                return m_removed;
            }

            set
            {
                m_removed = value;
            }
        }

        public bool Collected
        {
            get
            {
                return m_collected;
            }

            set
            {
                m_collected = value;
            }
        }

        public bool Spawned
        {
            get
            {
                return m_spawned;
            }

            set
            {
                m_spawned = value;
            }
        }

        public int SequenceIndex
        {
            get
            {
                return m_sequenceIndex;
            }

            set
            {
                m_sequenceIndex = value;
            }
        }

        public Item MorphItem
        {
            get
            {
                return m_morphItem;
            }

            set
            {
                m_morphItem = value;
            }
        }

    }
}
