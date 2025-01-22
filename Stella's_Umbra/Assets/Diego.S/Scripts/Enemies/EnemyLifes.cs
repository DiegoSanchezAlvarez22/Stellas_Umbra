using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyLifes : MonoBehaviour
{
    [SerializeField] private GameObject _playerTarget;
    [SerializeField] private HealthBarBehaviour _healthBarBehaviour;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _damageDuration;
    [SerializeField] private int _expToAdd;
    private SpriteRenderer _spriteRenderer;
    [SerializeField] public GameObject _dyingEffect;

    private void Start()
    {
        _currentHealth = _maxHealth;
        _healthBarBehaviour.UpdateHealthBar(_maxHealth, _currentHealth, _currentHealth);
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DamageRecieved(float _dmg)
    {
        float _previousHealth = _currentHealth;
        _currentHealth -= _dmg;
        Debug.Log("Vidas actuales enemigo = " + _currentHealth);

        _healthBarBehaviour.UpdateHealthBar(_maxHealth, _currentHealth, _previousHealth);

        if (_currentHealth > 0)
        {
            StartCoroutine(DamageChangeColor());
        }
        else
        {
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
