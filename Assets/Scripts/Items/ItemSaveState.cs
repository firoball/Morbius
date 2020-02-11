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

        public override string ToString()
        {
            string id = m_morphItem? Convert.ToString(m_morphItem.Id) : "0";
            
            return Convert.ToString(Convert.ToByte(m_removed)) + "#" +
                Convert.ToString(Convert.ToByte(m_collected)) + "#" +
                Convert.ToString(Convert.ToByte(m_spawned)) + "#" +
                Convert.ToString(m_sequenceIndex) + "#" +
                id;
        }

        public void ReadFromString(string data)
        {
            string[] elements = data.Split('#');
            if (elements != null && elements.Length == 5)
            { 
                m_removed = Convert.ToBoolean(Convert.ToByte(elements[0]));
                m_collected = Convert.ToBoolean(Convert.ToByte(elements[1]));
                m_spawned = Convert.ToBoolean(Convert.ToByte(elements[2]));
                m_sequenceIndex = Convert.ToInt32(elements[3]);
                int id = Convert.ToInt32(elements[4]);
                m_morphItem = ItemDatabase.GetItemById(id);
            }
        }
    }
}
