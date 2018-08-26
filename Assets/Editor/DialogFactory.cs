using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Morbius.Scripts.Dialog;

public class DialogFactory : GenericPrefabFactory<Dialog>
{
    private Dictionary<string, DialogElement> m_objectTable;
    private Dictionary<DialogText, string> m_targetTable;
    private Dictionary<DialogChoices, string[]> m_choiceTable;
    private Dictionary<string, string> m_gotoTable;

    public DialogFactory(GameObject prefab) : base(prefab)
    {
        m_objectTable = new Dictionary<string, DialogElement>();
        m_targetTable = new Dictionary<DialogText, string>();
        m_choiceTable = new Dictionary<DialogChoices, string[]>();
        m_gotoTable = new Dictionary<string, string>();
    }

    private AudioClip FindClip(string name)
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
                Debug.LogWarning("DialogFactory: AudioClip <" + name + "> not found.");
            }
        }

        return clip;
    }

    private string FindTargetId(XmlDialogElement content)
    {
        string result = null;
        if (!string.IsNullOrEmpty(content.Id))
        {
            int id = Convert.ToInt32(content.Id);
            id++;
            result = id.ToString();
        }
        else
        {
            //broken
        }

        return result;
    }

    private string ResolveRedirections(string target)
    {
        //check for any gotos first and patch target id
        string redirection = null;
        if (!string.IsNullOrEmpty(target) && m_gotoTable.TryGetValue(target, out redirection))
        {
            target = redirection;
        }
        return target;
    }

    private void BuildDialogText(XmlDialogText text, string name)
    {
        GameObject element = AddChild(name);
        DialogText dialogText = element.AddComponent<DialogText>();

        dialogText.Speaker = text.Name;
        dialogText.Text = text.Text;
        dialogText.Clip = FindClip(text.File);

        m_objectTable.Add(text.Id, dialogText);
        string targetId = FindTargetId(text);
        m_targetTable.Add(dialogText, targetId);
        if (!m_component.Head)
        {
            m_component.Head = dialogText;
        }
    }

    private void BuildDialogChoice(XmlDialogChoice choice, string name)
    {
        GameObject element = AddChild(name);
        DialogChoices dialogChoices = element.AddComponent<DialogChoices>();

        dialogChoices.Choices = choice.ChoiceItems.Select(x => new DialogChoice()
        {
            Text = x.Text,
            Result = x.Returnvalue
        }
        ).ToArray();
        string[] targets = choice.ChoiceItems.Select(x => x.Target).ToArray();

        m_objectTable.Add(choice.Id, dialogChoices);
        m_choiceTable.Add(dialogChoices, targets);
        if (!m_component.Head)
        {
            m_component.Head = dialogChoices;
        }
    }

    private void LinkDialogTexts()
    {
        foreach (KeyValuePair<DialogText, string> kvp in m_targetTable)
        {
            DialogText key = kvp.Key;
            string target = kvp.Value;
            target = ResolveRedirections(target);

            //now link
            DialogElement targetElement = null;
            if (!string.IsNullOrEmpty(target) && m_objectTable.TryGetValue(target, out targetElement))
            {
                key.Next = targetElement;
            }
        }
    }

    private void LinkDialogChoices()
    {
        foreach (KeyValuePair<DialogChoices, string[]> kvp in m_choiceTable)
        {
            DialogChoices key = kvp.Key;
            string[] values = kvp.Value;
            for (int i = 0; i < values.Length; i++)
            {
                string target = values[i];
                target = ResolveRedirections(target);

                //now link
                DialogElement targetElement = null;
                if (!string.IsNullOrEmpty(target) && m_objectTable.TryGetValue(target, out targetElement))
                {
                    key.Choices[i].Next = targetElement;
                }
            }
        }
    }

    private void Cleanup()
    {
        m_objectTable.Clear();
        m_choiceTable.Clear();
        m_targetTable.Clear();
        m_gotoTable.Clear();
    }

    protected override void Deserialize(TextAsset xmlAsset)
    {
        try
        {
            StringReader reader = new StringReader(xmlAsset.ToString());
            XmlSerializer serializer = new XmlSerializer(typeof(XmlDialog));
            XmlDialog dialog = (XmlDialog)serializer.Deserialize(reader);

            int textCount = 0;
            int choiceCount = 0;
            foreach (XmlDialogElement content in dialog.Elements)
            {
                if (content is XmlDialogText)
                {
                    XmlDialogText text = content as XmlDialogText;
                    BuildDialogText(text, "text" + textCount);
                    textCount++;
                }
                else if (content is XmlDialogChoice)
                {
                    XmlDialogChoice choice = content as XmlDialogChoice;
                    BuildDialogChoice(choice, "choice" + choiceCount);
                    choiceCount++;
                }
                else if (content is XmlDialogGoto)
                {
                    XmlDialogGoto redirection = content as XmlDialogGoto;
                    m_gotoTable.Add(redirection.Id, redirection.Target);
                }
                else
                {
                    //END or unknown element - nothing to do
                }

            }

            LinkDialogTexts();
            LinkDialogChoices();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
        Cleanup();
    }
}
