using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Check : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("CheckSelection");
        }
    }
    public void ChechSelection(InputAction.CallbackContext ctx)
    {
        Debug.Log("CheckSelection + " + ctx.ReadValue<Vector2>());
    }
}
