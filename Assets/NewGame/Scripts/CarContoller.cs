using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarContoller : MonoBehaviour
{
    [SerializeField] private WheelCollider[] _wheelCollider;
    [SerializeField] private Transform[] _wheelMesh;

    [SerializeField] private float _MotorSpeed = 2000f;
    [SerializeField] private float _breakPower = 2000f;

    private Quaternion _wheelColliderRotation;
    private Vector3 _wheelColliderPosition;

    private float _inpX;
    private float _inpZ;

    private void Update()
    {
        _inpX = Input.GetAxis("Horizontal");
        _inpZ = Input.GetAxis("Vertical");
    }
    private void FixedUpdate()
    {
        WheelMovement();
    }

    private void WheelMovement()
    {
        // forword movement
        _wheelCollider[2].motorTorque = _MotorSpeed * _inpZ;
        _wheelCollider[3].motorTorque = _MotorSpeed * _inpZ;

        // wheel trun
        _wheelCollider[0].steerAngle = 30 * _inpX;
        _wheelCollider[1].steerAngle = 30 * _inpX;

        // getting and setting wheel positoin and rotation
        for (int i = 0; i < 4; i++)
        {
            _wheelCollider[i].GetWorldPose(out _wheelColliderPosition, out _wheelColliderRotation);

            _wheelMesh[i].transform.position = _wheelColliderPosition; _wheelMesh[i].transform.rotation = _wheelColliderRotation;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            _wheelCollider[2].brakeTorque = _breakPower;
            _wheelCollider[3].brakeTorque = _breakPower;
        }
        else
        {
            _wheelCollider[2].brakeTorque = 0;
            _wheelCollider[3].brakeTorque = 0;
        }
    }


}