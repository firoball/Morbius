using UnityEngine;

namespace Morbius.Scripts.Messages
{
    public interface IPanelMessage : IMessageSystemHandler
    {
        void OnShow(Sprite sprite);
        void OnHide();
    }
}
