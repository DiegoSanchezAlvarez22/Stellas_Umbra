using UnityEngine;

public class BossManager : MonoBehaviour
{
    public static BossManager Instance;

    [SerializeField] GameObject _currentBossInstance;
    [SerializeField] PlayerLife _playerLife;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetBossInstance(GameObject bossInstance)
    {
        _currentBossInstance = bossInstance;
    }

    public void DestroyBossInstance()
    {
            Destroy(_currentBossInstance);
            Debug.Log("Boss destruido porque el jugador murió.");
    }
}
