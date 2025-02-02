using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] Slider _slider;
    [SerializeField] Gradient _gradient;
    [SerializeField] Image _fill;
    [SerializeField] int currentHealth;
    [SerializeField] int maxHealth;

    public void SetMaxHealth(int health)
    {
        _slider.maxValue = health;
        _slider.value = health;

        _fill.color = _gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        _slider.value = health;

        _fill.color = _gradient.Evaluate(_slider.normalizedValue);
    }
    public void ChangeHealth(int amount)
    {
        currentHealth += amount;

        SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
        }
        Debug.Log(currentHealth.ToString());
    }

    public bool IsAlive()
    {
        if(currentHealth <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
