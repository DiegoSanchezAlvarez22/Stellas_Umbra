using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    Animator animator;
    [SerializeField] bool isOpen;
    [SerializeField] Animator palancaAnimator; // Referencia al Animator de la palanca
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("Open", false);
    }

    private void Update()
    {
        // Verifica si la booleana "Activada" está en true
        if (palancaAnimator.GetBool("Activada"))
        {
           animator.SetBool("Open", true);
        }
        else if (!palancaAnimator.GetBool("Activada"))
        {
            animator.SetBool("Open", false);
        }

    }

}
