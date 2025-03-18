using UnityEngine;

public class PlatformBehaviour : MonoBehaviour
{
    private BoxCollider _boxCollider;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
            
    //    }
    //}

    private void EnableTrigger()
    {
        _boxCollider.isTrigger = true;
    }

    private void DisableTrigger()
    {
        _boxCollider.isTrigger = false;
    }
}
