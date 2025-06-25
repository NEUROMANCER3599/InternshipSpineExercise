using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UIElements;

public class CoreManager : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI SelectionDisplaytxt;
    [SerializeField] private TMP_Dropdown SkinPartsOptions;
    [SerializeField] private Transform SkinPartsListDisplayView;
    [SerializeField] private Transform CurrentSkinPartsDisplayView;

    [Header("System")]
    [SerializeField] private GameObject ScrollViewItemPrefab;
    [SerializeField] private List<ScrollViewItemDisplay> SkinPartsListDisplayItems;
    [SerializeField] private List<ScrollViewItemDisplay> CurrentSkinPartDisplayItems;
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
                UpdateAllSkinPartsList(_skinManager);
                UpdateCurrentSkinPartsView(_skinManager);
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
            OnClearItems();

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

    private void SelectionDisplayText(string name, Color32 textcolor)
    {
        SelectionDisplaytxt.text = "Selecting: " + name;
        SelectionDisplaytxt.color = textcolor;
    }

    private void UpdateAllSkinPartsList(SkinManager skinManager)
    {
        foreach(var skinparts in skinManager.AllSkinParts)
        {
            SkinPartsOptions.options.Add(new TMP_Dropdown.OptionData(skinparts, null,Color.black));
        }
    }

    public void OnClearItems()
    {
        if(SelectedObj != null && SelectedObj.GetComponentInParent<SkinManager>())
        {
            SkinManager _skinManager = SelectedObj.GetComponentInParent<SkinManager>();
            _skinManager.ClearSkinEditParts();
            
            ClearSkinPartsListDisplay();
            //ClearCurrentSkinPartDisplay();
        }
    }

    private void ClearSkinPartsListDisplay()
    {
        if (SkinPartsListDisplayItems != null)
        {
            foreach (var items in SkinPartsListDisplayItems)
            {
                Destroy(items.gameObject);
            }
            SkinPartsListDisplayItems.Clear();
        }
    }

    private void ClearCurrentSkinPartDisplay()
    {
        if(CurrentSkinPartDisplayItems != null)
        {
            foreach(var items in CurrentSkinPartDisplayItems)
            {
                Destroy(items.gameObject);
            }
            CurrentSkinPartDisplayItems.Clear();
        }
    }

    public void OnRandomizeSkin()
    {
        if (SelectedObj != null && SelectedObj.GetComponentInParent<SkinManager>())
        {
            SkinManager _skinManager = SelectedObj.GetComponentInParent<SkinManager>();
            _skinManager.RandomizeSkin();
            UpdateCurrentSkinPartsView(_skinManager);
        }
    }

    private string SelectingSkinPart()
    {
        int pickeditemindex = SkinPartsOptions.value;
        string pickeditemdirectory = SkinPartsOptions.options[pickeditemindex].text;
        return pickeditemdirectory;
    }

    public void OnAddSkinItem()
    {
            if (SelectedObj != null && SelectedObj.GetComponentInParent<SkinManager>())
            {
                SkinManager _skinManager = SelectedObj.GetComponentInParent<SkinManager>();
                _skinManager.AddItem(SelectingSkinPart());
                SkinPartsListDisplayItems.Add(CreateScrollViewItem(SelectingSkinPart(),SkinPartsListDisplayView));
            }
    }

    public void OnCreateSkin()
    {
        if (SelectedObj != null && SelectedObj.GetComponentInParent<SkinManager>())
        {
            SkinManager _skinManager = SelectedObj.GetComponentInParent<SkinManager>();
            _skinManager.CreateSkin();
            UpdateCurrentSkinPartsView(_skinManager);
        }
    }

    private void UpdateCurrentSkinPartsView(SkinManager _SkinM)
    {
        if(CurrentSkinPartDisplayItems != null)
        {
            ClearCurrentSkinPartDisplay();
        }
        foreach(var items in _SkinM.WornSkinParts)
        {
            CurrentSkinPartDisplayItems.Add(CreateScrollViewItem(items, CurrentSkinPartsDisplayView));
            
        }
    }

    private ScrollViewItemDisplay CreateScrollViewItem(string displaytext, Transform SpawnContainer)
    {
        var ScrollViewItem = Instantiate(ScrollViewItemPrefab, SpawnContainer);
        ScrollViewItem.GetComponent<ScrollViewItemDisplay>().WriteText(displaytext);

        return ScrollViewItem.GetComponent<ScrollViewItemDisplay>();
            
    }

   
   

}
