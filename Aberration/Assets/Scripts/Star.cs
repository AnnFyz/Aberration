using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    [SerializeField] float amplitude = 0.5f;
    [SerializeField] float speed = 0.5f;

    void Update()
    {
        transform.Translate(new Vector3(0, 0, Mathf.Sin(Time.time) * amplitude) * speed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            QuestManager.Instance.AmountOfCollectedStars++;
            AudioManager.Instance.PlaySound("Pickup");
            Destroy(this.gameObject);
        }
       
    }
}
