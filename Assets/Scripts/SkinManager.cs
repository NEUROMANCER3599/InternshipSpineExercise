using UnityEngine;
using System.Collections.Generic;
using Spine.Unity;
using Spine.Unity.AttachmentTools;
using TMPro;
using System.Linq;


public class SkinManager : MonoBehaviour
{
    [Header("Parameters")]
    [Tooltip("Set Auto-Customizing")]
    public bool IsRandomized;
    [Tooltip("Manually Customize Skin Parts Here")]
    [SpineSkin]public List<string> CustomizeSkinParts;
    [Tooltip("Adding Skin Parts Group Prefix for Randomization")]
    public List<string> SkinGroupPrefix;
    
   

    [Header("System")]
    [SerializeField] SkeletonAnimation skeletonAnimation;
    [SerializeField] public Spine.Skeleton skeleton;
    [SerializeField] public Spine.SkeletonData skeletonData;
    private Spine.Skin MixedSkin = new Spine.Skin("MixedSkin");
    [Tooltip("Do Not Edit | This list is auto generated")]
    [SpineSkin] public List<string> AllSkinParts;
    void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        skeleton = skeletonAnimation.Skeleton;
        skeletonData = skeleton.Data;

        
        InitializeData();
        GenerateSkin();
    }

    
    void InitializeData()
    {
        if(AllSkinParts != null)
        {
            AllSkinParts.Clear();
        }
        foreach(var skin in skeletonData.Skins)
        {
            AllSkinParts.Add(skin.ToString());
        }
    }

    string RandomSelectionFromPrefix(string Prefix)
    {
        List<string> FoundParts = new List<string>();

        foreach(string parts in AllSkinParts)
        {
            if (parts.StartsWith(Prefix))
            {
                FoundParts.Add(parts);
            }
        }

        return FoundParts[Random.Range(0, FoundParts.Count)];
    }

    public void GenerateSkin()
    {

        MixedSkin = new Spine.Skin("MixedSkin");
        if(!IsRandomized)//Manual Customization
        {
            foreach (string skinparts in CustomizeSkinParts)
            {
                MixedSkin.AddSkin(skeletonData.FindSkin(skinparts));
            }
        }
        else //Randomized Auto-Customization
        {
            foreach(string groups in SkinGroupPrefix)
            {
                MixedSkin.AddSkin(skeletonData.FindSkin(RandomSelectionFromPrefix(groups)));
            }
        }

        skeleton.SetSkin(MixedSkin);
        skeleton.SetSlotsToSetupPose();
        
    }
}
