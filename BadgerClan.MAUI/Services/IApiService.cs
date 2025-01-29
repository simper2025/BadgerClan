namespace BadgerClan.MAUI.Services;

public interface IApiService
{
    string GetClientUri();
    Task<string> ActivateRunAndGun();
}
