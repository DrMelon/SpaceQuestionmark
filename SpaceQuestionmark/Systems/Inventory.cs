using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace SpaceQuestionmark.Systems
{
    public class Inventory
    {
        public Dictionary<int, Entities.Item> mySlots;
        public int maxSlots = 10;
        public float maxSize = 100;
        public float currentSize = 0;

        public Inventory()
        {

        }

        public void Initialise(int newMaxSlots, int newMaxSize)
        {
            maxSlots = newMaxSlots;
            maxSize = newMaxSize;

            mySlots.Clear();
            for(int i = 0; i < maxSize; i++)
            {
                mySlots.Add(i, null);
            }
        }

        public int GetFirstEmpty()
        {
            var emptySlots = mySlots.Where(slot => slot.Value == null);
            if(emptySlots.Count() > 0)
            {
                return emptySlots.First().Key;
            }

            return -1;
        }

        public int GetSlotOfItem(Entities.Item itemToGet)
        {
            foreach(var kvp in mySlots)
            {
                if(kvp.Value == itemToGet)
                {
                    return kvp.Key;
                }
            }

            return -1;
        }

        public Entities.Item GetItemInSlot(int slotNum)
        {
            if(slotNum >= maxSlots)
            {
                return null;
            }

            Entities.Item item = mySlots[slotNum];
            return item;
        }

        public bool AddItem(Entities.Item itemToAdd)
        {
            int emptySlot = GetFirstEmpty();

            if (emptySlot < 0)
            {
                return false;
            }
            else if(itemToAdd.mySize + currentSize > maxSize)
            {
                return false;
            }
            else
            {
                mySlots[emptySlot] = itemToAdd;
                itemToAdd.parentInventory = this;
                return true;
            }
        }

        public bool RemoveItem(Entities.Item itemToRemove)
        {
            int slotOfItem = GetSlotOfItem(itemToRemove);
            if (slotOfItem < 0)
            {
                return false;
            }
            else
            {
                mySlots.Remove(slotOfItem);
                itemToRemove.parentInventory = null;
                currentSize -= itemToRemove.mySize;
                return true;
            }
        }
    }
}
