using UnityEngine;
using UnityEngine.EventSystems;

namespace Morbius.Scripts.UI
{
    public interface IInfoTextEventTarget : IEventSystemHandler
    {
        void OnShow(string text);
        void OnHide();
    }
}
