using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Morbius.Scripts.Items
{
    public class Combination : ScriptableObject
    {
        private AudioClip m_audio;
        private Item[] m_combinationItems;
        private Item m_newItem;
        private int m_triggerId;
    }
}
