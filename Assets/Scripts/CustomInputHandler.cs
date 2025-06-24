using UnityEngine;
using UnityEngine.InputSystem;

public class CustomInputHandler : MonoBehaviour
{
    private Camera _mainCamera;
    private GameObject SelectedObj;
    private void Awake()
    {
        _mainCamera = Camera.main;
    }
    
    public void OnSelect(InputAction.CallbackContext context)
    {
        if(!context.started) return;

        var rayHit = Physics2D.GetRayIntersection(_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));

        if (rayHit.collider)
        {
            Debug.Log("Collider Clicked");
            SelectedObj = rayHit.collider.gameObject;

            if (SelectedObj.GetComponentInParent<ActorBehavior>())
            {
                ActorBehavior Actor = SelectedObj.GetComponentInParent<ActorBehavior>();

                Actor.OnClicked();
            }
        }
        else
        {
            if(SelectedObj != null)
            {
                if (SelectedObj.GetComponentInParent<ActorBehavior>())
                {
                    ActorBehavior Actor = SelectedObj.GetComponentInParent<ActorBehavior>();

                    Actor.MoveOnClick(_mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
                }
            }
            
        }

    }

    public void OnUnSelect(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        var rayHit = Physics2D.GetRayIntersection(_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));

        if (!rayHit.collider)
        {
            if (SelectedObj != null) SelectedObj = null; Debug.Log("Object Unselected");
        }
    }
}
