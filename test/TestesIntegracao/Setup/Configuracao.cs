using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace TesteIntegracaoxUnit.TestesIntegracao.Setup;

[CollectionDefinition(Nome)]
public sealed class Colecao : ICollectionFixture<Configuracao>
{
   public const string Nome = nameof(Colecao);
}

public sealed class Configuracao : IAsyncLifetime
{
    private readonly BaseDados baseDados;

    internal WebApplicationFactory<Program> Api { get; }
    public HttpClient ClienteHttp { get; }
    public IServiceScope ServiceScope { get; set; }
    public IServiceProvider ServiceProvider { get; set; }


    // EXECUTADO 1 VEZ ANTES DE TODOS OS TESTES
    public Configuracao()
    {
        const string urlApi = "http://localhost/";

        Api = new WebApplicationFactory<Program>();

        ClienteHttp = Api.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri(urlApi)
        });

        ServiceScope = Api.Services.CreateScope();
        ServiceProvider = ServiceScope.ServiceProvider;

        baseDados = new BaseDados(ServiceProvider);
    }

    // EXECUTADO 1 VEZ ANTES DE TODOS OS TESTES, APÓS O CONSTRUTOR
    public Task InitializeAsync() => Task.CompletedTask;

    // EXECUTADO 1 VEZ DEPOIS DE TODOS OS TESTES
    public async Task DisposeAsync()
    {
        ClienteHttp.Dispose();

        if (ServiceScope is IAsyncDisposable asyncDisposableScope)
            await asyncDisposableScope.DisposeAsync();
        else
            ServiceScope.Dispose();

        Api.Dispose();
    }

    public void RestaurarDadosDaBase() => baseDados.RestaurarDados();
}

public sealed class PreparacaoClasseTeste : IDisposable
{
    // EXECUTADO 1 VEZ ANTES DE TODOS OS TESTES DE UMA CLASSE DE TESTE
    public PreparacaoClasseTeste(Configuracao configuracao)
    {
        configuracao.ServiceScope = configuracao.Api.Services.CreateScope();
        configuracao.ServiceProvider = configuracao.ServiceScope.ServiceProvider;
    }

    // EXECUTADO 1 VEZ DEPOIS DE TODOS OS TESTES DE UMA CLASSE DE TESTE
    public void Dispose() { }
}

[Collection(Colecao.Nome)]
public abstract class TesteBase : IDisposable, IClassFixture<PreparacaoClasseTeste>
{
    // EXECUTADO ANTES DE CADA TESTE (1 VEZ POR TESTE)
    protected TesteBase() { }

    // EXECUTADO DEPOIS DE CADA TESTE (1 VEZ POR TESTE)
    public void Dispose() { }
}
