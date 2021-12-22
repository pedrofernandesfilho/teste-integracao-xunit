using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace TesteIntegracaoxUnit.TestesIntegracao;

[CollectionDefinition(Nome)]
public sealed class Colecao : ICollectionFixture<Configuracao>
{
   public const string Nome = nameof(Colecao);
}

public sealed class Configuracao : IAsyncDisposable
{
    private readonly IServiceScope serviceScope;

    internal WebApplicationFactory<Program> AplicacaoWeb { get; }
    public HttpClient ClienteHttp { get; }
    public IServiceProvider ServiceProvider { get; }

    public Configuracao()
    {
        const string urlApi = "http://localhost/";

        AplicacaoWeb = new WebApplicationFactory<Program>();

        ClienteHttp = AplicacaoWeb.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri(urlApi)
        });

        serviceScope = AplicacaoWeb.Services.CreateScope();
        ServiceProvider = serviceScope.ServiceProvider;
    }

    public async ValueTask DisposeAsync()
    {
        ClienteHttp.Dispose();

        if (serviceScope is IAsyncDisposable asyncDisposableScope)
            await asyncDisposableScope.DisposeAsync();
        else
            serviceScope.Dispose();

        AplicacaoWeb.Dispose();
    }
}

[Collection(Colecao.Nome)]
public class TesteBase
{
    public HttpClient ClienteHttp { get; }
    public IServiceProvider ServiceProvider { get ; }

    public TesteBase(Configuracao fixture)
    {
        ClienteHttp = fixture.ClienteHttp;
        ServiceProvider = fixture.ServiceProvider;
    }
}