using UnityEngine;
using UnityEngine.SceneManagement;
using Morbius.Scripts.Game;
using Morbius.Scripts.Messages;

namespace Morbius.Scripts.Movement
{

    public class PlayerSaveState : MonoBehaviour, ISaveStatusMessage
    {

        private void Awake()
        {
            LoadPlayerData();
            MessageSystem.Register<ISaveStatusMessage>(gameObject);
        }

        private void LoadPlayerData()
        {
            if (GameStatus.Data.Scene == SceneManager.GetActiveScene().name)
            {
                transform.position = GameStatus.Data.PlayerPosition;
                Vector3 euler = transform.eulerAngles;
                euler.y = GameStatus.Data.PlayerYRotation;
                transform.eulerAngles = euler;

            }
        }

        public void OnSave()
        {
            GameStatus.Data.PlayerPosition = transform.position;
            GameStatus.Data.PlayerYRotation = transform.eulerAngles.y;
        }

    }
}