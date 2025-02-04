using UnityEngine;

public class TVController : MonoBehaviour
{
    public GameObject[] screens; // 각 채널에 해당하는 화면 오브젝트 배열
    public Material[] channelMaterials; // 각 채널에 해당하는 메테리얼 배열

    private int currentChannel = 0;
    private float volume = 0.5f; // 초기 볼륨 값 설정

    // TV의 채널을 설정하는 메서드
    public void SetChannel(int channel)
    {
        currentChannel = channel;

        foreach (GameObject screen in screens)
        {
            screen.SetActive(false);
        }

        // 현재 채널에 해당하는 화면 오브젝트가 있을 경우 메테리얼을 할당
        if (currentChannel >= 0 && currentChannel < screens.Length)
        {
            GameObject currentScreen = screens[currentChannel];
            currentScreen.SetActive(true);

            Renderer screenRenderer = screens[currentChannel].GetComponent<Renderer>();
            if (screenRenderer != null && currentChannel < channelMaterials.Length && channelMaterials[currentChannel] != null)
            {
                screenRenderer.material = channelMaterials[currentChannel];
            }
            else
            {
                Debug.LogError("스크린 렌더러 또는 채널 메테리얼이 올바르게 설정되지 않았습니다.");
            }
        }

        if (currentChannel == 5)
            GetComponent<QuestReporter>().Report(0);

        Debug.Log("현재 채널: " + currentChannel);
    }

    // TV의 전원 상태를 반환하는 메서드
    public bool IsOn()
    {
        // TV의 전원 상태는 현재 채널이 설정되어 있는지 여부로 판단합니다.
        return currentChannel >= 0 && currentChannel < screens.Length;
    }

    // TV의 볼륨을 설정하는 메서드
    public void SetVolume(float newVolume)
    {
        // 볼륨은 0에서 1 사이의 값으로 제한합니다.
        volume = Mathf.Clamp01(newVolume);
        Debug.Log("현재 볼륨: " + volume);
    }
}
