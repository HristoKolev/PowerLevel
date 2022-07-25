namespace PowerLevel.Server.Infrastructure;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xdxd.DotNet.Shared;

public interface RecaptchaService
{
    Task<bool> Verify(string token);
}

public class RecaptchaServiceImpl : RecaptchaService
{
    private readonly HttpServerAppConfig appConfig;
    private readonly ErrorReporter errorReporter;

    public RecaptchaServiceImpl(HttpServerAppConfig appConfig, ErrorReporter errorReporter)
    {
        this.appConfig = appConfig;
        this.errorReporter = errorReporter;
    }

    public async Task<bool> Verify(string token)
    {
        try
        {
            var httpClient = new HttpClient();

            var httpContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("secret", this.appConfig.RecaptchaSecret),
                new KeyValuePair<string, string>("response", token),
            });

            var httpResponse = await httpClient.PostAsync("https://www.google.com/recaptcha/api/siteverify", httpContent);

            httpResponse.EnsureSuccessStatusCode();

            var responseBody = await httpResponse.Content.ReadFromJsonAsync<RecaptchaVerifyResponse>();

            return responseBody!.Success;
        }
        catch (Exception exception)
        {
            await this.errorReporter.Error(exception, "FAILED_TO_VERIFY_RECAPTCHA");
            return false;
        }
    }
}

public class RecaptchaVerifyResponse
{
    public bool Success { get; set; }
}
