using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ChannelDialController : MonoBehaviour
{
    public TVController tvController; // TVController 참조
    public float rotationSpeed = 60f; // 초당 회전 속도 (도 단위)
    public bool enableDebugLogs = false; // 디버그 로그를 사용할지 여부를 결정하는 플래그
    public float minRotation = 0f; // 최소 회전 각도
    public float maxRotation = 360f; // 최대 회전 각도
    private int[] angleToChannelMapping; // 각도와 채널 매핑
    private int previousChannel = -1; // 이전 채널을 저장하는 변수

    [SerializeField] private InputActionReference leftActionReference;
    [SerializeField] private InputActionReference rightActionReference;
    private InputActionReference curRef;

    private void Start()
    {
        // 각도와 채널을 수작업으로 매핑합니다.
        angleToChannelMapping = new int[] { 3, 4, 5, 6, 0, 1, 2 };
    }

    public void SelectEnterDial(SelectEnterEventArgs arg) {
        int val = arg.interactorObject.interactionLayers.value;

        if ((val & InteractionLayerMask.GetMask("LeftDirect")) == InteractionLayerMask.GetMask("LeftDirect")) {
            leftActionReference.action.performed += RotateDial;
            curRef = leftActionReference;
        }
        else if ((val & InteractionLayerMask.GetMask("RightDirect")) == InteractionLayerMask.GetMask("RightDirect")) {
            rightActionReference.action.performed += RotateDial;
            curRef = rightActionReference;
        }

        Access.Player.StopPlayer();
    }

    public void SelectExitDial(SelectExitEventArgs arg) {
        leftActionReference.action.performed -= RotateDial;
        rightActionReference.action.performed -= RotateDial;
        Access.Player.MovePlayer();
        curRef = null;
    }

    private void RotateDial(InputAction.CallbackContext obj)
    {
        // 회전량을 계산합니다.
        float rotationAmount = curRef.action.ReadValue<Vector2>().x * rotationSpeed * Time.deltaTime;

        // 현재 회전값을 가져옵니다.
        float currentZRotation = transform.rotation.eulerAngles.z;
        float newZRotation = currentZRotation + rotationAmount;

        // 회전 범위를 0도에서 360도로 맞춥니다.
        if (newZRotation < 0f)
        {
            newZRotation += 360f;
        }
        else if (newZRotation >= 360f)
        {
            newZRotation -= 360f;
        }

        // 회전 범위를 제한합니다.
        if (newZRotation >= minRotation && newZRotation <= maxRotation)
        {
            transform.Rotate(Vector3.forward, rotationAmount, Space.Self);
            UpdateChannel(); // 채널 업데이트
        }
    }

    void UpdateChannel()
    {
        float currentAngle = transform.rotation.eulerAngles.z;
        int channel = GetChannelFromAngle(currentAngle); // 각도에서 채널을 계산
        if (channel != previousChannel)
        {
            tvController.SetChannel(channel); // TVController에 채널 설정
            previousChannel = channel; // 이전 채널을 현재 채널로 업데이트

            if (enableDebugLogs)
            {
                Debug.Log("현재 채널: " + channel); // 디버그 로그 출력
            }
        }
    }

    int GetChannelFromAngle(float angle)
    {
        // 0 ~ 360 각도 범위를 0 ~ 6 채널 범위로 매핑합니다.
        int index = Mathf.RoundToInt(angle / 51f) % 7;
        return angleToChannelMapping[index];
    }
}
