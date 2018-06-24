using System;
using System.Collections.Generic;
using UnityEngine;

namespace Morbius.Scripts.UI
{
    [Serializable]
    public class CursorConfig
    {
        [SerializeField]
        private CursorState m_cursorState;
        [SerializeField]
        private float m_animationDuration = 0.08f;
        [SerializeField]
        private List<Sprite> m_sprites;

        public CursorState CursorState
        {
            get
            {
                return m_cursorState;
            }
        }

        public List<Sprite> Sprites
        {
            get
            {
                return m_sprites;
            }
        }

        public float AnimationDuration
        {
            get
            {
                return m_animationDuration;
            }
        }
    }
}
