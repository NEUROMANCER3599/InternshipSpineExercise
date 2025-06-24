using UnityEngine;
using System.Collections.Generic;
using Spine.Unity;
using DG.Tweening;
using System.Collections;


public class CustomAnimationControl : MonoBehaviour
{
    [Header("Animations List")]
    [SpineAnimation] public List<string> IdleAnimation;
    [SpineAnimation] public List<string> WalkAnimation;
    [SpineAnimation] public List<string> InteractAnimation;
    [SpineAnimation] public List<string> JumpAnimation;

    [Header("Mouse Control")]
    public float MoveSpeed = 1f;
    float movementInterval;

    [Header("Keyboard Control (For Debugging)")]
    [SerializeField] private bool IsEnabled;
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
        if (IsEnabled)
        {
            KeyBoardControl();
        }

        if(movementInterval > 0)
        {
            movementInterval -= 1f * Time.deltaTime;
        }
        else
        {
            if (IsMoving)
            {
                IsMoving = false;
                spineAnimationState.AddAnimation(0, IdleAnimation[Random.Range(0, IdleAnimation.Count)], true, 0);
            }
        }
    }

    void KeyBoardControl()
    {
        if (Input.GetKeyDown(JumpKey) && !IsMoving) //Playing Jumping Animation
        {
            spineAnimationState.SetAnimation(0, JumpAnimation[Random.Range(0, JumpAnimation.Count)], false);
            spineAnimationState.AddAnimation(0, IdleAnimation[Random.Range(0, IdleAnimation.Count)], true, 0);
        }

        if (Input.GetKeyDown(InteractKey) && !IsMoving) //Playing Interaction Animation
        {
            spineAnimationState.SetAnimation(0, InteractAnimation[Random.Range(0, InteractAnimation.Count)], false);
            spineAnimationState.AddAnimation(0, IdleAnimation[Random.Range(0, IdleAnimation.Count)], true, 0);
        }

        if (Input.GetAxis(HorizontalMoveAxis) != 0 || Input.GetAxis(VerticalMoveAxis) != 0) //Playing Moving Animation
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
            else if (Input.GetAxis(HorizontalMoveAxis) > 0) //Turning Right
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
    } //For Debugging

    public void MoveOnClick(Vector2 clickedposition)
    {
            Debug.Log("Moving " + gameObject.name + " to position: " + clickedposition);
            transform.DOMove(new Vector3(clickedposition.x, clickedposition.y), MoveSpeed);
            movementInterval = MoveSpeed;
            IsMoving = true;
            spineAnimationState.SetAnimation(0, WalkAnimation[Random.Range(0, WalkAnimation.Count)], true);

            if(clickedposition.x > transform.position.x)
            {
                skeleton.ScaleX = 1;
            }
            else
            {
                skeleton.ScaleX = -1;
            }
    }


    public void OnClicked()
    {
        Debug.Log(gameObject.name + " Selected");

        //PlayRandomAnimation From Interact and Jump Anim list
        int RandomGroup = Random.Range(0, 2);

        switch (RandomGroup)
        {
            case 0: spineAnimationState.SetAnimation(0, JumpAnimation[Random.Range(0, JumpAnimation.Count)], false); break;
            case 1: spineAnimationState.SetAnimation(0, InteractAnimation[Random.Range(0, InteractAnimation.Count)], false); break;
            default: spineAnimationState.SetAnimation(0, JumpAnimation[Random.Range(0, JumpAnimation.Count)], false); break;
        }

        spineAnimationState.AddAnimation(0, IdleAnimation[Random.Range(0, IdleAnimation.Count)], true, 0);

    }
}
