using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Player_Controller : MonoBehaviour
{
    [Header("이동설정")]
    public float walkSpeed = 3.0f;
    public float runSpeed = 6.0f;
    public float rotationSpeed = 10.0f;

    [Header("공격 설정")]
    public float attackduration = 0.8f;
    public bool canMoveWhileAttacking = false;

    [Header("커보넌트")]
    public Animator animator;

    private CharacterController controller;
    private Camera playerCamera;

    private float currentSpeed;
    private bool isAttacking = false;
    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontial");
        float verical = Input.GetAxis("Vertical");

        if (horizontal != 0 || verical != 0)
        {
            Vector3 cameraForward = playerCamera.transform.forward;
            Vector3 cameraRight = playerCamera.transform.right;
            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 moveDirection = cameraForward * verical + cameraRight * horizontal;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeed = runSpeed;
            }
            else
            {
                currentSpeed = walkSpeed;
            }

            controller.Move(moveDirection * currentSpeed * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            currentSpeed = 0;
        }

        void UpdateAnimator()
        {
            float animatorSpeed = Mathf.Clamp01(currentSpeed / runSpeed);
            animator.SetFloat("speed", animatorSpeed);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        UpdateAnimator();
    }

    
}
