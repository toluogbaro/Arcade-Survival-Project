using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Movement
{
    public enum MovementStates { Jump, Walk, Crouch, Swim, Sprint };

    [RequireComponent(typeof(CharacterController))]
    public class SCR_PlayerController : MonoBehaviour
    {
        CharacterController controller;
        public float walkSpeed = 8f;
        public float jumpWalkSpeed = 2f;
        public float sprintSpeed = 12f;
        public float gravity = -10f;
        public float jumpHeight = 2f;
        public float airTime = 0.1f;

        public Transform groundCheck;
        public float groundDistance = 0.4f;
        public LayerMask groundMask;


        Vector3 velocity;
        public bool isGrounded;

        MovementStates currentMovementState;


        public void Start()
        {
            controller = GetComponent<CharacterController>();

        }

        public void Update()
        {
            Walk();

            Jump();

            
        }

        public void Walk()
        {
            
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            //controller.Move(move * speed * Time.deltaTime);
            //else controller.Move(move * jumpWalkSpeed * Time.deltaTime);

            if (InputManager._instance.GetKey("Sprint") && isGrounded) ChangeStates(MovementStates.Sprint);
            else ChangeStates(MovementStates.Walk);
            

            switch (currentMovementState)

            {
                case MovementStates.Jump:
                    controller.Move(move * jumpWalkSpeed * Time.deltaTime);
                    break;

                case MovementStates.Sprint:
                    controller.Move(move * sprintSpeed * Time.deltaTime);
                    break;

                case MovementStates.Walk:
                    controller.Move(move * walkSpeed * Time.deltaTime);
                    break;

            }
        }

        public void Jump()
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded && velocity.y <= 0)
            {
                airTime = 0.1f;
                velocity.y = -2f;
                
            }

            if (InputManager._instance.GetKeyDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                ChangeStates(MovementStates.Jump);
            }

            if (!isGrounded) airTime -= Time.deltaTime;

            if (airTime <= 0f) velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);
        }

        public void ChangeStates(MovementStates state)
        {
            currentMovementState = state;
        }


    }

  




}


