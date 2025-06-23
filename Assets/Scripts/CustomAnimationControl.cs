using UnityEngine;
using System.Collections.Generic;
using Spine.Unity;


public class CustomAnimationControl : MonoBehaviour
{
    [Header("Animations")]
    [SpineAnimation] public List<string> IdleAnimation;
    [SpineAnimation] public List<string> WalkAnimation;
    [SpineAnimation] public List<string> InteractAnimation;
    [SpineAnimation] public List<string> JumpAnimation;

    [Header("Key Control")]
    [SerializeField] private string HorizontalMoveAxis = "Horizontal";
    [SerializeField] private string VerticalMoveAxis = "Vertical";
    [SerializeField] private KeyCode JumpKey = KeyCode.Space;
    [SerializeField] private KeyCode InteractKey = KeyCode.E;


    [Header("System")]
    SkeletonAnimation skeletonAnimation;
    public Spine.AnimationState spineAnimationState;
    public Spine.Skeleton skeleton;
    bool IsMoving;


    void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        spineAnimationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.Skeleton;

        spineAnimationState.SetAnimation(0, IdleAnimation[Random.Range(0,IdleAnimation.Count)],true);
    }


    void Update()
    {
        if (Input.GetKeyDown(JumpKey) && !IsMoving) //Playing Jumping Animation
        {
            spineAnimationState.SetAnimation(0, JumpAnimation[Random.Range(0, JumpAnimation.Count)], false);
            spineAnimationState.AddAnimation(0, IdleAnimation[Random.Range(0, IdleAnimation.Count)], true,0);
        }

        if (Input.GetKeyDown(InteractKey) && !IsMoving) //Playing Interaction Animation
        {
            spineAnimationState.SetAnimation(0, InteractAnimation[Random.Range(0, InteractAnimation.Count)], false);
            spineAnimationState.AddAnimation(0, IdleAnimation[Random.Range(0, IdleAnimation.Count)], true, 0);
        }

        if(Input.GetAxis(HorizontalMoveAxis) != 0 || Input.GetAxis(VerticalMoveAxis) != 0) //Playing Moving Animation
        {
            if (!IsMoving)
            {
                IsMoving = true;
                spineAnimationState.SetAnimation(0, WalkAnimation[Random.Range(0, WalkAnimation.Count)], true);
            }

            if (Input.GetAxis(HorizontalMoveAxis) < 0) //Turning Left
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if(Input.GetAxis(HorizontalMoveAxis) > 0) //Turning Right
            {
                transform.localScale = new Vector3(1, 1, 1);
            }

        }
        else
        {
            if (IsMoving)
            {
                IsMoving = false;
                spineAnimationState.SetAnimation(0, IdleAnimation[Random.Range(0, IdleAnimation.Count)], true);
            }
        }
    }
}
