using UnityEngine;
using System.Collections.Generic;
using Spine.Unity;
using Spine.Unity.AttachmentTools;
using TMPro;
using System.Linq;
using UnityEngine.UI;
using Unity.VisualScripting;


public class SkinManager : MonoBehaviour
{
    [Header("Parameters")]
    public string DefaultSkinKeyword = "default";
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
    [Tooltip("Do Not Edit | This list is auto generated")]
    [SpineSkin] public List<string> WornSkinParts;
    [Tooltip("Do Not Edit | This list is auto generated")]
    [SpineSkin] public List<string> RuntimeEditSkinPartsList;



    public void InitializeData()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        skeleton = skeletonAnimation.Skeleton;
        skeletonData = skeleton.Data;

        if (AllSkinParts != null)
        {
            AllSkinParts.Clear();
        }
        foreach(var skin in skeletonData.Skins)
        {
            AllSkinParts.Add(skin.ToString());
        }

        GenerateSkinOnStart();
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

        if(FoundParts == null)
        {
            return null;
        }

        return FoundParts[Random.Range(0, FoundParts.Count)];
    }

    public void GenerateSkinOnStart()
    {

        MixedSkin = new Spine.Skin("MixedSkin");
        if(!IsRandomized)//Manual Customization
        {
            foreach (string skinparts in CustomizeSkinParts)
            {
                MixedSkin.AddSkin(skeletonData.FindSkin(skinparts));
                WornSkinParts.Add(skinparts);
            }
        }
        else //Randomized Auto-Customization
        {
            foreach(string groups in SkinGroupPrefix)
            {
                string partfound = RandomSelectionFromPrefix(groups);
                MixedSkin.AddSkin(skeletonData.FindSkin(partfound));
                WornSkinParts.Add(partfound);
                
            }
        }

        skeleton.SetSkin(MixedSkin);
        skeleton.SetSlotsToSetupPose();
        
    }

    public void RandomizeSkin()
    {
        ResetItem();
        MixedSkin = new Spine.Skin("MixedSkin");
        foreach (string groups in SkinGroupPrefix)
        {
            string partfound = RandomSelectionFromPrefix(groups);
            MixedSkin.AddSkin(skeletonData.FindSkin(partfound));
            WornSkinParts.Add(partfound);
        }
        skeleton.SetSkin(MixedSkin);
        skeleton.SetSlotsToSetupPose();
    }

    public void ResetItem()
    {
        MixedSkin.Clear();
        WornSkinParts.Clear();
        skeleton.SetSkin(MixedSkin);
        skeleton.SetSlotsToSetupPose();

    }

    public void ClearSkinEditParts()
    {
        RuntimeEditSkinPartsList.Clear();
    }

    public void AddItem(string SelectedItem)
    {
        RuntimeEditSkinPartsList.Add(SelectedItem);
    }

    public void CreateSkin()
    {
        if(RuntimeEditSkinPartsList != null)
        {
            ResetItem();
            MixedSkin = new Spine.Skin("MixedSkin");

            foreach (string skinparts in RuntimeEditSkinPartsList)
            {
                MixedSkin.AddSkin(skeletonData.FindSkin(skinparts));
                WornSkinParts.Add(skinparts);
            }

            skeleton.SetSkin(MixedSkin);
            skeleton.SetSlotsToSetupPose();
        }
    }

   
}
