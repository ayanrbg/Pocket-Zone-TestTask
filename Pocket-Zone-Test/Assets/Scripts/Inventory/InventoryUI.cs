using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    #region Переменные
    public Inventory inventory; // Ссылка на инвентарь игрока
    public GameObject slotPrefab; // Префаб слота
    public Transform slotParent; // Родительский объект для слотов
    private List<GameObject> slots = new List<GameObject>();
    public Sprite ammoSprite;
    public Sprite emptySlotSprite;
    #endregion
    private void Start()
    {
        // Создаем 4 слота в UI
        for (int i = 0; i < inventory.slotCount; i++)
        {
            GameObject slot = Instantiate(slotPrefab, slotParent);
            slots.Add(slot);
        }
        UpdateUI();
    }

    public void UpdateUI()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            Image slotImage = slots[i].GetComponent<Image>();
            Text amountText = slots[i].GetComponentInChildren<Text>();

            if (i < inventory.slots.Count && inventory.slots[i] != null)
            {
                slotImage.sprite = ammoSprite; // Устанавливаем иконку патронов
                slotImage.color = Color.white; // Заполненный слот
                if (inventory.slots[i].currentAmount > 1)
                {
                    amountText.text = inventory.slots[i].currentAmount.ToString();
                }
                else
                {
                    amountText.text = "";
                }
                
            }
            else
            {
                slotImage.sprite = emptySlotSprite;
                slotImage.color = Color.gray; // Пустой слот
                amountText.text = "";
            }
        }
    }
}
