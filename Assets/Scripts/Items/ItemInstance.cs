using System.Linq;
using UnityEngine;
using Morbius.Scripts.Events;
using Morbius.Scripts.Movement;

namespace Morbius.Scripts.Items
{
    public class ItemInstance : MonoBehaviour, IPlayerClickEventTarget
    {
        [SerializeField]
        protected Item m_item;
        [SerializeField]
        private bool m_spawnOnAwake = true;

        private Material[] m_materials;
        private bool m_destroy;
        private bool m_morphed;
        private bool m_spawned;
        protected ItemSaveState m_status;
        private bool m_readyForCollection;

        public Item Item
        {
            get
            {
                return m_item;
            }

            set
            {
                m_item = value;
            }
        }

        private void Awake()
        {
            SetupMaterials();
            m_destroy = false;
            m_morphed = false;
            m_readyForCollection = false;
        }

        protected virtual void Start()
        {
            if (!m_item)
            {
                Debug.LogWarning("ItemInstance: <" + name + "> has no item assigned!");
                return;
            }

            m_status = ItemDatabase.GetItemStatus(m_item);
            if (m_spawnOnAwake)
            {
                m_status.Spawned = true;
            }
            else
            {
                SetActive(false);
            }
            //UpdateStatus();
        }

        protected virtual void Update()
        {
            if (!m_item)
                return;

            //ItemSaveState may be updated by ItemDatabase, so cyclically poll it
            UpdateStatus();
            UpdateMorph();
            CheckRemove();
        }

        private void SetActive(bool enable)
        {
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = enable;
            }
            Collider[] colliders = GetComponentsInChildren<Collider>();
            foreach (Collider collider in colliders)
            {
                collider.enabled = enable;
            }
        }

        private void SetupMaterials()
        {
            Renderer[] renderer = GetComponentsInChildren<Renderer>();
            m_materials = renderer.Select(x => x.material).ToArray();
        }

        private void UpdateStatus()
        {
            if (!m_status.Spawned)
                return;

            //initial morph update should only be done once
            if (!m_spawned)
            {
                SetActive(true);
                m_spawned = true;
                UpdateMorphInitial();
            }

            //remove myself if necessary
            if (!m_destroy)
                m_destroy = m_status.Destroyed || m_status.Collected;

            UpdateMaterial();
        }

        private void UpdateMaterial()
        {
            if (m_item.IsReadyForCollection(m_status.SequenceIndex) && !m_readyForCollection)
            {
                m_readyForCollection = true;

                foreach (Material mat in m_materials)
                {
                    mat.SetFloat("_HighLightFac", 1.0f);
                }
            }
        }

        private void UpdateMorphInitial()
        {
            //iterate through all morph activities and only spawn last object, if required
            Item morphItem = m_item;
            ItemSaveState morphState = m_status;

            //find last morph state
            while (morphState != null && morphState.MorphItem != null)
            {
                morphItem = morphState.MorphItem;
                morphState = ItemDatabase.GetItemStatus(morphItem);
            }

            //morphstate was found
            if (morphItem != m_item)
            {
                //not yet registered or is active - spawn
                if (morphState == null || !(morphState.Destroyed || morphState.Collected))
                {
                    Debug.Log("UpdateMorphInitial() " + name);
                    morphItem.Spawn(transform, gameObject);
                    m_destroy = true;
                }
                m_morphed = true;
            }
        }

        private void UpdateMorph()
        {
            //if Morphitem was set externally, a morph event was issued. destroy item instance
            if (m_status.MorphItem != null && !m_morphed)
            {
                Debug.Log("UpdateMorph() " + name + m_status.MorphItem.Id);
                m_status.MorphItem.Spawn(transform, gameObject);
                m_destroy = true;
            }
        }

        private void CheckRemove()
        {
            if (m_destroy)
            {
                Destroy(gameObject);
            }
        }

        public void OnPlayerClick()
        {
            if (m_item)
            {
                EventManager.RaiseEvent(m_item.Id);
                UpdateMaterial();
            }
        }
    }
}
