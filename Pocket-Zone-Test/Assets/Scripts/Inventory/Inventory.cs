using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int slotCount = 4;
    public List<Item> slots = new List<Item>();
    private string savePath;
    public AudioClip _pickupSound;
    public AudioSource _audioSource;

    private void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "inventory.json");
        LoadInventory();
    }
    private void Start()
    {
        // Заполняем слоты пустыми значениями
        for (int i = 0; i < slotCount; i++)
        {
            slots.Add(null);
        }
    }

    public bool AddItem(string itemName, int amount)
    {
        // 1. Проверяем, можно ли добавить в существующий стек
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i] != null && slots[i].itemName == itemName && slots[i].CanStack(amount))
            {
                slots[i].AddToStack(amount);
                SaveInventory();
                FindObjectOfType<InventoryUI>().UpdateUI();
                _audioSource.PlayOneShot(_pickupSound);
                return true;
            }
        }

        // 2. Если нет места в стеках, ищем пустой слот
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = new Item(itemName, amount);
                SaveInventory();
                FindObjectOfType<InventoryUI>().UpdateUI();
                _audioSource.PlayOneShot(_pickupSound);
                return true;
            }
        }

        Debug.Log("Нет места в инвентаре!");
        return false;
    }

    public int GetAmmoCount()
    {
        int totalAmmo = 0;

        foreach (var item in slots)
        {
            if (item != null && item.itemName == "Ammo")
            {
                totalAmmo += item.currentAmount; // Считаем ВСЕ патроны в инвентаре
            }
        }

        Debug.Log("Патроны в инвентаре: " + totalAmmo);
        return totalAmmo;
    }

    public void UseAmmo(int amount)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i] != null && slots[i].itemName == "Ammo")
            {
                int toUse = Mathf.Min(amount, slots[i].currentAmount);
                slots[i].currentAmount -= toUse;
                amount -= toUse;

                if (slots[i].currentAmount <= 0) slots[i] = null; // Удаляем слот, если пустой

                if (amount <= 0) break;
            }
        }
        SaveInventory();
        FindObjectOfType<InventoryUI>().UpdateUI();
    }
    #region Сохранение и загрузка инвентаря
    public void SaveInventory()
    {
        InventoryData data = new InventoryData();
        data.items = slots.FindAll(i => i != null && i.currentAmount > 0); // Только непустые слоты

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Инвентарь сохранен в: " + savePath);
    }
    public void LoadInventory()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            InventoryData data = JsonUtility.FromJson<InventoryData>(json);

            slots = new List<Item>(4);
            foreach (var item in data.items)
            {
                if (item.currentAmount > 0) // 🔹 Добавляем только если патронов > 0
                {
                    slots.Add(new Item(item.itemName, item.currentAmount));
                }
            }
            Debug.Log("Инвентарь загружен");
        }
        else
        {
            Debug.Log("Файл сохранения не найден, создаем новый инвентарь.");
        }
        FindObjectOfType<InventoryUI>().UpdateUI();
    }
    #endregion
}
