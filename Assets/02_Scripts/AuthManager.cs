using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;

public class AuthManager : MonoBehaviour
{
    [SerializeField] private Button signInButton;
    [SerializeField] private TMP_Text messageText;

    private async void Awake()
    {
        // USG 초기화
        await UnityServices.InitializeAsync();

        // 버튼 이벤트 연결
        signInButton.onClick.AddListener(async () =>
        {
            // 익명 로그인
            await SignInAsync();
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
