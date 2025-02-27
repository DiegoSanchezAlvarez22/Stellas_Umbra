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
    private Animator _bossAnimator;
    private CheckPointSystem _checkPointSystem;
    private InteractableBoss _interactableBoss;
    private SpriteRenderer _spriteRenderer;
    private GameObject _playerTarget;

    public bool isDead = false;
    private Coroutine bossSoundCoroutine;

    private void Start()
    {
        _maxHealth = _currentHealth;
        _healthBarBehaviour.UpdateHealthBar(_maxHealth, _currentHealth, _currentHealth);
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _playerTarget = GameObject.Find("Player");
        _checkPointSystem = _playerTarget.GetComponent<CheckPointSystem>();

        if (gameObject.CompareTag("Boss"))
        {
            _bossAnimator.GetComponent<Animator>();
        }
        else
        {
            _bossAnimator = null;
        }

        if (gameObject.name == "Armature")
        {
            _interactableBoss.GetComponent<InteractableBoss>();
        }
        else
        {
            _interactableBoss = null;
        }
    }

    public void DamageRecieved(float _dmg)
    {
        float _previousHealth = _currentHealth;
        _currentHealth -= _dmg;

        if (gameObject.CompareTag("Boss"))
        {
            AudioManagerBehaviour.instance.PlaySFX("Boss Pain");
        }
        else
        {
            //MI COLEGON EL HULIOOOOO PONME UN RUIDITO WAPO DE COHONES PA LOS ENEMIGOS CHIKITOS
        }

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
                _bossAnimator.SetTrigger("Muerte");
                Debug.Log("Has matado al jefe final");

                isDead = true; //Variable si está muerto o no el Boss (Lo está)

                StartCoroutine(PlayBossDeathSoundDelayed()); //Audio de muerte tras 2 segundos de morir

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

    public void ResetBossLifes()
    {
        //Reseteas la vida del Boss
        _currentHealth = _maxHealth;

        //Actualizas la barra de vida del Boss
        _healthBarBehaviour.UpdateHealthBar(100,100,0);
    }

    public void BossLifeSounds()
    { 
        if (gameObject.CompareTag("Boss"))
        {
            if (bossSoundCoroutine != null)
            {
                StopCoroutine(bossSoundCoroutine);
            }
            bossSoundCoroutine = StartCoroutine(PlayBossSoundLoop());
        }
    }

    public IEnumerator PlayBossSoundLoop()
    {
        while (_currentHealth > 0)
        {
            float healthPercentage = (_currentHealth / _maxHealth) * 100;

            if (healthPercentage >= 50 && healthPercentage <= 100)
            {
                AudioManagerBehaviour.instance.PlaySFX("Boss Laugh");
            }
            else if (healthPercentage > 0 && healthPercentage < 30)
            {
                AudioManagerBehaviour.instance.PlaySFX("Boss Suffering");
            }

            float waitTime = Random.Range(8f,12f);
            yield return new WaitForSeconds(waitTime);
        }
    }

    public void StopBossSoundLoop()
    {
        StopCoroutine(PlayBossSoundLoop());
    }

    private IEnumerator PlayBossDeathSoundDelayed()
    {
        yield return new WaitForSeconds(1f);
        AudioManagerBehaviour.instance.PlaySFX("Boss Death");
    }
}
