using System.Collections.Generic;

public class Inventory
{
    public List<InventorySlot> slots = new List<InventorySlot>();
    public int numSlots;

    public Inventory(int numSlots)
    {
        this.numSlots = numSlots;
        for (int i = 0; i < numSlots; i++)
        {
            slots.Add(new InventorySlot());
        }
    }

    public bool AddItem(Item itemToAdd)
    {
        // Find an empty slot or a slot with the same item type to stack on
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].item = itemToAdd;
                slots[i].quantity = 1;
                return true;
            }
            else if (slots[i].item.itemID == itemToAdd.itemID && slots[i].quantity < slots[i].item.maxStack)
            {
                slots[i].quantity++;
                return true;
            }
        }

        return false;
    }

    public void RemoveItem(Item itemToRemove)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].item == itemToRemove)
            {
                slots[i].quantity--;
                if (slots[i].quantity == 0)
                {
                    slots[i].item = null;
                }
                break;
            }
        }
    }

    public void Clear()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].item = null;
            slots[i].quantity = 0;
        }
    }
}
