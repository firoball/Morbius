using UnityEngine.EventSystems;

namespace Morbius.Scripts.UI
{
    public interface IDialogResultTarget : IEventSystemHandler
    {
        void OnDecision(int index);
    }
}
