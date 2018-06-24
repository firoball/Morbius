using UnityEngine;
using UnityEngine.EventSystems;

namespace Morbius.Scripts.UI
{
    public interface IButtonEventTarget : IEventSystemHandler
    {
        void OnButtonNotification(GameObject sender);
    }
}
