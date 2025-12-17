using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventEase.Services
{
    public class ToastMessage
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Message { get; set; } = string.Empty;
        public string Level { get; set; } = "info"; // success, error, warning, info
        public int Duration { get; set; } = 3000; // ms
    }

    public class ToastService
    {
        public event Action<ToastMessage>? OnShow;

        public void Show(string message, string level = "info", int duration = 3000)
        {
            var t = new ToastMessage { Message = message, Level = level, Duration = duration };
            OnShow?.Invoke(t);
        }

        public void ShowSuccess(string message, int duration = 3000) => Show(message, "success", duration);
        public void ShowError(string message, int duration = 4000) => Show(message, "danger", duration);
        public void ShowInfo(string message, int duration = 3000) => Show(message, "info", duration);
    }
}
