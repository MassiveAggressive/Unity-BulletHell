using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct S_AttributeData
{
    public string name;
    public float value;
}

[CreateAssetMenu(fileName = "AttributeDataTable", menuName = "Attribute/AttributeDataTable")]
public class S_AttributeDataTable : ScriptableObject
{
    public List<S_AttributeData> data;
}
