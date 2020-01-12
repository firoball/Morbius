using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Morbius.Scripts.Game;

namespace Morbius.Scripts.Events
{
    public class CustomId1014Event : DefaultEvent
    {
        public override IEnumerator Execute(int eventId)
        {
            GameStatus.Data.GoodEnding = true;
            yield return new WaitForEndOfFrame();
        }
    }

}
