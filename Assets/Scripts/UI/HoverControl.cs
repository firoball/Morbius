using UnityEngine;
using UnityEngine.EventSystems;

namespace Morbius.Scripts.UI
{
    public class HoverControl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData data)
        {
            ExecuteEvents.Execute<IHoverEventTarget>(transform.parent.gameObject, null, (x, y) => x.OnHoverBeginNotification(gameObject));
        }

        public void OnPointerExit(PointerEventData data)
        {
            ExecuteEvents.Execute<IHoverEventTarget>(transform.parent.gameObject, null, (x, y) => x.OnHoverEndNotification(gameObject));
        }
    }
}
