using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBehaviour : MonoBehaviour
{
    [SerializeField] private Image _healthBarImage;

    private Quaternion initialRotation;

    void Start()
    {
        //guarda la rotación inicial
        initialRotation = transform.rotation;
    }

    void LateUpdate()
    {
        //mantén la rotación inicial sin importar la rotación del enemigo
        transform.rotation = initialRotation;
    }

    public void UpdateHealthBar(float _maxHealth, float _currentHealth, float _previousHealth)
    {
        float _targetHealth = _currentHealth / _maxHealth;
        _previousHealth = _previousHealth / _maxHealth;
        StartCoroutine(HealthBarAnim(_targetHealth, _previousHealth));
    }

    IEnumerator HealthBarAnim(float _targetHealth, float _previousHealth)
    {
        float _transitionTime = 0.5f;
        float _elapsedTime = 0f;
        while (_elapsedTime < _transitionTime)
        {
            _elapsedTime += Time.deltaTime;
            _healthBarImage.fillAmount = Mathf.Lerp(_previousHealth, _targetHealth, _elapsedTime / _transitionTime);
            yield return null;
        }
        _healthBarImage.fillAmount = _targetHealth;
    }
}
