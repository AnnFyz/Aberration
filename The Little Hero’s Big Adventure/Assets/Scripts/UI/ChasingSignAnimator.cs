using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingSignAnimator : MonoBehaviour
{
    [SerializeField] float desiredDuration = 3;
    [SerializeField] float percentageComplete;
    [SerializeField] Vector3 maxScale;
    [SerializeField] Vector3 minScale;
    private void Start()
    {
        minScale = transform.localScale;
        maxScale = transform.localScale + new Vector3(Random.Range(0.015f, 0.03f), Random.Range(0.015f, 0.03f), Random.Range(0.015f, 0.03f));
        
    }
    void Update()
    {
        if(percentageComplete <= 1)
        {
            percentageComplete += desiredDuration * Time.deltaTime;

            transform.localScale = Vector3.Lerp(minScale, maxScale, percentageComplete);  
        }
        else
        {
            Vector3 temp = maxScale;
            maxScale = minScale;
            minScale = temp;
            percentageComplete = 0;
             
        }
        
    }
}
