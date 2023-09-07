using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttributeData
{
    [CreateAssetMenu(fileName = "AttributeData", menuName = "Attribute/AttributeData")]
    public class S_AttributeData : ScriptableObject
    {
        public List<Attribute> attributes;
    }
}