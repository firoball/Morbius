using UnityEngine;
using UnityEngine.EventSystems;


namespace Morbius.Scripts.UI
{
    public interface IChapterEventTarget : IEventSystemHandler
    {
        void OnSetText(string title, string[] text);
        void OnShow(GameObject sender);
        void OnHide();
    }
}
