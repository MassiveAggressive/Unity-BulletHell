using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEquipmentPanel : UIInventoryEquipmentPanel
{
    [SerializeField] ItemEquipmentType panelEquipmentType = ItemEquipmentType.None;

    public override void SetPanelType(byte type)
    {
        panelEquipmentType = (ItemEquipmentType)type;
    }

    public override List<UIItemSlot> CreateSlots(int slotAmount)
    {
        for(int i = 0; i < slotAmount; i++) 
        {
            GameObject slotInstance = Instantiate(UIItemSlotPrefab);
            slots.Add(slotInstance.GetComponent<UIItemSlot>());

            slotInstance.transform.SetParent(transform, false);
        }

        return slots;
    }
}
