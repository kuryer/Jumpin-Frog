using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMVCamBaseValueCollector : MonoBehaviour
{
    [SerializeField] CMVCamBaseRuntimeValue reference;
    [SerializeField] CinemachineVirtualCameraBase cam;

    private void OnEnable()
    {
        reference.SetItem(cam);
    }

    private void OnDisable()
    {
        reference.NullItem();
    }

}
