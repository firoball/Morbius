﻿using System;
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
            return (sequenceCount >= m_sequences.Count - 1);
        }

        public bool IsReadyForCollection(int sequenceCount)
        {
            return (IsLastSequence(sequenceCount) && m_collectable);
        }

        public ItemSequence GetSequence(int sequenceCount)
        {
            if (m_sequences.Count == 0)
                return null;

            //limit sequence counter so last sequence in list is always repeated 
            sequenceCount = Math.Max(0, Math.Min(sequenceCount, m_sequences.Count - 1));
            return m_sequences[sequenceCount];
        }

        public override string ToString()
        {
            string str = m_label + " (" + m_id + ") ";
            if (m_collectable) str += "[C]";
            if (m_destroyable) str += "[D]";
            return str;
        }

        public GameObject Spawn(Transform spawnpoint)
        {
            GameObject itemObj = null;
            if (m_prefab)
            {
                itemObj = Spawn(spawnpoint, m_prefab);
            }
            else
            {
                Debug.LogWarning("Item: No prefab configured for " + name);
            }

            return itemObj;

        }

        public GameObject Spawn(Transform spawnpoint, GameObject prefab)
        {
            GameObject itemObj = null;
            /* WORKAROUND: 
             * always prefer configured prefab over parameter.
             * morph items do not always carry prefab information in XML if morph is not
             * supposed to alter the model or if no morph will happen at all.
             * This can create cases where the prefab has to be derived from the scene object.
             * ItemInstance class has to take care of providing proper prefab. This function will patch it accordingly. 
             * This is failure by design...
             */
            if (m_prefab != null)
            {
                prefab = m_prefab;
            }

            if (prefab)
            {
                ItemSaveState saveState = new ItemSaveState()
                {
                    Spawned = true
                };
                ItemDatabase.SetItemStatus(this, saveState);
                itemObj = Instantiate(prefab, spawnpoint.position, spawnpoint.rotation);
                itemObj.name = m_label;
                
                //in case prefab was based on scene object... clean it up (narf).
                ItemInstance old = itemObj.GetComponent<ItemInstance>();
                Destroy(old);

                ItemInstance instance = itemObj.AddComponent<ItemInstance>();
                instance.Item = this;
            }
            else
            {
                Debug.LogWarning("Item: Invalid prefab given for " + name);
            }

            return itemObj;
        }


    }
}