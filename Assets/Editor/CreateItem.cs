using UnityEngine;
using UnityEditor;
using Morbius.Scripts.Ambient;
using Morbius.Scripts.Cursor;
using Morbius.Scripts.Items;

public class CreateItem
{
    [MenuItem("GameObject/3D Object/Interaction Item")]
    private static void Create()
    {
        Vector3 position = Vector3.zero;

        GameObject obj = Selection.activeGameObject;
        if (obj != null)
        {
            MeshRenderer mesh = obj.GetComponent<MeshRenderer>();
            if (mesh != null)
            {
                position = mesh.bounds.center;
            }
        }
        GameObject itemObj = new GameObject("item");
        itemObj.transform.position = position;
        itemObj.AddComponent<ItemInstance>();
        itemObj.AddComponent<CursorSceneItem>();
        itemObj.AddComponent<ItemEditorVisualizer>();
        itemObj.AddComponent<PointOfInterest>();
    }

}
