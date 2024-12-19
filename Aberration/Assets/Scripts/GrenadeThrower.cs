using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors.Visuals;

public class GrenadeThrower : MonoBehaviour
{
    [SerializeField] Transform origin;
    [SerializeField] GameObject grenadePrefab;
    [SerializeField] float throwForce = 40f;
    [SerializeField] CurveVisualController visualController;
    [SerializeField] XRInteractorLineVisual lineVisual;
    private void Awake()
    {
        //visualController = GetComponent<CurveVisualController>();
    }

    public void ThrowProjectile()
    {
        visualController.GetLineOriginAndDirection(out Vector3 worldOrigin, out Vector3 worldDirection);
        Debug.DrawRay(worldOrigin, worldDirection);
        GameObject projectile = Instantiate(grenadePrefab, origin.position, origin.rotation);
        projectile.GetComponent<Rigidbody>().AddForce(throwForce * worldDirection);
        Debug.Log("Throw projectile");
    }

    public void ThrowGrenade()
    {
        if (!lineVisual.EvaluateReticle()) return;
        GameObject grenade = Instantiate(grenadePrefab, lineVisual.GetReticlePos(), Quaternion.identity);
        Debug.Log("Throw grenade");
    }
}
