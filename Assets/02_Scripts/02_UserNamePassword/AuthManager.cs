using System.Net.Http;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;

namespace AuthUserNamePassword
{
    public class AuthManager : MonoBehaviour
    {
        [SerializeField] private TMP_InputField userNameIf;
        [SerializeField] private TMP_InputField passwordIf;

        [SerializeField] private Button signUpButton;
        [SerializeField] private Button signInButton;

        private async void Awake()
        {
            await UnityServices.InitializeAsync();
            Debug.Log("UGS 초기화");

            signUpButton.onClick.AddListener(async () =>
            {
                await SignUpAsync(userNameIf.text, passwordIf.text);
            });

            signInButton.onClick.AddListener(async () =>
            {
                await SignInAsync(userNameIf.text, passwordIf.text);
            });
        }

        // 회원가입 로직
        // UserName : 대소문자 구별없음, 3자 ~ 20자, - @
        // Password : 대소문자 구별함, 8자 ~ 30자, 대문자 1, 소문자 1, 숫자 1, 특수문자 1 (-, @, #, !)
        private async Task SignUpAsync(string userName, string password)
        {
            try
            {
                await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(userName, password);
                Debug.Log("회원 가입 완료");
            }
            catch (AuthenticationException e)
            {
                Debug.LogException(e);
            }
            catch (RequestFailedException e)
            {
                Debug.LogException(e);
            }
        }

        // 로그인 로직
        private async Task SignInAsync(string userName, string password)
        {
            try
            {
                await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(userName, password);
                Debug.Log("로그인 완료");
            }
            catch (AuthenticationException e)
            {
                Debug.LogException(e);
            }
            catch (RequestFailedException e)
            {
                Debug.LogException(e);
            }
        }
    }
}
