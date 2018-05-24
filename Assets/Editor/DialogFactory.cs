using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

public class DialogFactory : GenericPrefabFactory<DialogManager>
{
    public DialogFactory(GameObject prefab) : base (prefab)
    {
    }

    protected override void Deserialize(TextAsset xmlAsset)
    {
        try
        {
            StringReader reader = new StringReader(xmlAsset.ToString());
            XmlSerializer serializer = new XmlSerializer(typeof(XmlDialog));
            XmlDialog dialog = (XmlDialog)serializer.Deserialize(reader);

            //dialogs don't provide an id, no additional check is done
            m_manager.Dialogs.Add(dialog);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
}
