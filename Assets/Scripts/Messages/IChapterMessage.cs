using UnityEngine;
using UnityEngine.EventSystems;


namespace Morbius.Scripts.Messages
{
    public interface IChapterMessage : IMessageSystemHandler
    {
        void OnSetText(string title, string[] text);
        void OnShow(GameObject sender);
        void OnHide();
    }
}
