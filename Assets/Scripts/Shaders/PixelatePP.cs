using System;
using UnityEngine;
using Morbius.Scripts.Messages;

namespace Morbius.Scripts.Shaders
{
    [RequireComponent(typeof(Camera))]
    [ExecuteInEditMode]
    public class PixelatePP : MonoBehaviour, IPixelProgressMessage
    {
        [SerializeField]
        private Material m_material;

        private float m_factor = 1.0f;
        private bool m_entry = true;
        private bool m_exit = false;

        void Start()
        {
            if (!SystemInfo.supportsImageEffects || null == m_material ||
               null == m_material.shader || !m_material.shader.isSupported)
            {
                enabled = false;
                return;
            }
            if (Application.isPlaying)
                m_material.SetFloat("_Factor", m_factor);

            MessageSystem.Register<IPixelProgressMessage>(gameObject);
        }

        private void Update()
        {
            if (Application.isPlaying)
            {
                if (m_exit)
                {
                    m_factor = Mathf.Min(1.0f, m_factor + Time.deltaTime);
                    m_material.SetFloat("_Factor", m_factor);
                    if (m_factor >= 1.0f)
                        m_exit = false;
                }
                else if (m_entry)
                {
                    m_factor = Mathf.Max(0.0f, m_factor - Time.deltaTime);
                    m_material.SetFloat("_Factor", m_factor);
                    if (m_factor <= 0.0f)
                        m_entry = false;
                }
                else
                {
                    //do nothing
                }
            }
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Graphics.Blit(source, destination, m_material);
        }

        public void OnPixelate()
        {
            m_entry = false;
            m_exit = true;
        }
    }
}