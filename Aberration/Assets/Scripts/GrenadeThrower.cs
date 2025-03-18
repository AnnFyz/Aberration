using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors.Visuals;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class GrenadeThrower : MonoBehaviour
{
    [SerializeField] Transform origin;
    [SerializeField] GameObject grenadePrefab;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float throwForce = 40f;
    [SerializeField] XRRayInteractor xrRayInteractor;
    [SerializeField] XRInteractorReticleVisual xRInteractorReticleVisual;
    public bool canPlaceGrenade = true;
    public ObjectPool grenadePool;
    public ObjectPool projectilePool;

    private void Awake()
    {
        grenadePool = ObjectPool.CreateInstance(grenadePrefab.GetComponent<GrenadeHandler>(), 200);
        projectilePool = ObjectPool.CreateInstance(projectilePrefab.GetComponent<ProjectileHandler>(), 300);
    }
    private void Start()
    {
        xRInteractorReticleVisual.enabled = false;
        canPlaceGrenade = true;
    }

    private void OnEnable()
    {
        xrRayInteractor.hoverEntered.AddListener(ActivateReticle);
        xrRayInteractor.hoverExited.AddListener(DeactivateReticle);
    }

    public void ThrowProjectile(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PoolableObject instance = projectilePool.GetObject();
            if (instance != null)
            {
                instance.transform.position = origin.position;
                instance.transform.rotation = origin.rotation;
                instance.GetComponent<Rigidbody>().AddForce(throwForce * origin.transform.forward);
                AudioManager.Instance.PlaySound("Shooting");
            }
        }
         
    }

    public void ThrowGrenade()
    {
        if (!xrRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit raycastHit)) return;
        if (!xRInteractorReticleVisual.enabled) return;

        PoolableObject instance = grenadePool.GetObject();
        if (instance != null)
        {
            instance.transform.position = raycastHit.point;
            instance.transform.rotation = Quaternion.identity;
        }
    }
  
    void ActivateReticle(HoverEnterEventArgs args)
    {
        xRInteractorReticleVisual.enabled = true;
    }

    void DeactivateReticle(HoverExitEventArgs args)
    {
        xRInteractorReticleVisual.enabled = false;
    }
}
