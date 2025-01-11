using UnityEngine;

public class EnemyLifes : MonoBehaviour
{
    [SerializeField] private int _lifes;

    void Update()
    {
        if (_lifes <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void DamageRecieved(int _dmg)
    {
        _lifes -= _dmg;
        Debug.Log("Vidas actuales enemigo = " + _lifes);
    }
}
