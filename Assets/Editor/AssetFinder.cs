using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class AssetFinder
{
    public static AudioClip FindClip(string name)
    {
        AudioClip clip = null;
        if (!string.IsNullOrEmpty(name))
        {
            string assetname = Path.GetFileNameWithoutExtension(name);
            string[] matches = AssetDatabase.FindAssets(assetname + " t:AudioClip");
            if ((matches.Length > 0) && !string.IsNullOrEmpty(matches[0]))
            {
                string path = AssetDatabase.GUIDToAssetPath(matches[0]);
                clip = AssetDatabase.LoadAssetAtPath<AudioClip>(path);
            }
            else
            {
                Debug.LogWarning("AssetFinder: AudioClip <" + assetname + "> not found.");
            }
        }

        return clip;
    }

    public static Sprite FindSprite(string name)
    {
        Sprite sprite = null;
        if (!string.IsNullOrEmpty(name))
        {
            string assetname = Path.GetFileNameWithoutExtension(name);
            string[] matches = AssetDatabase.FindAssets(assetname + " t:Sprite");
            if ((matches.Length > 0) && !string.IsNullOrEmpty(matches[0]))
            {
                string path = AssetDatabase.GUIDToAssetPath(matches[0]);
                sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            }
            else
            {
                Debug.LogWarning("AssetFinder: Sprite <" + assetname + "> not found.");
            }
        }

        return sprite;
    }

    public static GameObject FindGameObject(string name)
    {
        GameObject go = null;
        if (!string.IsNullOrEmpty(name))
        {
            string assetname = Path.GetFileNameWithoutExtension(name);
            string[] matches = AssetDatabase.FindAssets(assetname + " t:GameObject");
            if ((matches.Length > 0) && !string.IsNullOrEmpty(matches[0]))
            {
                //prefer prefab over imported model, if available
                string str = matches.Where(x => AssetDatabase.GUIDToAssetPath(x).Contains(".prefab")).FirstOrDefault();
                if (string.IsNullOrEmpty(str))
                    str = matches[0];

                string path = AssetDatabase.GUIDToAssetPath(str);
                go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            }
            else
            {
                Debug.LogWarning("AssetFinder: GameObject <" + assetname + "> not found.");
            }
        }

        return go;
    }

    public static List<Type> FindTypes(string name)
    {
        List<Type> types = new List<Type>();
        if (!string.IsNullOrEmpty(name))
        {
            string assetname = Path.GetFileNameWithoutExtension(name);
            Debug.Log("AssetFinder: Searching types for <" + assetname + "> ...");
            string[] matches = AssetDatabase.FindAssets(assetname);
            if ((matches.Length > 0) && !string.IsNullOrEmpty(matches[0]))
            {
                foreach (string str in matches)
                {
                    string path = AssetDatabase.GUIDToAssetPath(str);
                    Type t = AssetDatabase.GetMainAssetTypeAtPath(path);
                    types.Add(t);
                    Debug.Log("AssetFinder: Found <" + path + "> of type " + t.ToString());
                }
            }
            else
            {
                Debug.LogWarning("AssetFinder: No matches for <" + assetname + "> found.");
            }
        }

        return types;
    }

}

