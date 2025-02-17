using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyLifes : MonoBehaviour
{
    [SerializeField] private HealthBarBehaviour _healthBarBehaviour;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _damageDuration;
    [SerializeField] private int _expToAdd;
    private GameObject _playerTarget;
    private SpriteRenderer _spriteRenderer;
    [SerializeField] public GameObject _dyingEffect;

    private void Start()
    {
        _currentHealth = _maxHealth;
        _healthBarBehaviour.UpdateHealthBar(_maxHealth, _currentHealth, _currentHealth);
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _playerTarget = GameObject.Find("Player");
    }

    public void DamageRecieved(float _dmg)
    {
        float _previousHealth = _currentHealth;
        _currentHealth -= _dmg;
        Debug.Log("Vidas actuales enemigo = " + _currentHealth);

        _healthBarBehaviour.UpdateHealthBar(_maxHealth, _currentHealth, _previousHealth);
        Debug.Log("1");

        if (_currentHealth > 0)
        {
            Debug.Log("2");
            StartCoroutine(DamageChangeColor());
        }

        else
        {
            Debug.Log("3");
            _playerTarget.GetComponent<PlayerExpSystem>().AddExp(_expToAdd);
            Instantiate(_dyingEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    private IEnumerator DamageChangeColor()
    {
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(_damageDuration);
        _spriteRenderer.color = Color.white;
    }
}
