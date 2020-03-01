using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Morbius.Scripts.Messages
{
    public class MessageSystem
    {
        private static Dictionary<Type, MessageTargets> s_dictionary = new Dictionary<Type, MessageTargets>();

        public static void Clear()
        {
            foreach(KeyValuePair<Type, MessageTargets> pair in s_dictionary)
            {
                pair.Value.Clear();
            }
            s_dictionary.Clear();
            //Debug.Log("MessageSystem: Buffer cleared.");
        }

        public static void Register<T>(GameObject receiver) where T : IMessageSystemHandler
        {
            Type type = typeof(T);
            MessageTargets messageTargets; 

            if (!s_dictionary.TryGetValue(type, out messageTargets))
            {
                messageTargets = new MessageTargets();
                s_dictionary.Add(type, messageTargets);
            }
            messageTargets.Add(receiver);
            //Debug.Log("MessageSystem: Registered <" + receiver.name + "> for " + type.Name);
        }

        public static void Unregister<T>(GameObject receiver) where T : IMessageSystemHandler
        {
            Type type = typeof(T);
            MessageTargets messageTargets;
            if (s_dictionary.TryGetValue(type, out messageTargets))
            {
                messageTargets.Remove(receiver);
            }
            //Debug.Log("MessageSystem: Unregistered <" + receiver.name + "> for " + type.Name);
        }

        public static void Execute<T>(ExecuteEvents.EventFunction<T> functor) where T : IMessageSystemHandler
        {
            Type type = typeof(T);
            MessageTargets messageTargets;
            if (s_dictionary.TryGetValue(type, out messageTargets))
            {
                foreach (GameObject target in messageTargets)
                {
                    ExecuteEvents.Execute<T>(target, null, functor);
                }
            }
        }
    }
}
