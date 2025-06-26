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
    private ActorBehavior Selected_ActorBehavior;
    private CustomAnimationControl Selected_AnimControl;
    private CharacterDetailDisplay Selected_CharDetail;
    private SkinManager Selected_SkinManager;
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
        if (Selected_AnimControl == null) return;

        UpdateCurrentPlayingAnimation(Selected_AnimControl);

    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        var rayHit = Physics2D.GetRayIntersection(_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));

        if (!context.started) return;

        if (!rayHit.collider) return;
            
        SelectedObj = rayHit.collider.gameObject;

        ComponentsRefUpdate();
    }

    private void ComponentsRefUpdate()
    {
        if (!SelectedObj){
            Selected_ActorBehavior = null; 
            Selected_SkinManager = null; 
            Selected_CharDetail = null; 
            return;
        }
            
        if (!SelectedObj.GetComponentInParent<ActorBehavior>()) return;
        if (!SelectedObj.GetComponentInParent<SkinManager>()) return;
        if (!SelectedObj.GetComponentInParent<CharacterDetailDisplay>()) return;

        Selected_ActorBehavior = SelectedObj.GetComponentInParent<ActorBehavior>();
        Selected_SkinManager = SelectedObj.GetComponentInParent<SkinManager>();
        Selected_CharDetail = SelectedObj.GetComponentInParent<CharacterDetailDisplay>();

        Selected_ActorBehavior.OnClicked();
        UpdateActorSpeedDisplay(Selected_ActorBehavior);

        UpdateAllSkinPartsOptions(Selected_SkinManager);
        UpdateCurrentSkinPartsView(Selected_SkinManager);
        UpdateSkinGroupPrefixView(Selected_SkinManager);

        SelectionDisplayText(Selected_CharDetail.DisplayName, Selected_CharDetail.DisplayTextColor);

    }

    public void OnUnSelect(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        var rayHit = Physics2D.GetRayIntersection(_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));

        if (!rayHit.collider)
        {
            if(SelectedObj == null) return; 
            SelectedObj = null;
            ComponentsRefUpdate();
            SelectionDisplaytxt.text = "";
            OnClearItems();

        }
    }

    public void MoveOnClick(InputAction.CallbackContext context)
    {
        var rayHit = Physics2D.GetRayIntersection(_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));

        if (!context.started) return;

        if (rayHit.collider) return;

        if (!Selected_ActorBehavior) return;

        Selected_ActorBehavior = SelectedObj.GetComponentInParent<ActorBehavior>();

        Selected_ActorBehavior.MoveOnClick(_mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()));

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
       if(!Selected_SkinManager) return;

       Selected_SkinManager.ClearSkinEditParts();

       ClearScrollViewItemDisplay(SkinPartsListDisplayItems);
    }

    public void OnRandomizeSkin()
    {
        if (!Selected_SkinManager) return;
        Selected_SkinManager.RandomizeSkin();
        UpdateCurrentSkinPartsView(Selected_SkinManager);
    }

    private string SelectingSkinPart()
    {
        int pickeditemindex = SkinPartsOptions.value;
        string pickeditemdirectory = SkinPartsOptions.options[pickeditemindex].text;
        return pickeditemdirectory;
    }

    public void OnAddSkinItem()
    {
        if (!Selected_SkinManager) return;
        Selected_SkinManager.AddItem(SelectingSkinPart());
        SkinPartsListDisplayItems.Add(CreateScrollViewItem(SelectingSkinPart(),SkinPartsListDisplayView));
    }

    public void OnCreateSkin()
    {
        if (!Selected_SkinManager) return;
        Selected_SkinManager.CreateSkin();
        UpdateCurrentSkinPartsView(Selected_SkinManager);
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
        if (!Selected_SkinManager) return;
        Selected_SkinManager.SkinGroupPrefix.Clear();
        ClearScrollViewItemDisplay(SkinRandomGroupDisplayItems);
    }

    public void OnAddSkinGroupPrefix()
    {
            if (!Selected_SkinManager) return;
            string prefixinput = SkinGroupPrefixInput.text;
            Selected_SkinManager.SkinGroupPrefix.Add(prefixinput);
            UpdateSkinGroupPrefixView(Selected_SkinManager);
    }

    public void OnEditSpeed()
    {
        if (!Selected_ActorBehavior) return;
        if (!float.TryParse(ActorSpeedInput.text, out _)) return;

        Selected_ActorBehavior.ChangeSpeed(float.Parse(ActorSpeedInput.text));
        UpdateActorSpeedDisplay(Selected_ActorBehavior);

    }

    private void UpdateActorSpeedDisplay(ActorBehavior actor)
    {
        ActorSpeedDisplay.text = "Actor Speed: " + actor.MoveSpeed.ToString();
    }

    private void ClearScrollViewItemDisplay(List<ScrollViewItemDisplay> ScrollViewItems)
    {
            if (ScrollViewItems == null) return;

            foreach (var items in ScrollViewItems)
            {
                Destroy(items.gameObject);
            }

            ScrollViewItems.Clear();

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
