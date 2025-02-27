using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class InteractableBoss : MonoBehaviour
{
    private bool jugadorDentro = false;
    private PlayerInput playerInput;
    [SerializeField] BoxCollider interact;
    [SerializeField] BoxCollider hitBox;
    [SerializeField] Animator Boss;
    [SerializeField] VideoPlayer canvaPostMortem;
    [SerializeField] GameObject sistemaParticulas;
    [SerializeField] GameObject _healthCanvas;
    [SerializeField] CheckPointSystem _checkpointSystem;
    [SerializeField] EnemyLifes _enemyLifes;

    [Header("Icons")]
    [SerializeField] GameObject _bulbIcon;
    [SerializeField] GameObject _playerHearts;
    [SerializeField] GameObject _mapIcon;
    [SerializeField] GameObject _enemyIndicatorIcon;


    private void Start()
    {
        hitBox.enabled = false;
        sistemaParticulas.SetActive(false); //Desactiva completamente el objeto

        RestartBoss();
    }

    private void Update()
    {
        if (Boss.GetCurrentAnimatorStateInfo(0).IsName("Armature|MuerteIdle"))
        {
            interact.enabled = true; //Desactivar el collider después de la interacción
            hitBox.enabled = false;
            StopCoroutine(FasesJefe());
            sistemaParticulas.SetActive(true); //Activa el objeto y las partículas
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = true;
            playerInput = other.GetComponent<PlayerInput>(); //Obtienes el PlayerInput del jugador

            //A
            Boss.SetTrigger("Awake");
            _enemyLifes.BossLifeSounds(); //Llamas a la corroutina de sonidos del boss
            StartCoroutine(FasesJefe());  //Llamas a la corrutina
            interact.enabled = false; //Desactiva el collider después de la interacción

            if (playerInput != null)
            {
                playerInput.actions["Interact"].performed += Interactuar; //Suscribirse al evento
            }
        }
    }

    private void OnTriggerExit(Collider interactuar)
    {
        if (interactuar.CompareTag("Player"))
        {
            jugadorDentro = false;

            if (playerInput != null)
            {
                playerInput.actions["Interact"].performed -= Interactuar; //Desuscribirse del evento
                playerInput = null;
            }
        }
    }

    private void Interactuar(InputAction.CallbackContext context)
    {
        if (!jugadorDentro) return; //Si el jugador no está dentro, no hacer nada

        if (Boss.GetCurrentAnimatorStateInfo(0).IsName("Armature|MuerteIdle"))
        {
            Time.timeScale = 0;

            canvaPostMortem.Play();
            canvaPostMortem.loopPointReached += LoadNextScene;

            _bulbIcon.SetActive(false);
            _playerHearts.SetActive(false);
            _mapIcon.SetActive(false);
            _enemyIndicatorIcon.SetActive(false);

            Debug.Log("Reproduce animación final");
        }
    }

    private void LoadNextScene(VideoPlayer vp)
    {
        SceneManager.LoadScene("Menu Principal");
    }

    private IEnumerator FasesJefe()
    {
        yield return new WaitForSeconds(1f); //Espera el tiempo de la animación
                                             
        hitBox.enabled = true;

        while (true)
        {

            yield return new WaitForSeconds(5f); //Espera el tiempo de la animación

            Boss.SetTrigger("Barrido");

            Invoke(nameof(StartBossAttackSound2), 6f); //Sonido ataque barrido (Va bien)

            yield return new WaitForSeconds(5f); //Espera el tiempo de la animación

            Boss.SetTrigger("Palmada");

            Invoke(nameof(StartBossAttackSound1), 8.6f); //Sonido ataque palmada (Se desincroniza un poco)


            yield return new WaitForSeconds(2f); //Espera el tiempo de la animación

            Boss.SetTrigger("Palmada");

            yield return new WaitForSeconds(10f); //Espera el tiempo de la animación

            Boss.SetTrigger("Barrido");

            Invoke(nameof(StartBossAttackSound2), 6.2f); //Sonido ataque barrido (Este se desincroniza mazo)

            yield return new WaitForSeconds(2f); //Espera el tiempo de la animación

            Boss.SetTrigger("Palmada");
        }
    }

    //Audio del ataque palmada del Jefe final
    private void StartBossAttackSound1()
    {
        AudioManagerBehaviour.instance.PlaySFX("Boss Attack");
    }

    //Audio del ataque barrido del Jefe final
    private void StartBossAttackSound2()
    {
        AudioManagerBehaviour.instance.PlaySFX("Boss Attack 2");
    }

    void OnCollisionEnter(Collision collision)
    {
        //Verifica si el objeto con el que se choca tiene el tag "Enemigo"
        if (collision.gameObject.CompareTag("Floor") && Boss.GetCurrentAnimatorStateInfo(0).IsName("Armature|Palmada"))
        {
            Debug.Log("Hola");
            OndaExpansiva();
        }
    }


    private void OndaExpansiva()
    {

    }

    public void ShowBossCanvasLife()
    {
        _healthCanvas.SetActive(true);
    }
    public void HideBossCanvasLife()
    {
        _healthCanvas.SetActive(false);
    }

    private void RestartBoss()
    {
        RestartAnim();
        _enemyLifes.PlayBossSoundLoop();
    }

    private void RestartAnim()
    {
        Boss.enabled = false;
        Boss.enabled = true;
    }
}
