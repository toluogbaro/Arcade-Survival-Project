using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Movement
{
    public enum MovementStates { Jump, Walk, Crouch, Swim, Sprint, Dodge, Slide };


    public class SCR_PlayerController : MonoBehaviour
    {
        CharacterController controller;
        public GameObject capsule;
        public float walkSpeed = 8f;
        public float jumpWalkSpeed = 2f;
        public float sprintSpeed = 12f;
        public float gravity = -10f;
        public float jumpHeight = 2f;
        public float airTime = 0.1f;
        public float dodgeDistance = 5f;
        public float dodgeTime = 0.25f;

        public Transform groundCheck;
        public float groundDistance = 0.4f;
        public LayerMask groundMask;


        Vector3 velocity, move;
        public bool isGrounded;
        
        public MovementStates currentMovementState;
        public static bool cantMoveCharacter;


        private bool canDodge;

        public void Start()
        {
            controller = GetComponent<CharacterController>();
            canDodge =true;
        }

        public void Update()
        {

            if (!cantMoveCharacter)
                
            {
                Walk();
                Jump();
                Dodge();
            }

           

            
        }

        public void Walk()
        {
            
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            move = transform.right * x + transform.forward * z;

            //controller.Move(move * speed * Time.deltaTime);
            //else controller.Move(move * jumpWalkSpeed * Time.deltaTime);

            if (InputManager._instance.GetKey("Sprint") || InputManager._instance.GetKeyDown("Dodge") && isGrounded && SCR_PlayerValues._instance.currentStamina > 0) ChangeStates(MovementStates.Sprint);
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

        public void Dodge()
        {
            if (isGrounded && SCR_PlayerValues._instance.currentStamina > 0f && InputManager._instance.GetKeyDown("Dodge") && canDodge) StartCoroutine(InitiateDodge());
        }

        IEnumerator InitiateDodge()
        {
            canDodge = false;

            Vector3 lastPos = transform.position;

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            SCR_PlayerValues._instance.isInvincible = true;
            ChangeStates(MovementStates.Dodge);

            var passedTime = 0f;

            StartCoroutine(Rotate(1f));

            while(passedTime < dodgeTime)
            {
                cantMoveCharacter = true;

                passedTime += Time.deltaTime;

                Vector3 currentPos = transform.position;

                Debug.DrawLine(lastPos, currentPos, Color.red);

                controller.Move(move * dodgeDistance * Time.deltaTime);

                yield return null;

                cantMoveCharacter = false;
            }

            SCR_PlayerValues._instance.isInvincible = false;

            canDodge = true;

        }



        IEnumerator Rotate(float duration)
        {
            Quaternion startRot = capsule.transform.rotation;
            float t = 0f;
            int dodgeDirection = 0;

            if (Input.GetKey(KeyCode.D)) dodgeDirection = 0;
            else if (Input.GetKey(KeyCode.A)) dodgeDirection = 1;
            else if (Input.GetKey(KeyCode.W)) dodgeDirection = 2;
            else if (Input.GetKey(KeyCode.S)) dodgeDirection = 3;

            while (t < duration)
            {
                SCR_CameraMovement.cantMoveCamera = true;
                
                t += Time.deltaTime * 2.5f;
                //if (Input.GetKey(KeyCode.D)) capsule.transform.rotation = startRot * Quaternion.AngleAxis(t / duration * 360f, transform.right);
                //else if (Input.GetKey(KeyCode.A)) capsule.transform.rotation = startRot * Quaternion.AngleAxis(t / duration * 360f, -transform.right);
                

                switch (dodgeDirection)
                {
                    case 0:
                        capsule.transform.rotation = startRot * Quaternion.AngleAxis(t / duration * 360f, -Vector3.forward);
                        break;

                    case 1:
                        capsule.transform.rotation = startRot * Quaternion.AngleAxis(t / duration * 360f, Vector3.forward);
                        break;

                    case 2:
                        capsule.transform.rotation = startRot * Quaternion.AngleAxis(t / duration * 360f, Vector3.right);
                        break;

                    case 3:
                        capsule.transform.rotation = startRot * Quaternion.AngleAxis(t / duration * 360f, -Vector3.right);
                        break;


                }

                yield return null;
            }
            capsule.transform.rotation = startRot;
            SCR_CameraMovement.cantMoveCamera = false;

        }

        public void ChangeStates(MovementStates state)
        {
            currentMovementState = state;
        }


    }

  




}


