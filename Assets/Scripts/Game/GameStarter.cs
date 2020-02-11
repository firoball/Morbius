using UnityEngine;
using Morbius.Scripts.Items;

namespace Morbius.Scripts.Game
{
    public class GameStarter : MonoBehaviour
    {

        private void Awake()
        {
            GameStatus.Initialize();
        }

        private void Start()
        {
            Inventory.Initialize();
            ItemDatabase.Initialize();
        }

    }
}
