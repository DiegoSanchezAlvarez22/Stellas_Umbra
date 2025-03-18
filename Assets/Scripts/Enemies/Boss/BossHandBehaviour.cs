using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHandBehaviour : MonoBehaviour
{
    private Animator _anim;
    private bool _animTriggered = false; // Flag para saber si se activó el trigger
    private bool _damagable = false; //Indica si puede recibir daño
    [SerializeField] GameObject _objToSpawn; // El prefab del objeto que quieres instanciar
    private int _nObj = 4; // Cantidad de objetos a instanciar
    private float _minX = -10f; // Límite inferior en el eje X
    private float _maxX = 10f; // Límite superior en el eje X
    private float _fixedY = 0f; // Posición fija en el eje Y
    private float _fixedZ = 0f; // Posición fija en el eje Z
    public float _minDistance = 4f; // Distancia mínima entre objetos

    private List<Vector3> _spawnedPositions = new List<Vector3>(); // Lista de posiciones generadas
    private List<GameObject> _spawnedObj = new List<GameObject>(); // Lista de objetos instanciados

    [SerializeField] float _fallDelay = 2f; // Tiempo antes de que la mano caiga

    [SerializeField] Transform _player; // Referencia al jugador
    [SerializeField] GameObject _boss;
    private Animator _animBossBody;
    void Start()
    {
        _anim = GetComponent<Animator>();
        _animBossBody = _boss.GetComponent<Animator>();
        StartCoroutine(Fase1());
        _animBossBody.SetBool("BossAttack", false);
    }


    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.CompareTag("Player"))
        {
            //Quitar vida
        }
        else if (other.gameObject.CompareTag("Ataque") && _damagable == true)
        {
            Debug.Log("Mano recibe daño");
            _animTriggered = true;
        }
    }

    public IEnumerator Fase1()
    {
        while (true)
        {
            if (_animTriggered == true)
            {
                _anim.SetTrigger("BarridoRecibeDaño");
                _animTriggered = false; // Resetea el flag.

                yield return new WaitForSeconds(2f);

                _damagable = false;
                DestroyAllObjects();

                yield return new WaitForSeconds(5f);

                _anim.SetTrigger("JefeAcerca");
            }

            // Configurar Idle
            Debug.Log("Cambiando a Idle");
            _anim.SetBool("Barrido", false);

            yield return new WaitForSeconds(5f);

            Debug.Log("Cambiando a BarridoMano");
            _anim.SetBool("Barrido", true);

            yield return new WaitForSeconds(1f);

            Debug.Log("Regresando a Idle");
            _anim.SetBool("Barrido", false);

            yield return new WaitForSeconds(5f);

            Debug.Log("Invoca pinchos");
            _anim.SetTrigger("SacarPinchos");
            SpawnObjects();

            yield return new WaitForSeconds(2f);

            // Levantar la mano
            Debug.Log("Activando Levantada");
            _anim.SetTrigger("Levantada");
            _damagable = true;

            if (_damagable == true)
            {
                _anim.SetBool("Arriba",true);

                yield return new WaitForSeconds(_fallDelay);

                _anim.SetBool("Arriba", false);
                _anim.SetTrigger("Atacando");
            }

            //StartCoroutine(ReiniciarCourutine());
        }   
    }

    private void SpawnObjects()
    {

        int attempts = 1000; // Número máximo de intentos para encontrar una posición válida

        for (int i = 0; i < _nObj; i++)
        {
            Vector3 randomPosition;
            bool validPosition = false;
            int currentAttempts = 0;

            do
            {
                // Generar una nueva posición aleatoria
                float randomX = Random.Range(_minX, _maxX);
                randomPosition = new Vector3(randomX, _fixedY, _fixedZ);

                // Verificar si la posición cumple con la distancia mínima
                validPosition = IsPositionValid(randomPosition);
                currentAttempts++;

            } while (!validPosition && currentAttempts < attempts);

            // Si se encuentra una posición válida, instanciar el objeto
            if (validPosition)
            {
                GameObject newObject = Instantiate(_objToSpawn, randomPosition, Quaternion.identity);
                _spawnedPositions.Add(randomPosition); // Guardar la posición
                _spawnedObj.Add(newObject); // Guardar referencia al objeto instanciado
            }
        }
    }

   
    private bool IsPositionValid(Vector3 position)
    {
        foreach (Vector3 spawnedPosition in _spawnedPositions)
        {
            if (Vector3.Distance(position, spawnedPosition) < _minDistance)
            {
                return false;
            }
        }
        return true;
    }
    public void DestroyAllObjects()
    {
        foreach (GameObject obj in _spawnedObj)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
        _spawnedObj.Clear(); // Limpiar la lista después de destruir los objetos
    }
    private IEnumerator ReiniciarCourutine()
    {
        StopCoroutine(Fase1());

        yield return new WaitForSeconds(1f);

        StartCoroutine(Fase1());
        StopCoroutine(ReiniciarCourutine());
    }
    bool IsPlaying(string animName)
    {
        var animState = _anim.GetCurrentAnimatorStateInfo(0);
        return animState.IsName(animName) && animState.normalizedTime < 1.0f;
    }

}
