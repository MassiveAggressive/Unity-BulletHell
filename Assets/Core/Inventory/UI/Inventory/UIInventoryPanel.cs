using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class UIInventoryPanel : UIInventoryEquipmentPanel
{
    [SerializeField] ItemInventoryType panelInventoryType = ItemInventoryType.None;

    public override void SetPanelType(byte type)
    {
        panelInventoryType = (ItemInventoryType)type;
    }

    public override List<UIItemSlot> CreateSlots(int slotAmount)
    {
        for (int i = 0; i < slotAmount; i++)
        {
            GameObject slotInstance = Instantiate(UIItemSlotPrefab);
            slots.Add(slotInstance.GetComponent<UIItemSlot>());

            slotInstance.transform.SetParent(parentGridLayoutGroup, false);
        }

        return slots;
    }
}
