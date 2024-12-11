using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors.Visuals;

public class GrenadeThrower : MonoBehaviour
{
    [SerializeField] Transform grenadePrefab;
    [SerializeField] float throwForce = 40f;
    [SerializeField] CurveVisualController visualController;

    private void Awake()
    {
        visualController = GetComponent<CurveVisualController>();
    }

    public void ThrowGrenade()
    {

    }
}
