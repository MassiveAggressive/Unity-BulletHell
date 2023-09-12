using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ItemLocation
{
    InInventory,
    InEquipment
}

public class InventoryEquipmentComponent : MonoBehaviour
{
    Dictionary<ItemInventoryType, Dictionary<int, SItemSlot>> inventorySlots = new Dictionary<ItemInventoryType, Dictionary<int, SItemSlot>>();

    Dictionary<ItemEquipmentType, Dictionary<int, SItemSlot>> equipmentSlots = new Dictionary<ItemEquipmentType, Dictionary<int, SItemSlot>>();
    public Dictionary<ItemEquipmentType, int> equipmentSlotSizes = new Dictionary<ItemEquipmentType, int>();

    Dictionary<int, ItemLocation> itemSlotLocations = new Dictionary<int, ItemLocation>();

    Dictionary<string, float> localAttributes = new Dictionary<string, float>();

    public class MovedItemSlotArgs : EventArgs
    {
        public SItemSlot movedItemSlot;
    }
    private MovedItemSlotArgs movedItemSlot = new MovedItemSlotArgs();

    public event EventHandler<MovedItemSlotArgs> ItemAddedToInventory;
    public event EventHandler<MovedItemSlotArgs> ItemRemovedFromInventory;
    public event EventHandler<MovedItemSlotArgs> ItemAddedToEquipment;
    public event EventHandler<MovedItemSlotArgs> ItemRemovedFromEquipment;

    private void Awake()
    {
        foreach (ItemInventoryType itemInventoryType in Enum.GetValues(typeof(ItemInventoryType)))
        {
            inventorySlots[itemInventoryType] = new Dictionary<int, SItemSlot>();
        }
        foreach (ItemEquipmentType itemEquipmentType in Enum.GetValues(typeof(ItemEquipmentType)))
        {
            equipmentSlots [itemEquipmentType] = new Dictionary<int, SItemSlot>();
        }

        equipmentSlotSizes[ItemEquipmentType.PrimaryWeapon] = 10;
        equipmentSlotSizes[ItemEquipmentType.SecondaryWeapon] = 2;
        equipmentSlotSizes[ItemEquipmentType.Generator] = 15;
        equipmentSlotSizes[ItemEquipmentType.Extra] = 1;
    }

    public bool HasItem(int itemSlotID)
    {
        return itemSlotLocations.ContainsKey(itemSlotID);
    }
    public bool HasItem(SItemSlot itemSlot)
    {
        return itemSlotLocations.ContainsKey(itemSlot.slotID);
    }

    public class ItemLocationArgs : EventArgs 
    {
        public bool hasItem = false;
        public ItemLocation itemLocation = ItemLocation.InInventory;
    }

    public ItemLocationArgs FindItem(int itemSlotID)
    {
        ItemLocationArgs itemLocation = new ItemLocationArgs();

        itemLocation.hasItem = itemSlotLocations.ContainsKey(itemSlotID);
        
        if(itemLocation.hasItem)
        {
            itemLocation.itemLocation = itemSlotLocations[itemSlotID];
        }

        return itemLocation;
    }

    public ItemLocationArgs FindItem(SItemSlot itemSlot)
    {
        ItemLocationArgs itemLocation = new ItemLocationArgs();

        itemLocation.hasItem = itemSlotLocations.ContainsKey(itemSlot.slotID);

        if (itemLocation.hasItem)
        {
            itemLocation.itemLocation = itemSlotLocations[itemSlot.slotID];
        }

        return itemLocation;
    }

    private int CreateNewID()
    {
        if(itemSlotLocations.Keys.Count > 0)
        {
            return itemSlotLocations.Keys.Max() + 1;
        }
        else
        { 
            return 0; 
        }
    }

    public bool AddItemToInventory(SItem item)
    {
        SItem newItem = Instantiate(item);

        SItemSlot itemSlot = ScriptableObject.CreateInstance<SItemSlot>();
        itemSlot.slotID = CreateNewID();
        itemSlot.item = newItem;

        ItemInventoryType itemInventoryType = newItem.itemInventoryType;

        Dictionary<int, SItemSlot> inventorySlotsRef = inventorySlots[itemInventoryType];

        inventorySlotsRef[itemSlot.slotID] = itemSlot;
        inventorySlots[itemInventoryType] = inventorySlotsRef;

        itemSlotLocations[itemSlot.slotID] = ItemLocation.InInventory;

        movedItemSlot.movedItemSlot = itemSlot;
        ItemAddedToInventory?.Invoke(this, movedItemSlot);

        return true;
    }

