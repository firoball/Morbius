using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Morbius.Scripts.Ambient;
using Morbius.Scripts.Items;

namespace Morbius.Scripts.Game
{
    public static class GameStatus
    {
        private static List<string> s_status = new List<string>();
        private static int s_slot = 0;

        private static GameData s_data = new GameData();
        private static OptionData s_options = new OptionData();

        public static GameData Data { get => s_data; }
        public static OptionData Options { get => s_options; }
        public static int Slot { get => s_slot; set => s_slot = value; }

        public static void Set(string identifier, bool global)
        {
            if (!string.IsNullOrWhiteSpace(identifier))
            {
                string value = GetIdentifier(identifier, global);
                s_status.Add(value);
            }
        }

        public static void Unset(string identifier, bool global)
        {
            if (IsSet(identifier, global))
            {
                string value = GetIdentifier(identifier, global);
                s_status.Remove(value);
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

        public static void ApplySettings()
        {
            AudioListener.volume = s_options.MasterVolume;
            AudioManager.MusicVolume = s_options.MusicVolume;
            QualitySettings.SetQualityLevel(Convert.ToInt32(s_options.QualityLevel), true);
        }

        public static void Initialize()
        {
            s_status.Clear();
            s_data = new GameData();
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

        public static void SaveGame(int slot)
        {
            PlayerPrefs.SetInt("Morbius.GS." + slot + ".C", s_status.Count);
            for (int i = 0; i < s_status.Count; i++)
            {
                PlayerPrefs.SetString("Morbius.GS." + slot + "." + i, s_status[i]);
            }
            s_data.Save(slot);
            ItemDatabase.Save(slot);
            Inventory.Save(slot);
            PlayerPrefs.Save();
        }

        public static void LoadGame(int slot)
        {
            s_slot = slot;
            s_status = new List<string>();
            s_data = new GameData();

            int count = PlayerPrefs.GetInt("Morbius.GS." + slot + ".C", 0);
            for (int i = 0; i < count; i++)
            {
                string data = PlayerPrefs.GetString("Morbius.GS." + slot + "." + i, null);
                if (!string.IsNullOrWhiteSpace(data))
                {
                    s_status.Add(data);
                }
            }
            s_data.Load(slot);
            ItemDatabase.Load(slot);
            Inventory.Load(slot);
        }

        public static void DeleteGame(int slot)
        {
            for (int i = 0; i < s_status.Count; i++)
            {
                PlayerPrefs.DeleteKey("Morbius.GS." + slot + "." + i);
            }
            PlayerPrefs.DeleteKey("Morbius.GS." + slot + ".C");
            s_data.Delete(slot);
            ItemDatabase.Delete(slot);
            Inventory.Delete(slot);
        }

        /*public static void DebugPrint()
        {
            if(Input.GetMouseButtonDown(2))
            {
                foreach (string item in s_status)
                {
                    Debug.Log(item);
                }
            }
        }*/

    }
}
