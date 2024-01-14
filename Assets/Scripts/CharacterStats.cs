using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] float maxHealth = 100;
    public float power = 10;
    [SerializeField] Slider healthSlider;
    [SerializeField] Animator _animator;
    [SerializeField] GameObject _weaponPrefab;
    [SerializeField] GameObject _goldPrefab;
    [SerializeField] Text _healthTxt;

    [SerializeField] private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        if (_healthTxt != null)
            _healthTxt.text = (currentHealth.ToString()+ " / "+maxHealth.ToString());
    }
    public void ChangeHealth(float value)
    {
        currentHealth += value;
        if (healthSlider != null)
        {
            float fillAmount = currentHealth / maxHealth;
            healthSlider.value = fillAmount;
        }
        if (_healthTxt != null)
        {
            _healthTxt.text = (currentHealth.ToString() + " / " + maxHealth.ToString());
        }
        if (transform.CompareTag("Player"))
        {
            if (_animator != null)
                _animator.SetTrigger("isHit");
        }
        else if (transform.CompareTag("Enemy"))
        {
            AnimationsController animationsController = transform.GetComponent<AnimationsController>();

            if (animationsController != null)
            {
                animationsController.Hit();
            }
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (transform.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if (transform.CompareTag("Enemy"))
        {
            AnimationsController animationsController = transform.GetComponent<AnimationsController>();
            if (animationsController != null)
            {
                animationsController.SetDead();
            }
        }
        if (gameObject.layer == LayerMask.NameToLayer("Mineral"))
        {
            gameObject.SetActive(false);
        }
    }

    public void OnDead()
    {
        Destroy(gameObject,0.1f);
        if (_weaponPrefab != null)
            Instantiate(_weaponPrefab, transform.position, Quaternion.identity);
        for (int i = 0; i < 3; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-2f, 2f), Random.Range(0, 2f), 0f); // Adjust the range as needed
            Vector3 spawnPosition = transform.position + randomOffset;

            Instantiate(_goldPrefab, spawnPosition, Quaternion.identity);
        }
    }

    public float GetPower()
    {
        return power;
    }
    public void SetMaxHealth(float value)
    {
        maxHealth += value;
        currentHealth = maxHealth;
        if (healthSlider != null)
        {
            float fillAmount = currentHealth / maxHealth;
            healthSlider.value = fillAmount;
        }
        if (_healthTxt != null)
        {
            _healthTxt.text = (currentHealth.ToString() + " / " + maxHealth.ToString());
        }
    }
}
