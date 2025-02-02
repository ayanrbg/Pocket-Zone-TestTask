using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rbEnemy;
    private Transform player;
    [SerializeField] int damageToPlayer;
    [SerializeField] GameObject ammoPrefab;
    [SerializeField] Transform dropPoint;
    [SerializeField] int dropAmount = 10;
    private float speedEnemy = 2;
    public bool isChasing;
    private int _facingDirection = -1;
    
    private void Start()
    {
        rbEnemy = GetComponent<Rigidbody2D>();
    }
    private void LateUpdate()
    {
        if (isChasing == true)
        {
            if (player.position.x > transform.position.x && _facingDirection == -1 || 
                player.position.x < transform.position.x && _facingDirection == 1)
            {
                Flip();
            }
            Vector3 direction = (player.position - transform.position).normalized;
            rbEnemy.velocity = direction * speedEnemy;
            
        }
        else {
            rbEnemy.velocity = Vector2.zero;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(player == null)
            {
                player = collision.transform;
            }
            isChasing = true;
            Debug.Log(isChasing);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            rbEnemy.velocity = Vector2.zero;
            isChasing = false;
            Debug.Log(isChasing);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Health>().ChangeHealth(-damageToPlayer);
        }
    }
    void Flip()
    {
        _facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1,
            transform.localScale.y, transform.localScale.z);
    }
    public void Die()
    {
        DropAmmo();
        Destroy(gameObject);
    }

    private void DropAmmo()
    {
        if (ammoPrefab != null)
        {
            GameObject ammo = Instantiate(ammoPrefab, dropPoint.position, Quaternion.identity);
            ammo.GetComponent<AmmoPickup>().ammoAmount = dropAmount;
        }
    }
}
