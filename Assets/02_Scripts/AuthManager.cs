using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;

public class AuthManager : MonoBehaviour
{
    [SerializeField] private Button signinButton;
    [SerializeField] private Button signoutButton;
    [SerializeField] private Button playerNameSaveButton;

    [SerializeField] private TMP_Text messageText;
    [SerializeField] private TMP_InputField playerNameIf;

    private async void Awake()
    {
        // USG 초기화
        await UnityServices.InitializeAsync();

        // 이벤트 초기화
        EventConfig();

        // 버튼 이벤트 연결
        signinButton.onClick.AddListener(async () =>
        {
            // 익명 로그인
            await SignInAsync();
        });

        // 로그아웃 버튼 이벤트 연결
        signoutButton.onClick.AddListener(() =>
        {
            // 로그아웃
            AuthenticationService.Instance.SignOut();
        });

        // 플레이어 이름 변경 버튼 이벤트 연결
        /*
            50자
            공백 X
            Cookie#1234
        */
        playerNameSaveButton.onClick.AddListener(async () =>
        {
            await SetPlayerNameAsync(playerNameIf.text);
        });
    }

    // 익명 로그인 처리
    private async Task SignInAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("익명 로그인 성공");
        }
        catch (AuthenticationException e)
        {
            Debug.Log(e.Message);
        }
    }

    private void EventConfig()
    {
        // 로그인 성공했을 때 호출되는 이벤트
        AuthenticationService.Instance.SignedIn += () =>
        {
            messageText.text = $"Player Id : {AuthenticationService.Instance.PlayerId}\n";
            messageText.text += $"Player Name : {AuthenticationService.Instance.PlayerName.Split('#')[0]}\n";
        };

        // 로그아웃 성공했을 때 호출되는 이벤트
        AuthenticationService.Instance.SignedOut += () =>
        {
            messageText.text = $"Signed Out\n";
        };

        // 로그인이 만료됐을 때 호출되는 이벤트
        AuthenticationService.Instance.Expired += () =>
        {
            messageText.text = $"Player Session was expired!!\n";
        };
    }

    private async Task SetPlayerNameAsync(string playerName)
    {
        try
        {
            await AuthenticationService.Instance.UpdatePlayerNameAsync(playerName);

            var _playerName = AuthenticationService.Instance.PlayerName;

            _playerName = _playerName.Split('#')[0];

            messageText.text += $"{_playerName} is updated\n";
        }
        catch (AuthenticationException e)
        {
            Debug.Log(e.Message);
        }
    }
}
