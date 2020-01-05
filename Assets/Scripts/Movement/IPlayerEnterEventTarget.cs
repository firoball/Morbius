using UnityEngine;
using UnityEngine.EventSystems;

namespace Morbius.Scripts.Movement
{
    public interface IPlayerEnterEventTarget : IEventSystemHandler
    {
        void OnPlayerEnter();
    }
}
