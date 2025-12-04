using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;
using UnityEditor;

public class CameraSwap : MonoBehaviour
{
    public CustomInspectorObjects customInspectorObjects;

    private Collider2D coll;

    private void Start()
    {
        coll = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (customInspectorObjects.panCameraOnContact)
            {
                CameraManager.instance.PanCameraOnContact(customInspectorObjects.panDistance, customInspectorObjects.panTime, customInspectorObjects.panDirection, false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector2 exitDirection = (collision.transform.position - coll.bounds.center).normalized;

            if (customInspectorObjects.swapCameras && customInspectorObjects.cameraOnDown != null && customInspectorObjects.cameraOnUp != null)
            {
                CameraManager.instance.SwapCamera(customInspectorObjects.cameraOnDown, customInspectorObjects.cameraOnUp, exitDirection);
            }
            if (customInspectorObjects.panCameraOnContact)
            {
                CameraManager.instance.PanCameraOnContact(customInspectorObjects.panDistance, customInspectorObjects.panTime, customInspectorObjects.panDirection, true);
            }
        }
    }
}

[System.Serializable]
public class CustomInspectorObjects
{
    public bool swapCameras = false;
    public bool panCameraOnContact = false;

    [HideInInspector] public CinemachineCamera cameraOnDown;
    [HideInInspector] public CinemachineCamera cameraOnUp;
    [HideInInspector] public PanDirection panDirection;
    [HideInInspector] public float panDistance = 3f;
    [HideInInspector] public float panTime = 0.35f;
}

public enum PanDirection
{
    Left,
    Right,
    Up,
    Down
}

[CustomEditor(typeof(CameraSwap))]
public class MyScriptEditor : Editor
{
    CameraSwap cameraSwap;

    private void OnEnable()
    {
        cameraSwap = (CameraSwap)target;
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (cameraSwap.customInspectorObjects.swapCameras)
        {
            cameraSwap.customInspectorObjects.cameraOnDown = EditorGUILayout.ObjectField("Camera on down", cameraSwap.customInspectorObjects.cameraOnDown, typeof(CinemachineCamera), true ) as CinemachineCamera;

            cameraSwap.customInspectorObjects.cameraOnUp = EditorGUILayout.ObjectField("Camera on up", cameraSwap.customInspectorObjects.cameraOnUp, typeof(CinemachineCamera), true) as CinemachineCamera;
        }

        if (cameraSwap.customInspectorObjects.panCameraOnContact)
        {
            cameraSwap.customInspectorObjects.panDirection = (PanDirection)EditorGUILayout.EnumPopup("Camera pan direction", cameraSwap.customInspectorObjects.panDirection);

            cameraSwap.customInspectorObjects.panDistance = EditorGUILayout.FloatField("Pan distance", cameraSwap.customInspectorObjects.panDistance);

            cameraSwap.customInspectorObjects.panTime = EditorGUILayout.FloatField("Pan time", cameraSwap.customInspectorObjects.panTime);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(cameraSwap);
        }
    }
}