using UnityEngine;
using UnityEngine.EventSystems;

namespace Morbius.Scripts.Messages
{
    public interface ICursorUIMessage : IMessageSystemHandler
    {
        void OnUIEnter();
        void OnUIExit();
    }
}
