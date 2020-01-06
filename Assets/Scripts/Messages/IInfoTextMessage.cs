
namespace Morbius.Scripts.Messages
{
    public interface IInfoTextMessage : IMessageSystemHandler
    {
        void OnShow(string text);
        void OnShow(string text, float duration);
        void OnHide();
    }
}
