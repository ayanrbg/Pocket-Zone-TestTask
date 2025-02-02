using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    #region Переменные
    [SerializeField] Joystick joystick; 
    [SerializeField] Transform _playerTransform;
    [SerializeField] Inventory inventory;

    [SerializeField] float _timeToDestroy = 2; // Время действия (дальность) пули
    [SerializeField] float _bulletSpeed = 10f;
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] Transform _shootPoint; // Точка, откуда вылетают пули
    
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
            Debug.LogError("Inventory не найден!");
            return;
        }
        FindObjectOfType<InventoryUI>().UpdateUI();
    }

    void Update()
    {
        Vector2 inputDirection = new Vector2(joystick.Horizontal, joystick.Vertical);

        if (inputDirection.magnitude > 0.1f) // Если джойстик используется
        {
            _lastShootDirection = inputDirection.normalized; // Запоминаем направление
            FlipPlayer(_lastShootDirection.x);
        }
        if (Input.GetKeyDown(KeyCode.Space)) {  // Пробел в десктопной версии стреляет
            Shoot();
        }
    }
    public void Shoot()
    {
        if (inventory == null)
        {
            Debug.LogError("Inventory не найден!");
            return;
        }

        if (!HasAmmo())
        {
            Debug.Log("Нет патронов!");
            return;
        }
        if (!HasAmmo()) return;

        GameObject bullet = Instantiate(_bulletPrefab, _shootPoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        
        Destroy(bullet, _timeToDestroy);

        if (rb != null)
        {
            rb.velocity = _lastShootDirection * _bulletSpeed; // Пуля летит в последнем направлении
        }
        inventory.UseAmmo(1);

        FindObjectOfType<InventoryUI>().UpdateUI();

        _audioSource.PlayOneShot(_shootSound);
    }
    private bool HasAmmo()
    {
        return inventory.GetAmmoCount() > 0; // Проверяем, есть ли патроны
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

        // Разворачиваем игрока (меняем масштаб по X)
        Vector3 newScale = _playerTransform.localScale;
        newScale.x *= -1;
        _playerTransform.localScale = newScale;
    }   
}
