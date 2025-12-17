using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using EventEase.Models;

namespace EventEase.Services
{
    public class UserSessionService
    {
        private const string StorageKey = "eventease_user_session";
        private readonly IJSRuntime js;
        private UserSession? session;

        public UserSessionService(IJSRuntime js)
        {
            this.js = js;
        }

        public UserSession? CurrentUser => session;
        public bool IsAuthenticated => session != null;

        public event Action? OnChange;

        public async Task LoadAsync()
        {
            try
            {
                var json = await js.InvokeAsync<string?>("localStorage.getItem", StorageKey);
                if (!string.IsNullOrEmpty(json))
                {
                    session = JsonSerializer.Deserialize<UserSession>(json);
                    OnChange?.Invoke();
                }
            }
            catch
            {
                // ignore failures
            }
        }

        public async Task SignInAsync(UserSession s)
        {
            session = s;
            await SaveAsync();
            OnChange?.Invoke();
        }

        public async Task SignOutAsync()
        {
            session = null;
            await js.InvokeVoidAsync("localStorage.removeItem", StorageKey);
            OnChange?.Invoke();
        }

        private async Task SaveAsync()
        {
            if (session == null) return;
            var json = JsonSerializer.Serialize(session);
            await js.InvokeVoidAsync("localStorage.setItem", StorageKey, json);
        }
    }
}
