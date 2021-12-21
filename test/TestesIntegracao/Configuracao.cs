using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using Xunit;

namespace TesteIntegracaoxUnit.TestesIntegracao;

[CollectionDefinition(Nome)]
public sealed class Colecao : ICollectionFixture<Fixture>
{
   public const string Nome = nameof(Colecao);
}

public sealed class Fixture : IDisposable
{
    public WebApplicationFactory<Program> AplicacaoWeb { get; }
    public HttpClient ClienteHttp { get; }

    public Fixture()
    {
        const string urlApi = "http://localhost/api/";

        AplicacaoWeb = new WebApplicationFactory<Program>();
        ClienteHttp = AplicacaoWeb.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri(urlApi)
        });
    }

    public void Dispose()
    {
        ClienteHttp.Dispose();
        AplicacaoWeb.Dispose();
    }
}

[Collection(Colecao.Nome)]
public class TesteBase
{
    public HttpClient ClienteHttp { get; }

    public TesteBase(Fixture fixture) => ClienteHttp = fixture.ClienteHttp;
}