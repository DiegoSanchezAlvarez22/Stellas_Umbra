using UnityEngine;

public class PlayerExpSystem : MonoBehaviour
{
    [SerializeField] private int _currentExp = 0;

    public void AddExp(int _newExp)
    {
        _currentExp += _newExp;
        Debug.Log("Exp aumentada a: " + _currentExp);
    }

    public void SubtractExp(int _expLost)
    {
        _currentExp -= _expLost;
        Debug.Log("Exp diasminuida a: " + _currentExp);
    }
}
