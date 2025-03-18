using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Al colisionar con el jugador
        if (other.CompareTag("Player"))
        {
            PlayerLife itemCollector = other.GetComponent<PlayerLife>();

            if (itemCollector != null)
            {
                AudioManagerBehaviour.instance.PlaySFX("Take Item"); //Sonido del item

                itemCollector.TakeItem(); //Recoge el item

                Destroy(gameObject); //Destruye el item después de recogerlo
            }
        }
    }
}
