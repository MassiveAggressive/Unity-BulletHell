using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class S_Item : ScriptableObject
{
    public string itemName;
    public string description;
    public Texture2D texture;
    public ItemPrimaryType itemPrimaryType = ItemPrimaryType.None;
    public ItemInventoryType itemInventoryType = ItemInventoryType.None;
    public ItemEquipmentType itemEquipmentType = ItemEquipmentType.None;
    public bool isStackable = false;
    public int maxStack = 1;
    public int stack = 1;
    public List<Attribute> attributes; 
    public GameObject gameObject;
}
public enum ItemPrimaryType
{
    None,
    Weapon,
    Generator,
    Extra
}
public enum ItemInventoryType
{
    None,
    PrimaryWeapon,
    SecondaryWeapon,
    ShieldGenerator,
    SpeedGenerator,
    Extra
}
public enum ItemEquipmentType
{
    None,
    PrimaryWeapon,
    SecondaryWeapon,
    Generator,
    Extra
}