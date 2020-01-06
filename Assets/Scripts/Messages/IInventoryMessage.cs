using UnityEngine;

namespace Morbius.Scripts.Messages
{
    public interface IInventoryMessage : IMessageSystemHandler
    {
        void OnShow();
        void OnHide();
        void OnAdd(Sprite item);
        void OnRemove(Sprite item);
        void OnRegisterReceiver(GameObject receiver);
    }
}
