using UnityEngine.EventSystems;

namespace Morbius.Scripts.Shaders
{
    public interface IPixelProgressEventTarget : IEventSystemHandler
    {
        void OnPixelate();
    }
}
