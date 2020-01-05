﻿using UnityEngine;
using UnityEngine.EventSystems;
using Morbius.Scripts.Cursor;

namespace Morbius.Scripts.UI
{
    public interface IAnimatedCursorEventTarget : IEventSystemHandler
    {
        void OnSetText(string text);
        void OnSetIcon(Sprite icon);
        void OnSetCursor(CursorState cursor);
    }
}