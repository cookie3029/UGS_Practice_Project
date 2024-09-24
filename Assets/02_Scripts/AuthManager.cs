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
    [SerializeField] private TMP_Text messageText;

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
        };

        AuthenticationService.Instance.SignedOut += () =>
        {
            messageText.text = $"Signed Out\n";
        };

        AuthenticationService.Instance.Expired += () =>
        {
            messageText.text = $"Player Session was expired!!\n";
        };
    }


    /*
        C# Thread Programming

        Async / Await

        async void/Task 함수명()
        {
            로직1
            await 작업
            로직2
        }
    */
}
