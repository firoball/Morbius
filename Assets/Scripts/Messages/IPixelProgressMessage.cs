using UnityEngine.EventSystems;

namespace Morbius.Scripts.Messages
{
    public interface IPixelProgressMessage : IMessageSystemHandler
    {
        void OnPixelate();
    }
}
