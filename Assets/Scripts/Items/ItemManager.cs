using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Morbius.Scripts.Ambient;
using Morbius.Scripts.Items;
using Morbius.Scripts.UI;

[RequireComponent(typeof(AudioSource))]
public class ItemManager : MonoBehaviour
{
    private static ItemManager s_singleton;
    private static Dictionary<Item, ItemSaveState> s_itemStates = new Dictionary<Item, ItemSaveState>();

    [SerializeField]
    GameObject[] m_receivers;

    private float m_displayTime;
    private AudioSource m_audio;

    private const float c_minDisplayTime = 1.5f;


    void Awake()
    {
        if (s_singleton == null)
        {
            s_singleton = this;
            m_audio = GetComponent<AudioSource>();
        }
        else
        {
            Debug.Log("ItemManager: Multiple instances detected. Destroying...");
            Destroy(gameObject);
        }

    }

    private void Update()
    {
        Timer();
    }

    private void Timer()
    {
        //timer is handled in Update instead of Coroutines in order to avoid triggering multiple coroutines
        if (m_displayTime > 0.0f)
        {
            m_displayTime = Mathf.Max(m_displayTime - Time.deltaTime, 0.0f);
            if (m_displayTime <= 0.0f)
            {
                foreach (GameObject target in s_singleton.m_receivers)
                {
                    ExecuteEvents.Execute<IInfoTextEventTarget>(target, null, (x, y) => x.OnHide());
                }
            }
        }
    }

    public static ItemSaveState RegisterItem(Item item)
    {
        ItemSaveState state = new ItemSaveState();
        RegisterItem(item, state);
        return GetItemStatus(item);
    }

    public static void RegisterItem(Item item, ItemSaveState saveState)
    {
        if (!s_itemStates.ContainsKey(item))
        {
            s_itemStates.Add(item, saveState);
        }
    }

    public static ItemSaveState GetItemStatus(Item item)
    {
        ItemSaveState saveState;
        if (s_itemStates.TryGetValue(item, out saveState))
        {
            return saveState;
        }
        else
        {
            return null;
        }
    }

    public static void SetItemStatus(Item item, ItemSaveState saveState)
    {
        if (s_itemStates.ContainsKey(item))
        {
            s_itemStates[item] = saveState;
        }
    }

    public static void SpawnItem(Item item)
    {
        ItemSaveState state = GetItemStatus(item);
        if (state != null)
        {
            state.Spawned = true;
        }
    }

        public static GameObject SpawnItem(Item item, Transform spawnpoint)
    {
        GameObject itemObj = null;
        if (item)
        {
            if (item.Prefab)
            {
                ItemSaveState saveState = new ItemSaveState()
                {
                    Spawned = true
                };
                RegisterItem(item, saveState);
                itemObj = Instantiate(item.Prefab, spawnpoint.position, spawnpoint.rotation);
                ItemInstance instance = itemObj.AddComponent<ItemInstance>();
                instance.Item = item;
            }
            else
            {
                Debug.LogWarning("ItemManager: No prefab configured for " + item.name);
            }
        }

        return itemObj;
    }

    public static bool Combine(Item item1, Item item2)
    {
        //TODO handle combination
        //TODO set .Morphed and .Removed according to result of combination
        return false;
    }

    public static void CollectEvent(Item item)
    {
        if (s_singleton)
        {
            s_singleton.m_audio.Play();
            //TODO trigger inventory UI event
        }
    }

    public static void SequenceEvent(ItemSequence sequence)
    {
        if (s_singleton)
        {
            s_singleton.m_displayTime = Mathf.Max(sequence.Audio.length + 0.5f, c_minDisplayTime);
            foreach (GameObject target in s_singleton.m_receivers)
            {
                ExecuteEvents.Execute<IInfoTextEventTarget>(target, null, (x, y) => x.OnShow(sequence.Description));
            }
            AudioManager.ScheduleVoice(sequence.Audio);
        }
    }

    public static void Clear()
    {
        s_itemStates.Clear();
    }
}
