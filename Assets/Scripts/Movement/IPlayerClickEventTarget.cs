using UnityEngine;
using UnityEngine.EventSystems;

namespace Morbius.Scripts.Movement
{
    public interface IPlayerClickEventTarget : IEventSystemHandler
    {
        void OnPlayerClick();
    }
}
