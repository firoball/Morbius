using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Morbius.Scripts.Game
{
    public static class GameStatus
    {
        private static List<string> s_status = new List<string>();
        private static GameData s_data;

        public static GameData Data { get => s_data; }

        public static void Set(string identifier)
        {
            if (!string.IsNullOrWhiteSpace(identifier))
            {
                string value = SceneManager.GetActiveScene().buildIndex.ToString() + "_" + identifier;
                s_status.Add(value);
            }
        }

        public static bool IsSet(string identifier)
        {
            bool result = false;
            if (!string.IsNullOrWhiteSpace(identifier))
            {
                string value = SceneManager.GetActiveScene().buildIndex.ToString() + "_" + identifier;
                result = s_status.Contains(value);
            }
            return result;
        }

        public static void Initialize()
        {
            s_status.Clear();
            s_data.Initialize();
        }
    }
}
