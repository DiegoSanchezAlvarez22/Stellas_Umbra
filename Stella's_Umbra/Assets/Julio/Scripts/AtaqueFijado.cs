using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class AtaqueFijado : MonoBehaviour
{
    #region Variables
    [SerializeField] SphereCollider _detectionCollider;
    [SerializeField] Image _enemyIndicator;
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] Transform _shootingPoint;

    //Lista para detectar a los enemigos dentro del Sphere Collider
    [SerializeField] List<Collider> _enemiesInside = new List<Collider>();
    #endregion

    void Start()
    {
        _enemyIndicator.gameObject.SetActive(false);
    }


    void Update()
    {
        //Muestra/Oculta la imagen si hay enemigos en la 1º posición o no
        if (_enemiesInside.Count > 0)
        {
            //Habilita la imagen
            _enemyIndicator.gameObject.SetActive(true);

            //Posiciona la imagen en la pantalla encima del enemigo
            PositionIndicator(_enemiesInside[0]);

            //Detectar si pulsas la tecla C
            if (Input.GetKeyDown(KeyCode.C))
            {
                Debug.Log("Has pulsado la tecla C");
                ShootBullet();
            }
        }
        else
        {
            //Deshabilita la imagen
            _enemyIndicator.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyFloor") || other.CompareTag("EnemyAir"))
        { 
            //Añade al enemigo en la lista
            _enemiesInside.Add(other);

            Debug.Log("Ha entrado un enemigo: " + other.gameObject.name);
            Debug.Log("Total de enemigos dentro: " + _enemiesInside.Count);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("EnemyFloor") || other.CompareTag("EnemyAir"))
        {
            //Elimina al enemigo de la lista
            _enemiesInside.Remove(other);

            Debug.Log("Ha salido un enemigo: " + other.gameObject.name);
            Debug.Log("Total de enemigos dentro: " + _enemiesInside.Count);
        }
    }

    private void PositionIndicator(Collider enemy)
    {
        //Convertir la posición del enemigo en una posición de la pantalla
        Vector3 screenPos = Camera.main.WorldToScreenPoint(enemy.transform.position);

        //Ajusta la posición de la imagen un poco por encima
        screenPos.y += 90f;
        _enemyIndicator.transform.position = screenPos;
    }

    private void ShootBullet()
    {
        if (_bulletPrefab != null && _shootingPoint != null && _enemiesInside.Count > 0)
        {
            //Obtiene al primer enemigo de la lista
            Transform targetEnemy = _enemiesInside[0].transform;

            //Calcula la dirección hacia el enemigo
            Vector3 direction = (targetEnemy.position - _shootingPoint.position).normalized;

            //Instancia la bala
            GameObject bulletInstance = Instantiate(_bulletPrefab, _shootingPoint.position, _shootingPoint.rotation);

            //Configura la dirección de la bala
            BulletBehaviour _bulletBehaviour = bulletInstance.GetComponent<BulletBehaviour>();

            if (_bulletBehaviour != null)
            {
                _bulletBehaviour.SetDirection(direction);
            }
            Debug.Log("Se ha disparado la bala");
        }
        else
        {
            Debug.LogWarning("Falta asignar el prefab de la bala o el shootingPoint al Inspector");
        }
    }

    public void RemoveEnemyFromList(GameObject enemy)
    {
        //Buscar el collider del enemigo en la lista y eliminarlo
        for (int i = 0; i < _enemiesInside.Count; i++)
        {
            if (_enemiesInside[i].gameObject == enemy)
            {
                _enemiesInside.RemoveAt(i);
                break;
            }
        }

        //Si no quedan enemigos, ocultar la imagen
        if (_enemiesInside.Count == 0)
        {
            _enemyIndicator.gameObject.SetActive(false);
        }
    }
}
