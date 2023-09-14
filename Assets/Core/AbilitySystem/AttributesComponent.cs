using AttributeData;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AttributesContainerComponent : MonoBehaviour
{
    [SerializeField] S_AttributeData defaultAttributeData;
    [SerializeField] List<Attribute> defaultAttributes = new List<Attribute>();

    protected Dictionary<string, float> attributes = new Dictionary<string, float>();
    public Dictionary<string, float> Attributes { get { return attributes; } set { attributes = value; } }

    protected Dictionary<string, Dictionary<string, float>> allAttributes = new Dictionary<string, Dictionary<string, float>>();

    public event EventHandler<AttributeChangedArgs> AttributeChanged;
    public class AttributeChangedArgs : EventArgs
    {
        public string name;
        public float newValue;
        public float oldValue;
    }
    public AttributeChangedArgs attributeChangedArgs = new AttributeChangedArgs();

    public event EventHandler<AttributeAboutToChangeArgs> AttributeAboutToChange;
    public class AttributeAboutToChangeArgs : EventArgs
    {
        public string name;
    }
    private AttributeAboutToChangeArgs attributeAboutToChangeArgs = new AttributeAboutToChangeArgs();

    public event EventHandler<AttributesChangedArgs> AttributesChanged;
    public class AttributesChangedArgs : EventArgs
    {
        public Dictionary<string, float> attributes;
    }
    private AttributesChangedArgs attributesChangedArgs = new AttributesChangedArgs();

    private void Awake()
    {
        if(defaultAttributeData)
        {
            defaultAttributes = defaultAttributeData.attributes;
        }

        Dictionary<string, float> baseAttributes = new Dictionary<string, float>();
        foreach (Attribute attribute in defaultAttributes)
        {
            baseAttributes[attribute.name] = attribute.value;
        }

        AddAttributeField("BaseAttributes", baseAttributes);
    }

    public Dictionary<string, float> GetAttributes()
    {
        return attributes;
    }

    public class GetAttributeArgs : EventArgs
    {
        public float value = 0f;
        public bool found = false;
    }
    private GetAttributeArgs getAttributeArgs = new GetAttributeArgs();

    public GetAttributeArgs GetAttribute(string name)
    {
        if(attributes.ContainsKey(name))
        {
            getAttributeArgs.value = attributes[name];
            getAttributeArgs.found = true;
        }
        else
        {
            getAttributeArgs.value = 0f;
            getAttributeArgs.found = false;
        }

        return getAttributeArgs;
    }

    public void SetAttribute(string name, float value)
    {
        print(name);
        if(attributes.ContainsKey(name))
        {
            float oldValue = attributes[name];

            attributes[name] = value;

            attributeAboutToChangeArgs.name = name;
            AttributeAboutToChange?.Invoke(this, attributeAboutToChangeArgs);

            attributeChangedArgs.name = name;
            attributeChangedArgs.oldValue = oldValue;
            attributeChangedArgs.newValue = attributes[name];


            AttributeChanged?.Invoke(this, attributeChangedArgs);
        }
    }

    public void AddAttributeField(string fieldName, Dictionary<string, float> newAttributes)
    {
        allAttributes[fieldName] = newAttributes;

        CalculateAllAttributes();
    }

    private void CalculateAllAttributes()
    {
        attributes.Clear();

        foreach (string fieldName in allAttributes.Keys) 
        {
            foreach(string attributeName in allAttributes[fieldName].Keys)
            {
                float currentValue = 0f;

                if(attributes.ContainsKey(attributeName))
                {
                    currentValue = attributes[attributeName];
                }

                currentValue += allAttributes[fieldName][attributeName];

                attributes[attributeName] = currentValue;
            }
        }

        attributesChangedArgs.attributes = attributes;
        AttributesChanged?.Invoke(this, attributesChangedArgs);

        foreach(string attributeName in attributes.Keys)
        {
            attributeChangedArgs.name = attributeName;
            attributeChangedArgs.newValue = attributes[attributeName];

            AttributeChanged?.Invoke(this, attributeChangedArgs);
        }
    }
}
