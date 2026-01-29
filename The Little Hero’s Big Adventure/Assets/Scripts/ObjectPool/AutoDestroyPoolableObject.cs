using UnityEngine;

public class AutoDestroyPoolableObject : PoolableObject
{
    public float AutoDestroyTime = 0.5f;

    private const string DisableMethodName = "Disable";

    public virtual void setAutoDestroyTime(float newTime)
    {
        AutoDestroyTime = newTime;
        Debug.Log("AutoDestroyTime: " + newTime);
    }

    public virtual void OnEnable()
    {
        CancelInvoke(DisableMethodName);

        Invoke(DisableMethodName, AutoDestroyTime);
    }

    public virtual void Disable()
    {
        gameObject.SetActive(false);
    }
}