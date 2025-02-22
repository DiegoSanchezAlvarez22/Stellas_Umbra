using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    #region Variables
    [SerializeField] float _speed = 10f;
    [SerializeField] int _damage = 2;

    Vector3 _direction;
    #endregion


    public void SetDirection (Vector3 direction)
    {
        _direction = direction.normalized;

        //La bala apunta hacia el enemigo
        transform.forward = _direction;
    }

    void Start()
    {
        //Destruye la bala después de 5 segundos
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        //Mover la bala hacia delante
        transform.position += _direction * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Comprueba si la bala da a un enemigo
        if (other.CompareTag("EnemyFloor") || other.CompareTag("EnemyAir"))
        {
            //Consigue el componente del script EnemyLifes
            EnemyLifes enemyLifes = other.GetComponent<EnemyLifes>();

            if (enemyLifes != null)
            {
                //Hace daño al enemigo
                enemyLifes.DamageRecieved(_damage);
            }

            //Destruye la bala al chocar
            Destroy(gameObject);
        }
    }
}
