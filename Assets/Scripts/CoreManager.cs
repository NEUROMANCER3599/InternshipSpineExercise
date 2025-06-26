using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UIElements;

public class CoreManager : MonoBehaviour
{
    [Header("UI Components | Skin Menu")]
    [SerializeField] private TextMeshProUGUI SelectionDisplaytxt;
    [SerializeField] private TMP_Dropdown SkinPartsOptions;
    [SerializeField] private Transform SkinPartsListDisplayView;
    [SerializeField] private Transform CurrentSkinPartsDisplayView;

    [Header("UI Components | Skin Preferences")]
    [SerializeField] private TMP_InputField SkinGroupPrefixInput;
    [SerializeField] private Transform SkinGroupPrefixDisplayView;

    [Header("UI Components | Actor Preferences")]
    [SerializeField] private TextMeshProUGUI ActorSpeedDisplay;
    [SerializeField] private TextMeshProUGUI CurrentAnimationDisplay;
    [SerializeField] private TMP_InputField ActorSpeedInput;


    [Header("System | Do not change in runtime")]
    [SerializeField] private GameObject ScrollViewItemPrefab;
    [SerializeField] private List<ScrollViewItemDisplay> SkinPartsListDisplayItems;
    [SerializeField] private List<ScrollViewItemDisplay> CurrentSkinPartDisplayItems;
    [SerializeField] private List<ScrollViewItemDisplay> SkinRandomGroupDisplayItems;
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

    private void Update()
    {
        if(SelectedObj != null && SelectedObj.GetComponentInParent<CustomAnimationControl>())
        {
            CustomAnimationControl AnimControl = SelectedObj.GetComponentInParent<CustomAnimationControl>();
            UpdateCurrentPlayingAnimation(AnimControl);
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
                UpdateActorSpeedDisplay(Actor);
            }

            if (SelectedObj.GetComponentInParent<SkinManager>())
            {
                SkinManager _skinManager = SelectedObj.GetComponentInParent<SkinManager>();
                UpdateAllSkinPartsOptions(_skinManager);
                UpdateCurrentSkinPartsView(_skinManager);
                UpdateSkinGroupPrefixView(_skinManager);
            }

            if (SelectedObj.GetComponentInParent<CharacterDetailDisplay>())
            {
                CharacterDetailDisplay detail = SelectedObj.GetComponentInParent<CharacterDetailDisplay>();
                SelectionDisplayText(detail.DisplayName,detail.DisplayTextColor);
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

    private void UpdateAllSkinPartsOptions(SkinManager skinManager)
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

            ClearScrollViewItemDisplay(SkinPartsListDisplayItems);
            //ClearCurrentSkinPartDisplay();
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
      
        ClearScrollViewItemDisplay(CurrentSkinPartDisplayItems);

        foreach(var items in _SkinM.WornSkinParts)
        {
            CurrentSkinPartDisplayItems.Add(CreateScrollViewItem(items, CurrentSkinPartsDisplayView));
            
        }
    }

    private void UpdateSkinGroupPrefixView(SkinManager _SkinM)
    {
        ClearScrollViewItemDisplay(SkinRandomGroupDisplayItems);
        foreach (var items in _SkinM.SkinGroupPrefix)
        {
            SkinRandomGroupDisplayItems.Add(CreateScrollViewItem(items, SkinGroupPrefixDisplayView));
        }
    }

    public void OnClearSkinGroupPrefix()
    {
        if (SelectedObj.GetComponentInParent<SkinManager>())
        {
            SkinManager _skinM = SelectedObj.GetComponentInParent<SkinManager>();
            _skinM.SkinGroupPrefix.Clear();
            ClearScrollViewItemDisplay(SkinRandomGroupDisplayItems);
        }
    }

    public void OnAddSkinGroupPrefix()
    {
        if (SelectedObj != null && SelectedObj.GetComponentInParent<SkinManager>())
        {
            SkinManager _skinM = SelectedObj.GetComponentInParent<SkinManager>();
            string prefixinput = SkinGroupPrefixInput.text;
            _skinM.SkinGroupPrefix.Add(prefixinput);
            UpdateSkinGroupPrefixView(_skinM);
        }
    }

    public void OnEditSpeed()
    {
        if(SelectedObj != null  && SelectedObj.GetComponentInParent<ActorBehavior>())
        {
            ActorBehavior actor = SelectedObj.GetComponentInParent<ActorBehavior>();
            if (float.TryParse(ActorSpeedInput.text, out _))
            {
                actor.ChangeSpeed(float.Parse(ActorSpeedInput.text));
                UpdateActorSpeedDisplay(actor);
            }
            
        }
    }

    private void UpdateActorSpeedDisplay(ActorBehavior actor)
    {
        ActorSpeedDisplay.text = "Actor Speed: " + actor.MoveSpeed.ToString();
    }

    private void ClearScrollViewItemDisplay(List<ScrollViewItemDisplay> ScrollViewItems)
    {
        if (ScrollViewItems != null)
        {
            foreach (var items in ScrollViewItems)
            {
                Destroy(items.gameObject);
            }
            ScrollViewItems.Clear();
        }
    }

    private void UpdateCurrentPlayingAnimation(CustomAnimationControl AnimC)
    {
        string displayvalue = AnimC.skeletonAnimation.AnimationName;
        CurrentAnimationDisplay.text = "Currently Playing: " + displayvalue;
    }


    private ScrollViewItemDisplay CreateScrollViewItem(string displaytext, Transform SpawnContainer)
    {
        var ScrollViewItem = Instantiate(ScrollViewItemPrefab, SpawnContainer);
        ScrollViewItem.GetComponent<ScrollViewItemDisplay>().WriteText(displaytext);

        return ScrollViewItem.GetComponent<ScrollViewItemDisplay>();
            
    }

   
   

}
