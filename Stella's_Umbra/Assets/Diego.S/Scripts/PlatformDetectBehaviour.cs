using UnityEngine;

public class PlatformDetectBehaviour : MonoBehaviour
{
    [SerializeField] private PlayerMov _playerMov;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Platform")
        {
            other.isTrigger = false;
            _playerMov._otherCollider = other;
            _playerMov._floorIsPlat = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Platform")
        {
            EnableTrigger(other);
            _playerMov._floorIsPlat = false;
        }
    }

    private void EnableTrigger(Collider other)
    {
        other.isTrigger = true;
    }
}
