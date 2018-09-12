using UnityEngine;
using Morbius.Scripts.Items;

namespace Morbius.Scripts.Cursor
{
    public class CursorScenePortal : MonoBehaviour, ICursorObject
    {
        public CursorInfo GetCursorInfo()
        {
            CursorInfo info = new CursorInfo()
            {
                Label = null,
                Icon = null,
                IsGrabable = false,
                IsInteractable = false,
                IsPortal = true
            };

            return info;
        }
    }
}
