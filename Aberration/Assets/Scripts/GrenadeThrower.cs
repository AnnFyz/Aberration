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

    private void Awake()
    {
        //visualController = GetComponent<CurveVisualController>();
    }

    public void ThrowGrenade()
    {
        visualController.GetLineOriginAndDirection(out Vector3 worldOrigin, out Vector3 worldDirection);
        Debug.DrawRay(worldOrigin, worldDirection);
        GameObject grenade = Instantiate(grenadePrefab, origin.position, origin.rotation);
        grenade.GetComponent<Rigidbody>().AddForce(throwForce * worldDirection);
        Debug.Log("Throw grenade");
    }
}
