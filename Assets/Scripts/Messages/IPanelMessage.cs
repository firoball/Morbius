using UnityEngine;

namespace Morbius.Scripts.Messages
{
    public interface IPanelMessage : IMessageSystemHandler
    {
        void OnShow(Sprite sprite);
        void OnShow(Sprite sprite, bool nospin);
        void OnHide();
    }
}