    private bool AddItemToInventory(SItemSlot itemSlot)
    {
        ItemInventoryType itemInventoryType = itemSlot.item.itemInventoryType;

        Dictionary<int, SItemSlot> inventorySlotsRef = inventorySlots[itemInventoryType];

        inventorySlotsRef[itemSlot.slotID] = itemSlot;
        inventorySlots[itemInventoryType] = inventorySlotsRef;

        itemSlotLocations[itemSlot.slotID] = ItemLocation.InInventory;

        movedItemSlot.movedItemSlot = itemSlot;
        ItemAddedToInventory?.Invoke(this, movedItemSlot);

        return true;
    }

    public bool SendItemToEquipment(SItemSlot itemSlot)
    {
        if(AddItemToEquipment(itemSlot))
        {
            ItemInventoryType itemInventoryType = itemSlot.item.itemInventoryType;

            Dictionary<int, SItemSlot> inventorySlotsRef = inventorySlots[itemInventoryType];

            inventorySlotsRef.Remove(itemSlot.slotID);
            inventorySlots[itemInventoryType] = inventorySlotsRef;

            movedItemSlot.movedItemSlot = itemSlot;
            ItemRemovedFromInventory?.Invoke(this, movedItemSlot);

            return true;
        }
        return false;
    }

    private bool AddItemToEquipment(SItemSlot itemSlot)
    {
        int equipmentSlotSize = equipmentSlotSizes[itemSlot.item.itemEquipmentType];
        int equipmentSlotCount = equipmentSlots[itemSlot.item.itemEquipmentType].Keys.Count;

        if(equipmentSlotCount < equipmentSlotSize)
        {
            ItemEquipmentType itemEquipmentType = itemSlot.item.itemEquipmentType;
            Dictionary<int, SItemSlot> equipmentSlotsRef = equipmentSlots[itemEquipmentType];

            equipmentSlotsRef[itemSlot.slotID] = itemSlot;
            equipmentSlots[itemEquipmentType] = equipmentSlotsRef;

            itemSlotLocations[itemSlot.slotID] = ItemLocation.InEquipment;

            movedItemSlot.movedItemSlot = itemSlot;
            ItemAddedToEquipment?.Invoke(this, movedItemSlot);

            EquipmentChanged();

            return true;
        }

        return false;
    }

    public bool SendItemToInventory(SItemSlot itemSlot)
    {
        if (AddItemToInventory(itemSlot))
        {
            ItemEquipmentType itemEquipmentType = itemSlot.item.itemEquipmentType;
            Dictionary<int, SItemSlot> equipmentSlotsRef = equipmentSlots[itemEquipmentType];

            equipmentSlotsRef.Remove(itemSlot.slotID);
            equipmentSlots[itemEquipmentType] = equipmentSlotsRef;

            movedItemSlot.movedItemSlot = itemSlot;
            ItemRemovedFromEquipment?.Invoke(this, movedItemSlot);

            EquipmentChanged();

            return true;
        }
        return false;
    }

    private void EquipmentChanged()
    {
        localAttributes.Clear();
        foreach(ItemEquipmentType itemEquipmentType in equipmentSlots.Keys) 
        {
            foreach(int slotID in equipmentSlots[itemEquipmentType].Keys)
            {
                foreach(Attribute attribute in equipmentSlots[itemEquipmentType][slotID].item.attributes)
                {
                    float currentValue = 0f;

                    if (localAttributes.ContainsKey(attribute.name))
                    {
                        currentValue = localAttributes[attribute.name];
                    }

                    currentValue += attribute.value;

                    localAttributes[attribute.name] = currentValue;
                }
            }
        }

        GetComponent<AttributesContainerComponent>().AddAttributeField("Equipments", localAttributes);
    }
}