using UnityEngine;
using UnityEngine.EventSystems;

namespace Morbius.Scripts.UI
{
    public interface IHoverEventTarget : IEventSystemHandler
    {
        void OnHoverBeginNotification(GameObject sender);
        void OnHoverEndNotification(GameObject sender);
    }
}
