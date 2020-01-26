using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Morbius.Scripts.Game
{
    public static class GameStatus
    {
        private static List<string> s_status = new List<string>();
        private static GameData s_data = new GameData();

        public static GameData Data { get => s_data; }

        public static void Set(string identifier, bool global)
        {
            if (!string.IsNullOrWhiteSpace(identifier))
            {
                string value =GetIdentifier(identifier, global);
                s_status.Add(value);
            }
        }

        public static bool IsSet(string identifier, bool global)
        {
            bool result = false;
            if (!string.IsNullOrWhiteSpace(identifier))
            {
                string value = GetIdentifier(identifier, global);
                result = s_status.Contains(value);
            }
            return result;
        }

        public static void Initialize()
        {
            s_status.Clear();
            s_data.Initialize();
        }

        private static string GetIdentifier(string identifier, bool global)
        {
            string value;
            if (global)
            {
                value = "g_" + identifier;
            }
            else
            {
                value = SceneManager.GetActiveScene().buildIndex.ToString() + "_" + identifier;
            }
            return value;
        }

        public static void DebugPrint()
        {
            if(Input.GetMouseButtonDown(2))
            {
                foreach (string item in s_status)
                {
                    Debug.Log(item);
                }
            }
        }

    }
}
