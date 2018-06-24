using UnityEngine;
using UnityEngine.EventSystems;

namespace Morbius.Scripts.UI
{
    public interface IInventoryEventTarget : IEventSystemHandler
    {
        void OnShow();
        void OnHide();
        void OnAdd(Sprite item);
        void OnRemove(Sprite item);
        void OnRegisterReceiver(GameObject receiver);
    }
}
