using UnityEngine;
using System;

namespace Morbius.Scripts.Game
{
    public class GameData
    {
        private bool m_goodEnding;
        private string m_scene;
        private Vector3 m_playerPosition;
        private float m_playerYRotation;

        public bool GoodEnding { get => m_goodEnding; set => m_goodEnding = value; }
        public string Scene { get => m_scene; set => m_scene = value; }
        public Vector3 PlayerPosition { get => m_playerPosition; set => m_playerPosition = value; }
        public float PlayerYRotation { get => m_playerYRotation; set => m_playerYRotation = value; }

        public GameData()
        {
            m_goodEnding = false;
            m_playerPosition = Vector3.zero;
            m_playerYRotation = 0.0f;
        }

        public void Save(int slot)
        {
            PlayerPrefs.SetInt("Morbius.GD." + slot + ".GE", Convert.ToInt32(m_goodEnding));
            PlayerPrefs.SetString("Morbius.GD." + slot + ".SC", m_scene);
            PlayerPrefs.SetFloat("Morbius.GD." + slot + ".PX", m_playerPosition.x);
            PlayerPrefs.SetFloat("Morbius.GD." + slot + ".PY", m_playerPosition.y);
            PlayerPrefs.SetFloat("Morbius.GD." + slot + ".PZ", m_playerPosition.z);
            PlayerPrefs.SetFloat("Morbius.GD." + slot + ".RY", m_playerYRotation);
        }

        public void Load(int slot)
        {
            int goodEnding = PlayerPrefs.GetInt("Morbius.GD." + slot + ".GE", Convert.ToInt32(false));
            m_goodEnding = Convert.ToBoolean(goodEnding);
            m_scene = PlayerPrefs.GetString("Morbius.GD." + slot + ".SC", null);
            m_playerPosition.x = PlayerPrefs.GetFloat("Morbius.GD." + slot + ".PX", 0.0f);
            m_playerPosition.y = PlayerPrefs.GetFloat("Morbius.GD." + slot + ".PY", 0.0f);
            m_playerPosition.z = PlayerPrefs.GetFloat("Morbius.GD." + slot + ".PZ", 0.0f);
            m_playerYRotation = PlayerPrefs.GetFloat("Morbius.GD." + slot + ".RY", 0.0f);
        }

        public void Delete(int slot)
        {
            PlayerPrefs.DeleteKey("Morbius.GD." + slot + ".GE");
            PlayerPrefs.DeleteKey("Morbius.GD." + slot + ".SC");
            PlayerPrefs.DeleteKey("Morbius.GD." + slot + ".PX");
            PlayerPrefs.DeleteKey("Morbius.GD." + slot + ".PY");
            PlayerPrefs.DeleteKey("Morbius.GD." + slot + ".PZ");
            PlayerPrefs.DeleteKey("Morbius.GD." + slot + ".RY");
            PlayerPrefs.DeleteKey("Morbius.GD." + slot + ".TX");
        }

        public Texture2D GetScreenshot(int slot)
        {
            string base64Tex = PlayerPrefs.GetString("Morbius.GD." + slot + ".TX", null);
            if (!string.IsNullOrEmpty(base64Tex))
            {
                byte[] texByte = Convert.FromBase64String(base64Tex);
                Texture2D tex = new Texture2D(2, 2);
                if (tex.LoadImage(texByte))
                {
                    tex.Apply();
                    return tex;
                }
            }
            return null;
        }

        public void SetScreenshot(Texture2D texture, int slot)
        {
            if (texture)
            {
                byte[] bytes = texture.EncodeToPNG();
                string base64Tex = Convert.ToBase64String(bytes);
                PlayerPrefs.SetString("Morbius.GD." + slot + ".TX", base64Tex);
            }
        }

    }

}
