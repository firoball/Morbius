using UnityEditor;
using UnityEngine;

public class AssetBuilder
{
    private string m_folder;
    private string m_path;
    private bool m_isReady;

    public AssetBuilder() : this("Generated")
    {
    }

    public AssetBuilder(string folder)
    {
        Debug.Log("XML Importer running...");
        m_folder = folder;
        m_isReady = CreateAndClearFolder();
    }

    public void Import(TextAsset asset, XmlType type)
    {
        if (m_isReady)
        {
            //string name = GetPrefabName(type);
            string name = GetPrefabName(asset);
//            GameObject prefab = CreateEmptyPrefab(name);
            GameObject prefab = new GameObject(name);
            switch (type)
            {
                case XmlType.CHAPTERS:
                    GameObject.DestroyImmediate(prefab);
                    ChapterFactory chapterFactory = new ChapterFactory(m_path);
                    chapterFactory.Add(asset);
                    break;

                case XmlType.COMBINATIONS:
                    CombinationFactory comboFactory = new CombinationFactory(prefab);
                    comboFactory.Add(asset);
                    break;

                case XmlType.DIALOG:
                    DialogFactory dialogFactory = new DialogFactory(prefab);
                    dialogFactory.Add(asset);
                    break;

                case XmlType.ITEMS:
                    ItemFactory itemFactory = new ItemFactory(prefab, m_path);
                    itemFactory.Add(asset);
                    break;

                default:
                    break;
            }

            if (prefab)
            {
                CreatePrefab(prefab);
            }
        }
    }

    public void Finish()
    {
        if (m_isReady)
        {
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("XML Importer", "Asset import completed.", "Ok");
            if(AssetDatabase.IsValidFolder(m_path))
            {
                UnityEngine.Object folder = AssetDatabase.LoadMainAssetAtPath(m_path);
                Selection.activeObject = folder;
                EditorGUIUtility.PingObject(folder);
            }
        }
        Debug.Log("XML Importer finished.");
    }

    private bool CreateAndClearFolder()
    {
        bool doCreate = true;
        string folder = "assets/" + m_folder;
        if (AssetDatabase.IsValidFolder(folder))
        {
            if (EditorUtility.DisplayDialog
                (
                "XML Importer",
                "Folder <" + m_folder + "> already exists.\nProceed and delete folder with all contents?\nNote: this cannot be undone!", 
                "Yes", "No"
                )
             )
            {
                AssetDatabase.DeleteAsset(folder);
            }
            else
            {
                doCreate = false;
            }
        }

        if (doCreate)
        {
            string guid = AssetDatabase.CreateFolder("assets", m_folder);
            m_path = AssetDatabase.GUIDToAssetPath(guid);
        }

        return doCreate;
    }

    private GameObject CreateEmptyPrefab(string prefabName)
    {
        string prefabPath = m_path + "/" + prefabName + ".prefab";
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

        //only create new prefab if no matching one was found
        if (prefab == null)
        {
            //create object in scene, make it a prefab, remove it from scene
            GameObject newPrefab = new GameObject(prefabName);
            prefab = PrefabUtility.CreatePrefab(prefabPath, newPrefab);
            GameObject.DestroyImmediate(newPrefab);
            if (AssetDatabase.Contains(prefab))
            {
                Debug.Log("XML Importer: " + prefabName + " created.");
            }
        }

        return prefab;
    }

    private GameObject CreatePrefab(GameObject obj)
    {
        if (!obj)
        {
            return null;
        }

        string prefabName = obj.name;
        string prefabPath = m_path + "/" + prefabName + ".prefab";
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

        //only create new prefab if no matching one was found
        if (prefab == null)
        {
            //make GameObject a prefab, remove it from scene
            prefab = PrefabUtility.CreatePrefab(prefabPath, obj);
            GameObject.DestroyImmediate(obj);
            if (AssetDatabase.Contains(prefab))
            {
                Debug.Log("XML Importer: " + prefabName + " created.");
            }
        }

        return prefab;
    }

    private string GetPrefabName(XmlType type)
    {
        string name = type.ToString();
        name = name.ToLower();
        name = char.ToUpper(name[0]) + name.Substring(1);
        name += "Config";

        return name;
    }

    private string GetPrefabName(TextAsset asset)
    {
        string name = asset.name;
        name = char.ToUpper(name[0]) + name.Substring(1);

        return name;
    }
}
