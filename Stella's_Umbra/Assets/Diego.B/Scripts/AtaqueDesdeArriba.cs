using UnityEngine;

public class AtaqueDesdeArriba : MonoBehaviour
{
    [SerializeField] int damage;
    private GameObject objeto;

    private void OnCollisionEnter(Collision collision)
    {
        objeto = collision.gameObject;

        if (collision.gameObject.CompareTag("Destruible"))
        {
            Destroy(objeto);
            Destroy(this.gameObject);
        }
    }
}
