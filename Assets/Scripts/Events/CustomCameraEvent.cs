using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Morbius.Scripts.Ambient;
using Morbius.Scripts.Level;
using Morbius.Scripts.Messages;

namespace Morbius.Scripts.Events
{
    [RequireComponent(typeof(CameraSwitcher))]
    public class CustomCameraEvent : DefaultEvent
    {
        private CameraSwitcher m_switcher;

        private void Awake()
        {
            m_switcher = GetComponent<CameraSwitcher>();
        }

        public override IEnumerator Execute(int eventId)
        {
            MessageSystem.Execute<IInputBlockerMessage>((x, y) => x.OnBlock());

            AudioManager.Fade(true, 1.0f);
            m_switcher.SetFixedCamera(true);
            yield return new WaitForSeconds(1.0f);
            yield return new WaitWhile(() => !Input.anyKeyDown);
            m_switcher.SetFixedCamera(false);
            AudioManager.Fade(false, 1.0f);
            yield return new WaitForSeconds(1.0f);

            MessageSystem.Execute<IInputBlockerMessage>((x, y) => x.OnUnblock());
        }
    }

}
