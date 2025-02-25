using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BossLife : MonoBehaviour
{
    [SerializeField] private HealthBarBehaviour _healthBarBehaviour;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _damageDuration;
    [SerializeField] private int _expToAdd;
    [SerializeField] Animator Boss;
    private SpriteRenderer _spriteRenderer;
    private GameObject _playerTarget;

    private void Start()
    {
        _maxHealth = _currentHealth;
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
            Boss.SetTrigger("Muerte");

            //Sonido de muerte
            AudioManagerBehaviour.instance.PlaySFX("Enemies Deaths");

            //Avisar a AtaqueFijado que el enemigo ha muerto
            PlayerAttacks _playerAttacks = FindAnyObjectByType<PlayerAttacks>();
            if (_playerAttacks != null)
            {
                _playerAttacks.RemoveEnemyFromList(this.gameObject);
            }

        }
    }

    private IEnumerator DamageChangeColor()
    {
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(_damageDuration);
        _spriteRenderer.color = Color.white;
    }
}
