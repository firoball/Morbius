using UnityEngine;
using Morbius.Scripts.Game;
using Morbius.Scripts.Items;
using Morbius.Scripts.Messages;

namespace Morbius.Scripts.Level
{
    public class LevelStarter : MonoBehaviour
    {
        private void Awake()
        {
            PortalInfo.Identifier = "";
            MessageSystem.Clear();
            Inventory.Setup();
        }

        private void Start()
        {
            //GameStatus requires awake phase to be ready
            GameStatus.Options.Load();
            GameStatus.ApplySettings();
        }

    }
}
