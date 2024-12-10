using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class BossHandBehaviour : MonoBehaviour
{
    private Animator Animator;
    private bool animatorTriggered = false; // Flag para saber si se activó el trigger
    private bool dañable = false;
    [SerializeField] GameObject objectToSpawn; // El prefab del objeto que quieres instanciar
    private int numberOfObjects = 4; // Cantidad de objetos a instanciar
    private float minX = -10f; // Límite inferior en el eje X
    private float maxX = 10f; // Límite superior en el eje X
    private float fixedY = 0f; // Posición fija en el eje Y
    private float fixedZ = 0f; // Posición fija en el eje Z
    public float minDistance = 4f; // Distancia mínima entre objetos

    private List<Vector3> spawnedPositions = new List<Vector3>(); // Lista de posiciones generadas
    private List<GameObject> spawnedObjects = new List<GameObject>(); // Lista de objetos instanciados

    [SerializeField] float followSpeed = 5f; // Velocidad de seguimiento en el eje X
    [SerializeField] float fallDelay = 2f; // Tiempo antes de que la mano caiga

    [SerializeField] Transform player; // Referencia al jugador
    [SerializeField] GameObject Boss;
    private Animator BossBody;
    [SerializeField] Transform tracker;
    private bool trackea = false;

    void Start()
    {
        Animator = GetComponent<Animator>();
        BossBody = Boss.GetComponent<Animator>();
        StartCoroutine(Fase1());
        BossBody.SetBool("BossAttack", false);
    }

    private void Update()
    {
        while (trackea == true)
        {
            Transform newPosition = tracker.transform;
            newPosition.position = new Vector3(player.position.x * followSpeed, 2, player.position.z);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.CompareTag("Player"))
        {
            //Quitar vida
        }
        else if (other.gameObject.CompareTag("Ataque") && dañable == true)
        {
            Debug.Log("Mano recibe daño");
            animatorTriggered = true;
        }
    }

    public IEnumerator Fase1()
    {
        while (true)
        {
            if (animatorTriggered == true && dañable)
            {
                Animator.SetTrigger("BarridoRecibeDaño");
                animatorTriggered = false; // Resetea el flag.

                yield return new WaitForSeconds(2f);

                dañable = false;
                DestroyAllObjects();
            }

            // Configurar Idle
            Debug.Log("Cambiando a Idle");
            Animator.SetBool("Barrido", false);

            yield return new WaitForSeconds(5f);

            Debug.Log("Cambiando a BarridoMano");
            Animator.SetBool("Barrido", true);

            yield return new WaitForSeconds(1f);

            Debug.Log("Regresando a Idle");
            Animator.SetBool("Barrido", false);

            yield return new WaitForSeconds(5f);

            Debug.Log("Invoca pinchos");
            Animator.SetTrigger("SacarPinchos");
            SpawnObjects();

            yield return new WaitForSeconds(2f);

            // Levantar la mano
            Debug.Log("Activando Levantada");
            Animator.SetTrigger("Levantada");
            dañable = true;

            if (dañable == true)
            {
                    trackea = true;

                    yield return new WaitForSeconds(fallDelay);

                    Animator.SetTrigger("Atacando");
                    trackea = false;

            }

            yield return new WaitForSeconds(5f);

            Animator.SetTrigger("JefeAcerca");

            //StartCoroutine(ReiniciarCourutine());
        }   
    }

    private void SpawnObjects()
    {

        int attempts = 1000; // Número máximo de intentos para encontrar una posición válida

        for (int i = 0; i < numberOfObjects; i++)
        {
            Vector3 randomPosition;
            bool validPosition = false;
            int currentAttempts = 0;

            do
            {
                // Generar una nueva posición aleatoria
                float randomX = Random.Range(minX, maxX);
                randomPosition = new Vector3(randomX, fixedY, fixedZ);

                // Verificar si la posición cumple con la distancia mínima
                validPosition = IsPositionValid(randomPosition);
                currentAttempts++;

            } while (!validPosition && currentAttempts < attempts);

            // Si se encuentra una posición válida, instanciar el objeto
            if (validPosition)
            {
                GameObject newObject = Instantiate(objectToSpawn, randomPosition, Quaternion.identity);
                spawnedPositions.Add(randomPosition); // Guardar la posición
                spawnedObjects.Add(newObject); // Guardar referencia al objeto instanciado
            }
        }
    }

   
    private bool IsPositionValid(Vector3 position)
    {
        foreach (Vector3 spawnedPosition in spawnedPositions)
        {
            if (Vector3.Distance(position, spawnedPosition) < minDistance)
            {
                return false;
            }
        }
        return true;
    }
    public void DestroyAllObjects()
    {
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
        spawnedObjects.Clear(); // Limpiar la lista después de destruir los objetos
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
        var animState = Animator.GetCurrentAnimatorStateInfo(0);
        return animState.IsName(animName) && animState.normalizedTime < 1.0f;
    }

}
