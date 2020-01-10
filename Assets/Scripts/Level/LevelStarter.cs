﻿using UnityEngine;
using Morbius.Scripts.Items;
using Morbius.Scripts.Messages;

namespace Morbius.Scripts.Level
{
    public class LevelStarter : MonoBehaviour
    {
        void Awake()
        {
            MessageSystem.Clear();
            Inventory.Setup();
        }
    }
}