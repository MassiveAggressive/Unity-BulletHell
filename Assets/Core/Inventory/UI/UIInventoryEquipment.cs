using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryEquipment : MonoBehaviour
{
    [SerializeField] InventoryEquipmentComponent inventoryEquipment;

    [Header("Inventory")]
    [SerializeField] TMP_Dropdown inventoryDropdown;
    [SerializeField] UIPanelSwitcher inventoryPanelSwitcher;

    [SerializeField] Dictionary<ItemInventoryType, UIInventoryPanel> UIInventoryPanels = new Dictionary<ItemInventoryType, UIInventoryPanel>();
    
    [SerializeField] GameObject inventoryPanelPrefab;
    [SerializeField] Transform UIInventoryPanelObjectParent;

    Dictionary<ItemInventoryType, List<UIItemSlot>> inventorySlots = new Dictionary<ItemInventoryType, List<UIItemSlot>>();

    [SerializeField] Dictionary<ItemEquipmentType, UIEquipmentPanel> UIEquipmentPanels = new Dictionary<ItemEquipmentType, UIEquipmentPanel>();
    [Header("Equipment")]
    [SerializeField] GameObject equipmentPanelPrefab;
    [SerializeField] Transform UIEquipmentPanelContainerPrefab;
    [SerializeField] Transform UIEquipmentPanelObjectParent;

    Dictionary<ItemEquipmentType, List<UIItemSlot>> equipmentSlots = new Dictionary<ItemEquipmentType, List<UIItemSlot>>();

    private void Awake()
    {
        inventoryDropdown.ClearOptions();

        inventoryDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    public void OnDropdownValueChanged(int value)
    {
        inventoryPanelSwitcher.SetCurrentPanel(value);
    }

    private void Start()
    {
        foreach (ItemInventoryType itemInventoryType in Enum.GetValues(typeof(ItemInventoryType)))
        {
            GameObject UIInventoryPanelObject = Instantiate(inventoryPanelPrefab);
            UIInventoryPanelObject.transform.SetParent(UIInventoryPanelObjectParent, false);
            UIInventoryPanelObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            UIInventoryPanels[itemInventoryType] = UIInventoryPanelObject.GetComponent<UIInventoryPanel>();

            inventoryDropdown.options.Add(new TMP_Dropdown.OptionData(itemInventoryType.ToString()));
        }

        foreach (ItemEquipmentType itemEquipmentType in inventoryEquipment.equipmentSlotSizes.Keys)
        {
            GameObject UIEquipmentPanelObject = Instantiate(equipmentPanelPrefab);
            Transform UIEquipmentPanelContainerObject = Instantiate(UIEquipmentPanelContainerPrefab);
            UIEquipmentPanelObject.GetComponent<UIEquipmentPanel>().SetPanelType((byte)itemEquipmentType);
            UIEquipmentPanelContainerObject.GetComponent<UIExpandableArea>().SetTitleText(itemEquipmentType.ToString());

            UIEquipmentPanelContainerObject.transform.SetParent(UIEquipmentPanelObjectParent);
            UIEquipmentPanelObject.transform.SetParent(UIEquipmentPanelContainerObject.transform.GetChild(1).transform);
            UIEquipmentPanels[itemEquipmentType] = UIEquipmentPanelObject.GetComponent<UIEquipmentPanel>();
        }

        foreach (ItemInventoryType itemInventoryType in Enum.GetValues(typeof(ItemInventoryType)))
        {
            List<UIItemSlot> itemSlots = UIInventoryPanels[itemInventoryType].CreateSlots(25);

            inventorySlots[itemInventoryType] = itemSlots;
            
            foreach(UIItemSlot itemSlot in itemSlots)
            {
                itemSlot.UIItemSlotClicked += UIItemSlotClicked;
            }
        }

        foreach(ItemEquipmentType itemEquipmentType in inventoryEquipment.equipmentSlotSizes.Keys)
        {
            List<UIItemSlot> itemSlots = UIEquipmentPanels[itemEquipmentType].CreateSlots(inventoryEquipment.equipmentSlotSizes[itemEquipmentType]);
            equipmentSlots[itemEquipmentType] = itemSlots;

            foreach (UIItemSlot itemSlot in itemSlots)
            {
                itemSlot.UIItemSlotClicked += UIItemSlotClicked;
            }
        }

        inventoryEquipment.ItemAddedToInventory += AddItemToInventory;
        inventoryEquipment.ItemRemovedFromInventory += ItemRemovedFromInventory;
        inventoryEquipment.ItemAddedToEquipment += ItemAddedToEquipment;
        inventoryEquipment.ItemRemovedFromEquipment += ItemRemovedFromEquipment;
    }

    private void UIItemSlotClicked(object sender, System.EventArgs e)
    {
        UIItemSlot itemSlot = (UIItemSlot)sender;

        InventoryEquipmentComponent.ItemLocationArgs itemLocation = inventoryEquipment.FindItem(itemSlot.ItemSlot);

        if(itemLocation.hasItem)
        {
            switch(itemLocation.itemLocation) 
            {
                case ItemLocation.InInventory:
                    inventoryEquipment.SendItemToEquipment(itemSlot.ItemSlot);
                    break;
                case ItemLocation.InEquipment:
                    inventoryEquipment.SendItemToInventory(itemSlot.ItemSlot);
                    break;
            }
        }
    }

    private void AddItemToInventory(object sender, InventoryEquipmentComponent.MovedItemSlotArgs e)
    {
        List<UIItemSlot> inventorySlotsRef = inventorySlots[e.movedItemSlot.item.itemInventoryType];

        foreach(UIItemSlot itemSlot in inventorySlotsRef)
        {
            if(itemSlot.IsAvailable)
            {
                itemSlot.SetItemSlot(e.movedItemSlot);
                break;
            }
        }
    }

    private void ItemRemovedFromInventory(object sender, InventoryEquipmentComponent.MovedItemSlotArgs e)
    {
        List<UIItemSlot> inventorySlotsRef = inventorySlots[e.movedItemSlot.item.itemInventoryType];

        foreach (UIItemSlot itemSlot in inventorySlotsRef)
        {
            if(itemSlot.ItemSlot)
            {
                if (itemSlot.ItemSlot.slotID == e.movedItemSlot.slotID)
                {
                    itemSlot.SetItemSlot(null);
                    break;
                }
            }
        }
    }

    private void ItemAddedToEquipment(object sender, InventoryEquipmentComponent.MovedItemSlotArgs e)
    {
        List<UIItemSlot> equipmentSlotsRef = equipmentSlots[e.movedItemSlot.item.itemEquipmentType];

        foreach (UIItemSlot itemSlot in equipmentSlotsRef)
        {
            if (itemSlot.IsAvailable)
            {
                itemSlot.SetItemSlot(e.movedItemSlot);
                break;
            }
        }
    }

    private void ItemRemovedFromEquipment(object sender, InventoryEquipmentComponent.MovedItemSlotArgs e)
    {
        List<UIItemSlot> equipmentSlotsRef = equipmentSlots[e.movedItemSlot.item.itemEquipmentType];

        foreach (UIItemSlot itemSlot in equipmentSlotsRef)
        {
            if (itemSlot.ItemSlot)
            {
                if (itemSlot.ItemSlot.slotID == e.movedItemSlot.slotID)
                {
                    itemSlot.SetItemSlot(null);
                    break;
                }
            }
        }
    }
}
