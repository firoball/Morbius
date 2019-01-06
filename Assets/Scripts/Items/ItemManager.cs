using UnityEngine;
using UnityEngine.EventSystems;
using Morbius.Scripts.Ambient;
using Morbius.Scripts.Items;
using Morbius.Scripts.UI;

//[RequireComponent(typeof(AudioSource))]
public class ItemManager : MonoBehaviour
{
    private static ItemManager s_singleton;

    [SerializeField]
    GameObject[] m_receivers;

    private float m_displayTime;
//    private AudioSource m_audio;

    private const float c_minDisplayTime = 1.5f;


    void Awake()
    {
        if (s_singleton == null)
        {
            s_singleton = this;
            //m_audio = GetComponent<AudioSource>();
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

    /*public static void CollectEvent(Item item)
    {
        if (s_singleton)
        {
            s_singleton.m_audio.Play();
            //TODO trigger inventory UI event
        }
    }*/

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
}
