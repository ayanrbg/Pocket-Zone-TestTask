using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 10; // ���������� �������� ��� �������
   
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Inventory inventory = other.GetComponent<Inventory>();
            if (inventory != null)
            {
                bool added = inventory.AddItem("Ammo", ammoAmount);
                
                if (added)
                {
                    Destroy(gameObject); // ������� ������ ����� �������
                }
            }
        }
    }
    
}
