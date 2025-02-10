using UnityEngine;

public class PlatformBehaviour : MonoBehaviour
{
    private BoxCollider _boxCollider;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Invoke("DisableTrigger", 1);
        }
    }

    private void DisableTrigger()
    {
        _boxCollider.isTrigger = true;
    }
}
