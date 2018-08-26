using UnityEngine;
using UnityEngine.EventSystems;


namespace Morbius.Scripts.UI
{
    public interface IChapterResultTarget : IEventSystemHandler
    {
        void OnChapterDone();
    }
}
