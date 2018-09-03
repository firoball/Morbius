using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Morbius.Scripts.Events;
using Morbius.Scripts.Cursor;

namespace Morbius.Scripts.Items
{
    public class ItemInstance : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private Item m_item;
        [SerializeField]
        private bool m_spawnOnAwake = true;

        private Material[] m_materials;
        private bool m_destroy;
        private bool m_morphed;
        private bool m_spawned;
        private ItemSaveState m_status;
        private bool m_readyForCollection;
        private bool m_hovered;
        private bool m_playerNear;

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
            m_playerNear = false;
            m_readyForCollection = false;
            m_hovered = false;
        }

        private void Start()
        {
            if (!m_item)
                return;

            m_status = ItemManager.RegisterItem(m_item);
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

        private void Update()
        {
            if (!m_item)
                return;

            //ItemSaveState may be updated by ItemManager, so cyclically poll it
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
                m_destroy = m_status.Removed;

            UpdateMaterial();
        }

        private void UpdateMaterial()
        {
            if (m_item.IsReadyForCollection(m_status.SequenceIndex) && !m_readyForCollection)
            {
                m_readyForCollection = true;
                if (m_hovered)
                {
                    CursorManager.UpdateCursorItem(m_item.Label, true, false);
                }

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
                morphState = ItemManager.GetItemStatus(morphItem);
            }

            //morphstate was found
            if (morphItem != m_item)
            {
                //not yet registered or is active - spawn
                if (morphState == null || !morphState.Removed)
                {
                    ItemManager.SpawnItem(morphItem, transform);
                }
                m_morphed = true;
            }
        }

        private void UpdateMorph()
        {
            //if Morphitem was set externally, a morph event was issued. destroy item instance
            //creating the morph item is handled in ItemManager
            if (m_status.MorphItem != null && !m_morphed)
            {
                ItemManager.SpawnItem(m_status.MorphItem, transform);
                m_destroy = true;
            }
        }

        private void CheckRemove()
        {
            if (m_destroy)
            {
                if (m_hovered)
                {
                    CursorManager.SetDefaultCursor();
                }
                Destroy(gameObject);
            }
        }

        private void Collect()
        {
            EventManager.RaiseEvent(m_item.Id);
            m_destroy = true;
        }

        private void ExecuteSequence()
        {
            ItemSequence sequence = m_item.GetSequence(m_status.SequenceIndex);
            ItemManager.SequenceEvent(sequence);
            EventManager.RaiseEvent(sequence.TriggerId);
        }

        private void Interact()
        {
            if (m_status != null)
            {
                if (!m_item.IsLastSequence(m_status.SequenceIndex))
                {
                    ExecuteSequence();
                    m_status.SequenceIndex++;
                    UpdateMaterial();
                }
                else if (m_item.IsReadyForCollection(m_status.SequenceIndex))
                {
                    //TODO add trigger region and only collect if player is in region
                    Collect();
                }
                else
                {
                    //stuck with end state
                    ExecuteSequence();
                }
            }
        }

        private void TriggerAction()
        {
            Item handItem = Inventory.ItemInHand;
            if (handItem != null)
            {
                if (ItemManager.Combine(m_item, handItem))
                {
                    //TODO this may not be required here and can be handled via ItemSAveState in Combine() directly
                    if (m_item.Destroyable)
                    {
                        m_destroy = true;
                    }
                }
            }
            else
            {
                Interact();
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            //TODO: check if collider is the one of player
            m_playerNear = true;
            //TODO: stop player movement

            //TODO check if item is still the selected one
            TriggerAction();
            m_playerNear = false;
        }

        public void OnTriggerExit(Collider other)
        {
            //TODO: check if collider is the one of player
            m_playerNear = false;
        }

        public void OnPointerClick(PointerEventData data)
        {
            
            //TODO: store clicked object with Cursormanager and evaluate
            //evaluation here is required in case player is already in trigger range
            //if (m_playerNear) //temp
            {
                TriggerAction();
                m_playerNear = false;
            }
            //TODO player must be near - add "memory" function for cursormanager
        }

        public void OnPointerEnter(PointerEventData data)
        {
            m_hovered = true;
            bool collectable = m_item.IsReadyForCollection(m_status.SequenceIndex);
            CursorManager.UpdateCursorItem(m_item.Label, collectable, false);
        }

        public void OnPointerExit(PointerEventData data)
        {
            m_hovered = false;
            CursorManager.SetDefaultCursor();
        }
    }
}
