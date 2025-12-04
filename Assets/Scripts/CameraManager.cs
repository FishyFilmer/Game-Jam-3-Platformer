using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [Header("Controls for lerping the Y Damping during player jump/fall")]
    [SerializeField] private float fallPanAmount = 0.25f;
    [SerializeField] private float fallYPanTime = 0.35f;
    [SerializeField] private CinemachineCamera[] allCameras;

    public float fallSpeedYDampingChangeThreshold = -15f;
    private float normYPanAmount;
    private Vector2 startingTrackedObjectOffset;
    public bool isLerpingYDamping { get; private set; }
    public bool lerpedFromPlayerFalling { get; set; }

    private Coroutine lerpYPanCoroutine;
    private Coroutine panCameraCoroutine;
    private CinemachinePositionComposer positionComposer;
    private CinemachineCamera currentCamera;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        for (int i = 0; i < allCameras.Length; i++)
        {
            if (allCameras[i].enabled)
            {
                currentCamera = allCameras[i];
                positionComposer = currentCamera.GetComponent<CinemachinePositionComposer>();
            }
        }
        normYPanAmount = positionComposer.Damping.y;

        startingTrackedObjectOffset = positionComposer.TargetOffset;
    }

    public void LerpYDamping(bool isPlayerFalling)
    {
        lerpYPanCoroutine = StartCoroutine(LerpYAction(isPlayerFalling));
    }

    private IEnumerator LerpYAction(bool isPlayerFalling)
    {
        isLerpingYDamping = true;

        float startDampAmount = positionComposer.Damping.y;
        float endDampAmount = 0f;

        if (isPlayerFalling)
        {
            endDampAmount = fallPanAmount;
            lerpedFromPlayerFalling = true;
        }
        else
        {
            endDampAmount = normYPanAmount;
        }

        float elapsedTime = 0f;
        while (elapsedTime < fallYPanTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedPanAmount = Mathf.Lerp(startDampAmount, endDampAmount, (elapsedTime / fallYPanTime));
            positionComposer.Damping.y = lerpedPanAmount;

            yield return null;
        }

        isLerpingYDamping = false;
    }

    public void SwapCamera(CinemachineCamera cameraFromDown, CinemachineCamera cameraFromUp, Vector2 triggerExitDirection)
    {
        if (currentCamera == cameraFromDown && triggerExitDirection.x > 0f)
        {
            cameraFromUp.enabled = true;
            cameraFromDown.enabled = false;
            currentCamera = cameraFromUp;
            positionComposer=currentCamera.GetComponent<CinemachinePositionComposer>();
        }
        else if (currentCamera == cameraFromUp && triggerExitDirection.x < 0f)
        {
            cameraFromDown.enabled = true;
            cameraFromUp.enabled = false;
            currentCamera = cameraFromDown;
            positionComposer = currentCamera.GetComponent<CinemachinePositionComposer>();
        }
    }

    public void PanCameraOnContact(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        panCameraCoroutine = StartCoroutine(PanCamera(panDistance, panTime, panDirection, panToStartingPos));
    }

    private IEnumerator PanCamera(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        Vector2 endPos = Vector2.zero;
        Vector2 startingPos = Vector2.zero;

        if (!panToStartingPos)
        {
            switch (panDirection)
            {
                case PanDirection.Up:
                    endPos = Vector2.up;
                    break;
                case PanDirection.Down:
                    endPos = Vector2.down; 
                    break;
                case PanDirection.Left:
                    endPos = Vector2.left; 
                    break;
                case PanDirection.Right:
                    endPos = Vector2.right; 
                    break;
                default:
                    break;
            }

            endPos *= panDistance;
            startingPos = startingTrackedObjectOffset;
            endPos += startingPos;
        }
        else
        {
            startingPos = positionComposer.TargetOffset;
            endPos = startingTrackedObjectOffset;
        }

        float elapsedTime = 0f;
        while(elapsedTime < panTime)
        {
            elapsedTime += Time.deltaTime;

            Vector3 panLerp = Vector3.Lerp(startingPos, endPos, (elapsedTime / panTime));
            positionComposer.TargetOffset = panLerp;

            yield return null;
        }
    }
}
