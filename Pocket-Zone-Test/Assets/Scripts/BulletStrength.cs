using UnityEngine;

public class BulletStrength: MonoBehaviour
{
    [SerializeField] Rigidbody2D _rbBullet;
    [SerializeField] int _bulletDamage = 5;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);

            Health enemyHealth = collision.gameObject.GetComponent<Health>();
            Enemy enemy = enemyHealth.GetComponent<Enemy>();
           
            enemyHealth.ChangeHealth(-_bulletDamage);
            
            if (!enemyHealth.IsAlive())
            {
                enemy.Die();
            }
        }
    }
}
