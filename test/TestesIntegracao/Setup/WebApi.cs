using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;

namespace TesteIntegracaoxUnit.TestesIntegracao.Setup;

internal class WebApi : WebApplicationFactory<Program>
{
    private const string urlApi = "http://localhost/";

    public HttpClient ClienteHttp { get; }

    public WebApi() =>
        ClienteHttp = CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri(urlApi)
        });
}
