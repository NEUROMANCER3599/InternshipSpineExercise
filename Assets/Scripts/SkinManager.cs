using UnityEngine;
using System.Collections.Generic;
using Spine.Unity;
using TMPro;
using NUnit.Framework.Internal;

public enum KiddoSkins
{
    boy,girl,georgie,shinchan,shisuka,pirate1,pirate2
}
public enum KiddoHats
{
    none,bandana1, bandana2, bandana3
}

public enum KiddoItems
{
    none,redballoon,blueballoon,yellowballoon,purpleballoon,Twig
}


public class SkinManager : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private KiddoSkins BaseSkins;
    [SerializeField] private KiddoHats Hats;
    [SerializeField] private KiddoItems Items;
    private Spine.Skin MixedSkin = new Spine.Skin("MixedSkin");

    [Header("UI Components")]
    [SerializeField] private TMP_Dropdown baseskindropdownmenu;
    [SerializeField] private TMP_Dropdown hatdropdownmenu;
    [SerializeField] private TMP_Dropdown itemdropdownmenu;
    private List<string> baseskinoptions;
    private List<string> hatsoptions;
    private List<string> itemdrops;

    [Header("System")]
    [SerializeField] SkeletonAnimation skeletonAnimation;
    [SerializeField] public Spine.Skeleton skeleton;
    [SerializeField] public Spine.SkeletonData skeletonData;
    void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        skeleton = skeletonAnimation.Skeleton;
        skeletonData = skeleton.Data;

        //InitializeSkin
        DropdownOptionUpdate();
        GenerateSkin();
    }


    void LateUpdate()
    {
        
        //GenerateSkin();
    }

    void DropdownOptionUpdate()
    {
        
        switch (baseskindropdownmenu.value)
        {
            case 0: BaseSkins = KiddoSkins.boy;  break;
            case 1: BaseSkins = KiddoSkins.girl; break;
            case 2: BaseSkins = KiddoSkins.georgie; break;
            case 3: BaseSkins = KiddoSkins.shisuka; break;
            case 4: BaseSkins = KiddoSkins.shinchan; break;
            case 5: BaseSkins = KiddoSkins.pirate1; break;
            case 6: BaseSkins = KiddoSkins.pirate2; break;
        }

        switch (hatdropdownmenu.value)
        {
            case 0: break;
            case 1: Hats = KiddoHats.bandana1; break;
            case 2: Hats = KiddoHats.bandana2; break;
            case 3: Hats = KiddoHats.bandana3; break;
        }

        switch (itemdropdownmenu.value)
        {
            case 0: Items = KiddoItems.none; break;
            case 1: Items = KiddoItems.redballoon; break;
            case 2: Items = KiddoItems.yellowballoon; break;
            case 3: Items = KiddoItems.blueballoon; break;
            case 4: Items = KiddoItems.purpleballoon; break;
            case 5: Items = KiddoItems.Twig; break;
        }
    }
        

        


    void BaseSkinCustomization()
    {
        switch (BaseSkins)
        {
            case KiddoSkins.boy: MixedSkin.AddSkin(skeletonData.FindSkin("preset/town/boy01")); break;
            case KiddoSkins.girl: MixedSkin.AddSkin(skeletonData.FindSkin("preset/town/girl01")); break;
            case KiddoSkins.shisuka: MixedSkin.AddSkin(skeletonData.FindSkin("preset/town/shisuka")); break;
            case KiddoSkins.georgie: MixedSkin.AddSkin(skeletonData.FindSkin("preset/town/georgie")); break;
            case KiddoSkins.shinchan: MixedSkin.AddSkin(skeletonData.FindSkin("preset/town/shinchan")); break;
            case KiddoSkins.pirate1: MixedSkin.AddSkin(skeletonData.FindSkin("preset/seaside/thief01")); break;
            case KiddoSkins.pirate2: MixedSkin.AddSkin(skeletonData.FindSkin("preset/seaside/thief02")); break;
        }
    }

    void HatCustomization()
    {
        switch (Hats)
        {
            case KiddoHats.bandana1: MixedSkin.AddSkin(skeletonData.FindSkin("hat/seaside/thief_bandana")); break;
            case KiddoHats.bandana2: MixedSkin.AddSkin(skeletonData.FindSkin("hat/seaside/thief_bandana2")); break;
            case KiddoHats.bandana3: MixedSkin.AddSkin(skeletonData.FindSkin("hat/seaside/thief_bandana3")); break;
        }
    }

    void ItemCustomization()
    {
        switch (Items)
        {
            case KiddoItems.none: MixedSkin.AddSkin(skeletonData.FindSkin("L_hand/town/_none")); break;
            case KiddoItems.redballoon: MixedSkin.AddSkin(skeletonData.FindSkin("L_hand/town/red_balloon")); break;
            case KiddoItems.yellowballoon: MixedSkin.AddSkin(skeletonData.FindSkin("L_hand/town/yellow_balloon")); break;
            case KiddoItems.blueballoon: MixedSkin.AddSkin(skeletonData.FindSkin("L_hand/town/blue_balloon")); break;
            case KiddoItems.purpleballoon: MixedSkin.AddSkin(skeletonData.FindSkin("L_hand/town/purple_balloon")); break;
            case KiddoItems.Twig: MixedSkin.AddSkin(skeletonData.FindSkin("L_hand/town/twig")); break;
        }
    }

    public void GenerateSkin()
    {
        //MixedSkin.Clear();
        DropdownOptionUpdate();
        MixedSkin = new Spine.Skin("MixedSkin");
        BaseSkinCustomization();
        HatCustomization();
        ItemCustomization();
        skeleton.SetSkin(MixedSkin);
        skeleton.SetSlotsToSetupPose();
    }
}
