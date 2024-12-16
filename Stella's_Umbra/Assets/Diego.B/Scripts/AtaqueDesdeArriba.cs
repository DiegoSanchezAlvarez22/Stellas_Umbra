using UnityEngine;

public class AtaqueDesdeArriba : MonoBehaviour
{
    [SerializeField] int damage;


    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Destruible"))
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
}
