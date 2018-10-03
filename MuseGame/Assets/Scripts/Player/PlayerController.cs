using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lookSensitivity = 3f;

    [SerializeField]
    private float thrusterForce = 1000f;

    [SerializeField]
    private float thrusterFuelBurnSpeed = 1f;

    [SerializeField]
    private float thrusterFuelRegenSpeed = 0.3f;
    private float thrusterFuelAmount = 1f;

    public float GetThrusterFuelAmount(){
        return thrusterFuelAmount;
    }

    [SerializeField]
    private LayerMask environmentMask;

    [Header("Sping settings:")]
    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 40f;

    private PlayerMotor playerMotor;
    private ConfigurableJoint configurableJoint;
    private Animator animator;

    void Start()
    {
        playerMotor = GetComponent<PlayerMotor>();
        configurableJoint = GetComponent<ConfigurableJoint>();
        animator = GetComponent<Animator>();

        SetJointSettings(jointSpring);
    }

    void Update()
    {
        if(PauseMenu.IsOn){
            if(Cursor.lockState != CursorLockMode.None){
                Cursor.lockState = CursorLockMode.None;
            }

            playerMotor.Move(Vector3.zero);
            playerMotor.Rotate(Vector3.zero);
            playerMotor.RotateCamera(0f);
            return;
        }

        

        if(Cursor.lockState != CursorLockMode.Locked){
            Cursor.lockState = CursorLockMode.Locked;
        }
        //Setting target position for spring
        //This make the physics act right when it comes to
        //applying gravity when flying over objects
        RaycastHit _hit;
        if(Physics.Raycast(transform.position, Vector3.down, out _hit, environmentMask)){
            configurableJoint.targetPosition = new Vector3(0f, _hit.point.y, 0f);
        }else{
            configurableJoint.targetPosition = new Vector3(0f, 0f, 0f);
        }
        float _xMov = Input.GetAxis("Horizontal");
        float _zMov = Input.GetAxis("Vertical");

        Vector3 _movHorizontal = transform.right * _xMov;
        Vector3 _movVertical = transform.forward * _zMov;

        Vector3 _velocity = (_movHorizontal + _movVertical).normalized * speed;

        //Animation Event
        animator.SetFloat("ForwardVelocity", _zMov);

        playerMotor.Move(_velocity);

        float _yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;

        playerMotor.Rotate(_rotation);

        float _xRot = Input.GetAxisRaw("Mouse Y");

        float _cameraRotationX = _xRot * lookSensitivity;

        playerMotor.RotateCamera(_cameraRotationX);

        Vector3 _thrusterForce = Vector3.zero;
        if (Input.GetButton("Jump") && thrusterFuelAmount > 0f)
        {
            thrusterFuelAmount -= thrusterFuelBurnSpeed * Time.deltaTime;

            if(thrusterFuelAmount >= 0.01f){
                _thrusterForce = Vector3.up * thrusterForce;
                SetJointSettings(0f);
            }
        }else{

            thrusterFuelAmount += thrusterFuelRegenSpeed * Time.deltaTime;
            SetJointSettings(jointSpring);
        }

        thrusterFuelAmount = Mathf.Clamp(thrusterFuelAmount, 0f, 1f);

        playerMotor.ApplyThruster(_thrusterForce);
    }

    private void SetJointSettings(float _jointSpring){
        configurableJoint.yDrive = new JointDrive
        {
            positionSpring = _jointSpring,
            maximumForce = jointMaxForce
        };
    }
}
