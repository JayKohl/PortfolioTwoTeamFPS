// ---------------------------------------------
// Sci-Fi Heavy Station Kit 
// Copyright (c) DotTeam. All Rights Reserved.
// https://dotteam.xyz, info@dotteam.xyz
// ---------------------------------------------

// Simple First Person Controller class, based on example from Unity documentation
// ( https://docs.unity3d.com/2019.3/Documentation/ScriptReference/CharacterController.Move.html )
// and some ideas from https://sharpcoderblog.com/blog/unity-3d-fps-controller

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DotTeam.HSK
{

    [RequireComponent(typeof(CharacterController))]

    public class FPC : MonoBehaviour
    {
        [Header("Speed")]
        public float walkingSpeed = 7.5f;
        public float runningSpeed = 11.5f;
        public float climbingSpeed = 5f;
        public float jumpSpeed = 8.0f;

        [Header("Look")]
        public float lookSpeed = 2.0f;
        public float lookXLimit = 45.0f;

        [Header("Features")]
        public float crouchSpeedRatio = 0.5f;
        public float crouchHeightRatio = 0.43f;

        [Header("Climbing")]
        public bool climbingAutoStart = false;

        [Space(10)]
        public bool canMove = true;
        public float gravity = 20.0f;
        public Camera playerCamera;

        [HideInInspector]
        public bool canClimbing = false;
        [HideInInspector]
        public bool isClimbing = false;
        [HideInInspector]
        public bool isHeightChanged = false;

        private CharacterController characterController;
        private Vector3 moveDirection = Vector3.zero;
        private float rotationX = 0;

        private float characterHeight = 0f;
        private float targetHeight = 0f;
        private Vector3 cameraLocalPos;
        private float cameraHeightOffset = 0f;
        private bool isCrouching = false;
        private float controllerCenterRatio = 0.5f;

        private DotControlCenter ccInstance = null;
        private KeyCode crouchShortcut = KeyCode.C;
        private KeyCode interactShortcut = KeyCode.E;


        void Start()
        {
            // Initialization
            characterController = GetComponent<CharacterController>();
            characterHeight = targetHeight = characterController.height;
            controllerCenterRatio = (characterHeight > 0) ? characterController.center.y / characterHeight : 0;
            if (playerCamera != null)
            {
                cameraHeightOffset = characterController.height - playerCamera.transform.localPosition.y;
                cameraLocalPos = playerCamera.transform.localPosition;
            }
            // Update common settings
            if (DotControlCenter.instance != null)
            {
                if (DotControlCenter.instance.trackChangesSettings) { ccInstance = DotControlCenter.instance; };
                UpdateConfig(DotControlCenter.instance);
            }
            // Lock cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            // Update Configuration Changes (shortcuts)
            if (ccInstance != null) { UpdateConfig(ccInstance); }

            // Check climbing state (InteractShortcut Button Pressed when a ladder is available)
            if (canClimbing && Input.GetKeyDown(interactShortcut)) { isClimbing = !isClimbing; }

            // Check crouch state (crouchShortcut Button Pressed)
            if (Input.GetKeyDown(crouchShortcut))
            {
                // Set target Controller Height (for a smooth crouch / lift up)
                targetHeight = characterHeight;
                if (isCrouching = !isCrouching) { targetHeight *= crouchHeightRatio; }
            }

            // Check running state (Left Shift Pressed)
            bool isRunning = Input.GetKey(KeyCode.LeftShift);

            // Calculate moving speed and direction
            if (canMove)
            {
                // Check jumping state
                bool isJumping = Input.GetButton("Jump");
                if (isClimbing)
                {
                    // Climbing 
                    moveDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
                    moveDirection = transform.TransformDirection(moveDirection);
                    moveDirection *= climbingSpeed;
                }
                else
                {
                    // Walkinng
                    float movementDirectionY = moveDirection.y;
                    float curSpeed = (isRunning ? runningSpeed : walkingSpeed) * (isCrouching ? crouchSpeedRatio : 1);
                    moveDirection = (transform.TransformDirection(Vector3.forward) * curSpeed * Input.GetAxis("Vertical")) + (transform.TransformDirection(Vector3.right) * curSpeed * Input.GetAxis("Horizontal"));
                    moveDirection.y = (isJumping && characterController.isGrounded) ? jumpSpeed : movementDirectionY;
                }
            }
            else
            {
                moveDirection = Vector3.zero;
            }

            // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
            // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            // as an acceleration (ms^-2)
            if ( !(isClimbing || characterController.isGrounded) )
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }

            // Accomodate caracter / camera height in crouch state
            float currentHeight = characterController.height;

            // Apply heights changes
            if (isHeightChanged = (targetHeight != currentHeight))
            {
                float newHeight = Mathf.Lerp(currentHeight, targetHeight, 4f * Time.deltaTime);
                if (Mathf.Abs(targetHeight - newHeight) < 0.0001f)
                {
                    // Desired height is almost reached, we set the target height forcibly
                    newHeight = targetHeight;
                }
                else
                {
                    // Stands up - check for the presence of a collider above the character, preventing further standing
                    if ((newHeight > currentHeight) && (playerCamera != null))
                    {
                        RaycastHit hit;
                        Ray ray2;
                        if (Physics.Raycast(ray2 = new Ray(playerCamera.transform.position, playerCamera.transform.up), out hit, cameraHeightOffset + 0.2f))
                        {
                            newHeight = currentHeight;
                        }
                    }
                }

                // Set character Height and Center
                characterController.height = newHeight;
                var ccenter = characterController.center;
                ccenter.y = controllerCenterRatio * newHeight;
                characterController.center = ccenter;

                // Calculate new camera local Y-position
                if (playerCamera != null)
                {
                    cameraLocalPos.y = newHeight - cameraHeightOffset;
                    playerCamera.transform.localPosition = cameraLocalPos;
                }
            }

            // Move the controller
            characterController.Move(moveDirection * Time.deltaTime);

            // Player and Camera rotation
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        // Tracking approach to the ladder (ladder collider must have a tag "Ladder2")
        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Ladder2")
            {
                // Climbing the ladder is available
                canClimbing = true;
                if (climbingAutoStart)
                {
                    // Automatic start of ladder climbing
                    isClimbing = true;
                }
            }
        }

        // Detaching from the ladder when leaving the ladder collider
        void OnTriggerExit(Collider other)
        {
            if (other.tag == "Ladder2") { canClimbing = isClimbing = false; }
        }

        // Automatic detaching from the ladder (in climbingAutoStart mode)
        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (climbingAutoStart && isClimbing && (hit.controller.collisionFlags == CollisionFlags.Below))
            {
                isClimbing = false;
            }
        }

        // Load configuration settings from DotControlCentre prefab
        void UpdateConfig(DotControlCenter c)
        {
            crouchShortcut = c.crouchShortcut;
            interactShortcut = c.interactShortcut;
        }
    }
}