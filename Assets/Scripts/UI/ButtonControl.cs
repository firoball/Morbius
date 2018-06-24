using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Morbius.Scripts.UI
{
    [RequireComponent(typeof(Button))]
    public class ButtonControl : MonoBehaviour
    {
        private void Awake()
        {
            Button button;
            button = GetComponent<Button>();
            button.onClick.AddListener(() => SendButtonEvent());
        }

        private void SendButtonEvent()
        {
            ExecuteEvents.Execute<IButtonEventTarget>(transform.parent.gameObject, null, (x, y) => x.OnButtonNotification(gameObject));
        }
    }
}
