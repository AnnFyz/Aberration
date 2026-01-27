using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartAnimation : MonoBehaviour
{
    [SerializeField] Color lerpedColor_1;
    [SerializeField] Color lerpedColor_2;
    [SerializeField] Color currentColor;
    [SerializeField] Image uiHealth;
    [SerializeField] float time_c = 0.5f;
    [SerializeField] float time_s = 0.5f;
    [SerializeField] Transform heart;
    private void Start()
    {
        uiHealth = GetComponent<Image>();
    }
    void Update()
    {
        currentColor = Color.Lerp(lerpedColor_1, lerpedColor_2, Mathf.PingPong(Time.time* time_c, 1));
        heart.localScale = Vector3.Lerp(new Vector3(0.45f,0.45f,0.45f), new Vector3(0.35f, 0.35f, 0.35f), Mathf.PingPong(Time.time * time_s, 1));
        uiHealth.color = currentColor;
    }

}
