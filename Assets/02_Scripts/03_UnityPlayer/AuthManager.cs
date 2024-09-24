using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Authentication.PlayerAccounts;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;

namespace AuthUnityPlayer
{
    public class AuthManager : MonoBehaviour
    {
        [SerializeField] private Button signInButton, signOutButton, playerNameSaveButton;
        [SerializeField] private TMP_Text messageText;
        [SerializeField] private TMP_InputField playerNameIf;

        private async void Awake()
        {
            await UnityServices.InitializeAsync();

            // 로그인 후 콜백 연결
            PlayerAccountService.Instance.SignedIn += OnSignedIn;
            PlayerAccountService.Instance.SignedOut += () =>
            {
                Debug.Log("로그 아웃!");
            };

            signInButton.onClick.AddListener(async () =>
            {
                await PlayerAccountService.Instance.StartSignInAsync();
            });

            signOutButton.onClick.AddListener(() =>
            {
                PlayerAccountService.Instance.SignOut();
            });

            playerNameSaveButton.onClick.AddListener(async () =>
            {
                await SetPlayerNameAsync(playerNameIf.text);
            });
        }

        // 로그인 프로세스
        private async void OnSignedIn()
        {
            string accessToken = PlayerAccountService.Instance.AccessToken;

            await AuthenticationService.Instance.SignInWithUnityAsync(accessToken);

            Debug.Log("로그인 완료");
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
}