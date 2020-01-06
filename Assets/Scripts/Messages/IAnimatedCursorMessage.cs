using UnityEngine;
using Morbius.Scripts.Cursor;

namespace Morbius.Scripts.Messages
{
    public interface IAnimatedCursorMessage : IMessageSystemHandler
    {
        void OnSetText(string text);
        void OnSetIcon(Sprite icon);
        void OnSetCursor(CursorState cursor);
    }
}
