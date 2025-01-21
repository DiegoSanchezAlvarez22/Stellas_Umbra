using System.Threading;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    [Header("BasicAttack")]
    [SerializeField] private GameObject _attackArea;
    [SerializeField] private bool _canAttack;
    [SerializeField] private float _timeAttaking = 0.2f;
    [SerializeField] private float _timeRechargeAttack = 0.6f;
    private bool _isAttacking;
    private float _timer = 0f;
    private float _timerRecharge = 0f;

    void Update()
    {
        #region BasicAttack
        if (Input.GetKeyDown(KeyCode.G) && _canAttack)
        {
            BasicAttack();
        }

        if (!_isAttacking)
        {
            _timerRecharge += Time.deltaTime;

            if (_timerRecharge >= _timeRechargeAttack)
            {
                _canAttack = true;
            }
        }

        if (_isAttacking)
        {
            _timer += Time.deltaTime;

            if (_timer >= _timeAttaking)
            {
                _timer = 0;
                _isAttacking = false;
                _attackArea.SetActive(_isAttacking);
            }
        }
        #endregion
    }

    private void BasicAttack()
    {
        _isAttacking = true;
        _attackArea.SetActive(_isAttacking);

        _canAttack = false;
        _timerRecharge = 0;
    }
}
