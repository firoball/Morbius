using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class XmlImport : EditorWindow
{
    private static List<TextAsset> s_assets;
    private static List<XmlType> s_xmlTypes;
    private Vector2 m_scrollPos;

    [MenuItem("Window/XML Importer")]
    private static void OpenWindow()
    {
        XmlImport window = GetWindow<XmlImport>();
        window.titleContent = new GUIContent("XML Importer");
    }

    private void OnGUI()
    {
        DrawAssetList();
        DrawButtons();
        if (Event.current.type == EventType.Repaint)
        {
            UpdateAssetList();
        }
    }

    private void OnEnable()
    {
        Init();
    }

    private void OnDestroy()
    {
        Save();
    }

    private void UpdateAssetList()
    {
        //now remove all empty elements
        for (int i = s_xmlTypes.Count - 1; i >= 0; i--)
        {
            if ((s_assets[i] == null) && (i < s_assets.Count - 1))
            {
                s_xmlTypes.RemoveAt(i);
            }
        }
        for (int i = s_assets.Count - 1; i >= 0; i--)
        {
            if ((s_assets[i] == null) && (i < s_assets.Count - 1))
            {
                s_assets.RemoveAt(i);
            }
        }

        //if last element is not empty, add new empty element
        if (s_assets.Count > 0 && s_assets[s_assets.Count - 1] != null)
        {
            s_assets.Add(null);
            //take over setting of previous element
            s_xmlTypes.Add(s_xmlTypes[s_xmlTypes.Count - 1]);
        }
    }

    private void DrawAssetList()
    {
        EditorGUILayout.LabelField("XML Assets");

        m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos);
        for (int i = 0; i < s_assets.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayoutOption[] options1 = {GUILayout.MinWidth(150.0f) };
            s_assets[i] = EditorGUILayout.ObjectField(s_assets[i], typeof(TextAsset), false, options1) as TextAsset;
            GUILayoutOption[] options2 = {GUILayout.MinWidth(100.0f) };
            s_xmlTypes[i] = (XmlType)EditorGUILayout.EnumPopup(s_xmlTypes[i], options2);
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();

    }

    private void DrawButtons()
    {
        EditorGUILayout.Separator();
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        GUILayoutOption[] options = { GUILayout.MinWidth(80.0f), GUILayout.MaxWidth(80.0f) };

        Rect buttonClose = EditorGUILayout.GetControlRect(options);
        if (GUI.Button(buttonClose, "Close"))
        {
            Close();
        }

        Rect buttonClear = EditorGUILayout.GetControlRect(options);
        if (GUI.Button(buttonClear, "Clear"))
        {
            Clear();
        }

        EditorGUILayout.Space();

        Rect buttonOk = EditorGUILayout.GetControlRect(options);
        if (GUI.Button(buttonOk, "Import"))
        {
            Run();
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
    }

    private void Init()
    {
        if ((s_assets == null) || (s_xmlTypes == null))
        {
            s_assets = new List<TextAsset>();
            s_xmlTypes = new List<XmlType>();
            Load();
            s_assets.Add(null);
            s_xmlTypes.Add(XmlType.DEFAULT);
        }
    }

    private void Clear()
    {
        DeleteSaveState();
        s_assets = null;
        s_xmlTypes = null;
        Init();
    }

    private void Run()
    {
        AssetBuilder builder = new AssetBuilder();
        for (int i = 0; i < s_assets.Count - 1; i++)
        {
            builder.Import(s_assets[i], s_xmlTypes[i]);
        }
        builder.Finish();
    }

    private void DeleteSaveState()
    {
        int i = 0;
        bool done = false;
        do
        {
            string prefId1 = PlayerSettings.productName + "_n" + i;
            string prefId2 = PlayerSettings.productName + "_t" + i;
            if (EditorPrefs.HasKey(prefId1) && EditorPrefs.HasKey(prefId2))
            {
                EditorPrefs.DeleteKey(prefId1);
                EditorPrefs.DeleteKey(prefId2);
            }
            else
            {
                done = true;
            }
            i++;
        } while (!done);
    }

    private void Save()
    {
        DeleteSaveState();
        for (int i = 0; i < s_assets.Count; i++)
        {
            if (s_assets[i] != null)
            {
                string prefId1 = PlayerSettings.productName + "_n" + i;
                EditorPrefs.SetString(prefId1, s_assets[i].name);
                string prefId2 = PlayerSettings.productName + "_t" + i;
                EditorPrefs.SetInt(prefId2, (int)s_xmlTypes[i]);
            }
        }

    }

    private void Load()
    {
        int i = 0;
        bool done = false;

        do
        {
            string prefId1 = PlayerSettings.productName + "_n" + i;
            string prefId2 = PlayerSettings.productName + "_t" + i;
            if (EditorPrefs.HasKey(prefId1) && EditorPrefs.HasKey(prefId2))
            {
                string name = EditorPrefs.GetString(prefId1);
                string[] matches = AssetDatabase.FindAssets(name + " t:TextAsset");
                if (
                    (matches.Length > 0) && !string.IsNullOrEmpty(matches[0]) &&
                    (s_assets != null) && (s_xmlTypes != null)
                )
                {
                    string path = AssetDatabase.GUIDToAssetPath(matches[0]);

                    //ugly patch to skip search in editor folder
                    int skipCounter = 1;
                    while (path.Contains("Editor") && matches.Length > skipCounter)
                    {
                        path = AssetDatabase.GUIDToAssetPath(matches[skipCounter]);
                        skipCounter++;
                    }

                    TextAsset asset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
                    s_assets.Add(asset);

                    XmlType type = (XmlType)EditorPrefs.GetInt(prefId2);
                    s_xmlTypes.Add(type);
                }
            }
            else
            {
                done = true;
            }
            i++;
        } while (!done);
    }

}
