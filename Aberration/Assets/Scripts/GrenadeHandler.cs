using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeHandler : AutoDestroyPoolableObject
{
    [SerializeField] GameObject particlesPrefab;
    [SerializeField] GameObject fallingGrenadeVisual;
    [SerializeField] float explosionDelay = 5f;

    public override void OnEnable()
    {
        base.OnEnable();
        particlesPrefab.SetActive(false);
        fallingGrenadeVisual.SetActive(true);
        //StartCoroutine(LiveTimeCountdown()); I already do it in AutoDestroyPoolableObject
        StartCoroutine(FallingTimeCountdown());
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Damage")
        {
            //StopCoroutine(LiveTimeCountdown());
            //StartCoroutine(StartExplosion());
            particlesPrefab.SetActive(true);
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ground" || other.gameObject.tag == "Damage")
        {
            //StopCoroutine(LiveTimeCountdown());
            //StartCoroutine(StartExplosion());
            particlesPrefab.SetActive(true);

        }

    }
    //IEnumerator LiveTimeCountdown()
    //{
    //    yield return new WaitForSeconds(explosionDelay);
    //    //Destroy(this.gameObject);  // TO CHANGE TO DISABLE
    //}


    IEnumerator FallingTimeCountdown()
    {
        yield return new WaitForSeconds(0.5f);
        fallingGrenadeVisual.SetActive(false);
    }

    //IEnumerator StartExplosion()
    //{
    //    yield return new WaitForSeconds(2f);
    //    //Destroy(this.gameObject); // TO CHANGE TO DISABLE
    //}

    public override void OnDisable()
    {
        base.OnDisable();
    }
}
