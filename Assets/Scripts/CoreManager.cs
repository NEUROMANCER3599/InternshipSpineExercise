using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using TMPro;

public class CoreManager : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI SelectionDisplaytxt;
    [SerializeField] private TMP_Dropdown SkinPartsOptions;

    [Header("System")]
    private Camera _mainCamera;
    private GameObject SelectedObj;

    void Awake()
    {
        //Auto Initialize

        _mainCamera = Camera.main;

        foreach (var skinComponent in FindObjectsByType<SkinManager>(FindObjectsSortMode.None))
        {
            skinComponent.InitializeData();
        }
        foreach(var AnimationComponent in FindObjectsByType<CustomAnimationControl>(FindObjectsSortMode.None))
        {
            AnimationComponent.InitializeData();
        }
        foreach(var ActorBehavior in FindObjectsByType<ActorBehavior>(FindObjectsSortMode.None))
        {
            ActorBehavior.InitializeData();
        }
        
        
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        if (!context.started) return;

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

            if (SelectedObj.GetComponentInParent<SkinManager>())
            {
                SkinManager _skinManager = SelectedObj.GetComponentInParent<SkinManager>();
                UpdateSkinPartsList(_skinManager);
            }

            if (SelectedObj.GetComponentInParent<CharacterDetailDisplay>())
            {
                CharacterDetailDisplay detail = SelectedObj.GetComponentInParent<CharacterDetailDisplay>();
                SelectionDisplayText(detail.DisplayName,detail.DisplayTextColor);
            }
        }
        /*
        else
        {
            if (SelectedObj != null)
            {
                if (SelectedObj.GetComponentInParent<ActorBehavior>())
                {
                    ActorBehavior Actor = SelectedObj.GetComponentInParent<ActorBehavior>();

                    Actor.MoveOnClick(_mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
                }
            }

        }
        */

    }

    public void OnUnSelect(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        var rayHit = Physics2D.GetRayIntersection(_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));

        if (!rayHit.collider)
        {
            if (SelectedObj != null) SelectedObj = null; Debug.Log("Object Unselected");
            SelectionDisplaytxt.text = "";
        }
    }

    public void MoveOnClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        var rayHit = Physics2D.GetRayIntersection(_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));

        if (!rayHit.collider)
        {
            if (SelectedObj != null)
            {
                if (SelectedObj.GetComponentInParent<ActorBehavior>())
                {
                    ActorBehavior Actor = SelectedObj.GetComponentInParent<ActorBehavior>();

                    Actor.MoveOnClick(_mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
                }
            }
        }
       
    }

    public void SelectionDisplayText(string name, Color32 textcolor)
    {
        SelectionDisplaytxt.text = "Selecting: " + name;
        SelectionDisplaytxt.color = textcolor;
    }

    public void UpdateSkinPartsList(SkinManager skinManager)
    {
        foreach(var skinparts in skinManager.AllSkinParts)
        {
            SkinPartsOptions.options.Add(new TMP_Dropdown.OptionData(skinparts, null,Color.black));
        }
    }

    public void OnClearSkin()
    {
        if(SelectedObj != null && SelectedObj.GetComponentInParent<SkinManager>())
        {
            SkinManager _skinManager = SelectedObj.GetComponentInParent<SkinManager>();
            _skinManager.ClearItem();
        }
    }

    public void OnRandomizeSkin()
    {
        if (SelectedObj != null && SelectedObj.GetComponentInParent<SkinManager>())
        {
            SkinManager _skinManager = SelectedObj.GetComponentInParent<SkinManager>();
            _skinManager.RandomizeSkin();
        }
    }
}
