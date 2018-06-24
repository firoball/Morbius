using UnityEngine;
using UnityEngine.EventSystems;

namespace Morbius.Scripts.UI
{
    public interface IDialogEventTarget : IEventSystemHandler
    {
        void OnShowText(string speaker, string text);
        void OnShowDecision(GameObject sender, string[] decisions);
        void OnHide();
    }
}
