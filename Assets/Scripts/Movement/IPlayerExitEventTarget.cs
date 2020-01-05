using UnityEngine;
using UnityEngine.EventSystems;

namespace Morbius.Scripts.Movement
{
    public interface IPlayerExitEventTarget : IEventSystemHandler
    {
        void OnPlayerExit();
    }
}
