
namespace Morbius.Scripts.Messages
{
    public interface IDialogMessage : IMessageSystemHandler
    {
        void OnShowText(string speaker, string text);
        void OnShowDecision(string[] decisions);
        void OnHide();
    }
}
