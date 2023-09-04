using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attributes", menuName = "Attribute/Attributes")]
public class S_Attributes : ScriptableObject
{
    Dictionary<string, float> attributes;
    [SerializeField] List<S_AttributeDataTable> defaultAttributes;

    public void AddAttribute(string name, float value)
    {
        attributes.Add(name, value);
    }

    public void InitializeAttributeByDataTable(S_AttributeDataTable dataTable)
    {
        foreach(S_AttributeData attributeData in dataTable.data) 
        {
            AddAttribute(attributeData.name, attributeData.value);
        }
    }

    public float GetAttribute(string name)
    {
        return attributes[name];
    }

    public float SetAttribute(string name, float value) 
    {
        return attributes[name] = value;
    }
}
