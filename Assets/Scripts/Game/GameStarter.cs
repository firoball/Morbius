using UnityEngine;
using System.Collections;
using Morbius.Scripts.Items;
using Morbius.Scripts.Level;
using Morbius.Scripts.UI;

namespace Morbius.Scripts.Game
{
    [RequireComponent(typeof(ScenePortal))]
    public class GameStarter : MonoBehaviour
    {

        [SerializeField]
        private UIFader m_fader;

        private ScenePortal m_portal;

        private void Awake()
        {
            m_portal = GetComponent<ScenePortal>();
        }

        // Use this for initialization
        void Start()
        {
            GameStatus.Initialize();
            Inventory.Initialize();
            ItemDatabase.Initialize();
        }

        // Update is called once per frame
        void Update()
        {
            //quick hack
            if (Input.anyKeyDown)
            {
                m_fader?.Hide(false);
                m_portal.Load();
            }
        }
    }
}
