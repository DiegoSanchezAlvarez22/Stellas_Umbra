using System.Collections;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    [SerializeField] GameObject Boss;
    private Animator Cuerpo;
    [SerializeField] GameObject Corazón;
    private SphereCollider heart;
    [SerializeField] float bossHealth = 100f;
    private int currentPhase;

    [Header("Ataque")]
    [SerializeField] GameObject Mano;
    private Behaviour BossHandBehaviour;
    private Animator mano;


    private void Start()
    {
        Cuerpo = GetComponent<Animator>();
        heart = Corazón.GetComponent<SphereCollider>();
        BossHandBehaviour = Mano.GetComponent<BossHandBehaviour>();
        mano = Mano.GetComponent<Animator>();
        Cuerpo.SetBool("BossAttack", false);
    }

    private void Update()
    {
        if (IsPlaying("BossHandAtacaIdle"))
        {
            Cuerpo.SetTrigger("Attack");
            Cuerpo.SetBool("BossAttack", true);
        }
        //if (bossHealth <= 75 && currentPhase < 2)
        //{
        //    currentPhase = 2;
        //    StopAllCoroutines();
        //    StartCoroutine(Fase2());
        //}
        //else if (bossHealth <= 50 && currentPhase < 3)
        //{
        //    currentPhase = 3;
        //    StopAllCoroutines();
        //    StartCoroutine(Fase3());
        //}
        //else if(bossHealth <= 25 && currentPhase < 4)
        //{
        //    currentPhase = 4;
        //    StopAllCoroutines();
        //    StartCoroutine(Fase4());
        //}
        //else if(bossHealth == 0 && currentPhase == 4)
        //{
        //    StopAllCoroutines();
        //    Cuerpo.Play("Muerte");
        //}
    }

    private void OnTriggerEnter(Collider heart)
    {
       if (heart.CompareTag("Ataque"))
       {
            Debug.Log("CorazonDaño");
            bossHealth = bossHealth - 25;
       }
    }
    bool IsPlaying(string animName)
    {
        var animState = mano.GetCurrentAnimatorStateInfo(0);
        return animState.IsName(animName) && animState.normalizedTime < 1.0f;
    }

}
