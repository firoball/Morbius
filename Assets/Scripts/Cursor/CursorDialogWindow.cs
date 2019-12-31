using UnityEngine;

namespace Morbius.Scripts.Cursor
{
    public class CursorDialogWindow : MonoBehaviour, ICursorObject
    {
        public CursorInfo GetCursorInfo()
        {
            CursorInfo info = new CursorInfo()
            {
                Label = null,
                Icon = null,
                IsGrabable = false,
                IsDialog = true,
                IsInteractable = false,
                IsPortal = false
            };

            return info;
        }
    }
}
