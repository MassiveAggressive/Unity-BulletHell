using AttributeData;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttributesComponent : MonoBehaviour
{
    [SerializeField] S_AttributeData defaultAttributeData;
    [SerializeField] List<Attribute> defaultAttributes = new List<Attribute>();
    protected Dictionary<string, float> attributes = new Dictionary<string, float>();

    public event EventHandler<AttributeChangedArgs> AttributeChanged;
    public class AttributeChangedArgs : EventArgs
    {
        public string name;
        public float newValue;
        public float oldValue;
    }
    public AttributeChangedArgs attributeChangedArgs = new AttributeChangedArgs();

    private void Awake()
    {
        if(defaultAttributeData)
        {
            defaultAttributes = defaultAttributeData.attributes;
        }

        foreach(Attribute attribute in defaultAttributes)
        {
            attributes[attribute.name] = attribute.value;
        }
    }

    public void AddAttribute(string name, float value)
    {
        attributes[name] = value;
    }

    public float GetAttribute(string name)
    {
        return attributes[name];
    }

    public void SetAttribute(string name, float value)
    {
        float oldValue = attributes[name];

        attributes[name] = value;

        PostAttributeChanged(name, oldValue, value);

        attributeChangedArgs.name = name;
        attributeChangedArgs.oldValue = oldValue;
        attributeChangedArgs.newValue = attributes[name];

        AttributeChanged?.Invoke(this, attributeChangedArgs);
    }

    public virtual void PostAttributeChanged(string name, float oldValue, float newValue)
    {
        
    }
}
