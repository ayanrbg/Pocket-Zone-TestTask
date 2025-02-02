using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    #region ����������
    [SerializeField] Joystick joystick; 
    [SerializeField] Transform _playerTransform;
    [SerializeField] Inventory inventory;

    [SerializeField] float _timeToDestroy = 2; // ����� �������� (���������) ����
    [SerializeField] float _bulletSpeed = 10f;
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] Transform _shootPoint; // �����, ������ �������� ����
    
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _shootSound;
    
    private bool isFacingRight = true;
    private Vector2 _lastShootDirection = Vector2.right;
    #endregion

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();

        if (inventory == null)
        {
            Debug.LogError("Inventory �� ������!");
            return;
        }
        FindObjectOfType<InventoryUI>().UpdateUI();
    }

    void Update()
    {
        Vector2 inputDirection = new Vector2(joystick.Horizontal, joystick.Vertical);

        if (inputDirection.magnitude > 0.1f) // ���� �������� ������������
        {
            _lastShootDirection = inputDirection.normalized; // ���������� �����������
            FlipPlayer(_lastShootDirection.x);
        }
        if (Input.GetKeyDown(KeyCode.Space)) {  // ������ � ���������� ������ ��������
            Shoot();
        }
    }
    public void Shoot()
    {
        if (inventory == null)
        {
            Debug.LogError("Inventory �� ������!");
            return;
        }

        if (!HasAmmo())
        {
            Debug.Log("��� ��������!");
            return;
        }
        if (!HasAmmo()) return;

        GameObject bullet = Instantiate(_bulletPrefab, _shootPoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        
        Destroy(bullet, _timeToDestroy);

        if (rb != null)
        {
            rb.velocity = _lastShootDirection * _bulletSpeed; // ���� ����� � ��������� �����������
        }
        inventory.UseAmmo(1);

        FindObjectOfType<InventoryUI>().UpdateUI();

        _audioSource.PlayOneShot(_shootSound);
    }
    private bool HasAmmo()
    {
        return inventory.GetAmmoCount() > 0; // ���������, ���� �� �������
    }

    private void FlipPlayer(float directionX)
    {
        if (directionX > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (directionX < 0 && isFacingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;

        // ������������� ������ (������ ������� �� X)
        Vector3 newScale = _playerTransform.localScale;
        newScale.x *= -1;
        _playerTransform.localScale = newScale;
    }   
}
