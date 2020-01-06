using UnityEngine;
using UnityEngine.EventSystems;

namespace Morbius.Scripts.Messages
{
    public interface IDialogMessage : IMessageSystemHandler
    {
        void OnShowText(string speaker, string text);
        void OnShowDecision(GameObject sender, string[] decisions);
        void OnHide();
    }
}
