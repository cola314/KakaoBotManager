using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace KakaoBotManager.Utils;

public class UserInteraction
{
    private readonly IJSRuntime _jSRuntime;

    public UserInteraction(IJSRuntime jSRunTime)
    {
        _jSRuntime = jSRunTime;
    }

    public async Task Alert(string message)
    {
        await _jSRuntime.InvokeAsync<bool>("alert", message);
    }

    public async Task<bool> Confirm(string message)
    {
        return await _jSRuntime.InvokeAsync<bool>("confirm", message);
    }

    public async Task<string> Prompt(string message, string defaultValue = "")
    {
        return await _jSRuntime.InvokeAsync<string>("prompt", defaultValue);
    }
}
