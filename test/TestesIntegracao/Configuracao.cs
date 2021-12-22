using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using Xunit;

namespace TesteIntegracaoxUnit.TestesIntegracao;

[CollectionDefinition(Nome)]
public sealed class Colecao : ICollectionFixture<Configuracao>
{
   public const string Nome = nameof(Colecao);
}

public sealed class Configuracao : IDisposable
{
    internal WebApplicationFactory<Program> AplicacaoWeb { get; }
    public HttpClient ClienteHttp { get; }

    public Configuracao()
    {
        const string urlApi = "http://localhost/";

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

    public TesteBase(Configuracao fixture) => ClienteHttp = fixture.ClienteHttp;
}