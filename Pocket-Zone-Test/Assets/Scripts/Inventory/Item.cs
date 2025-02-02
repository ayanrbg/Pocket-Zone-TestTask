using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName; // �������� ��������
    public int maxStack = 20; // ������������ ���������� � �����
    public int currentAmount; // ������� ����������

    public Item(string name, int amount)
    {
        itemName = name;
        currentAmount = amount;
    }

    public bool CanStack(int amount)
    {
        return currentAmount + amount <= maxStack;
    }

    public void AddToStack(int amount)
    {
        currentAmount = Mathf.Min(currentAmount + amount, maxStack);
    }
}
