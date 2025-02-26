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
    [SerializeField] public GameObject _dyingEffect;
    [SerializeField] Animator Boss;
    [SerializeField] CheckPointSystem _checkPointSystem;
    [SerializeField] InteractableBoss _interactableBoss;
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


        if (_currentHealth > 0)
        {

            StartCoroutine(DamageChangeColor());
        }

        else
        {

            _playerTarget.GetComponent<PlayerExpSystem>().AddExp(_expToAdd);
            Instantiate(_dyingEffect, transform.position, transform.rotation);

            //Avisar a AtaqueFijado que el enemigo ha muerto
            PlayerAttacks _playerAttacks = FindAnyObjectByType<PlayerAttacks>();
            if (_playerAttacks != null)
            {
                _playerAttacks.RemoveEnemyFromList(this.gameObject);
            }

            if (gameObject.CompareTag("Boss"))
            {
                Boss.SetTrigger("Muerte");
                Debug.Log("Has matado al jefe final");

                _checkPointSystem.ClearProgress();
                Debug.Log("Borras el progreso al terminar el juego");

                _interactableBoss.HideBossCanvasLife();
                Debug.Log("Se esconde la barra de vida del jefe final"); 
                
            }
            else
            {
                //Sonido de muerte
                AudioManagerBehaviour.instance.PlaySFX("Enemies Deaths");
                Destroy(gameObject);
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
