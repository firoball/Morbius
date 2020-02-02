using UnityEngine;
using System.Runtime.InteropServices;

namespace Morbius.Scripts.Util
{
    public class URLReader
    {
        [DllImport("__Internal")]
        private static extern string GetURLFromPage();

        [DllImport("__Internal")]
        private static extern string GetQueryParam(string paramId);

        public static string ReadQueryParam(string paramId)
        {
            return GetQueryParam(paramId);
        }

        public static string ReadURL()
        {
            return GetURLFromPage();
        }
    }
}