using UnityEngine;

namespace Morbius.Scripts.Messages
{
    public interface IDarkAreaMessage : IMessageSystemHandler
    {
        void OnDarkAreaEnter();
        void OnDarkAreaExit();
    }
}
