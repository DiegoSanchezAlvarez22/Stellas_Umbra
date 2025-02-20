using UnityEngine;

public class MapaDesplegable : MonoBehaviour
{
    public Animator mapaAnimator;
    public bool estaAbierto = false;

    private void Start()
    {
        mapaAnimator.Play("MapaCerrado");
    }
    public void AlternarMapa()
    {
        estaAbierto = !estaAbierto;
        if (estaAbierto)
        {
            mapaAnimator.Play("MapaAbierto");
        }
        else
        {
            mapaAnimator.Play("MapaCerrado");
        }
    }
}
