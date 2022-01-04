using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TesteIntegracaoxUnit.TestesIntegracao.Setup;
using TesteIntegracaoxUnit.WebApi.Models;
using Xunit;

namespace TesteIntegracaoxUnit.TestesIntegracao.Api;

public class TarefasControllerObterPorIdTestes
{
    public class RequisicaoTarefaExistente : IAsyncLifetime
    {
        private readonly Aplicacao aplicacao;

        public readonly TarefaModel TarefaEsperada;
        public HttpResponseMessage RespostaHttp = default!;

        public RequisicaoTarefaExistente(Aplicacao aplicacao)
        {
            this.aplicacao = aplicacao;

            aplicacao.RestaurarDadosDaBase();

            TarefaEsperada = new TarefaModel(2, "Tarefa 2 do teste", true);

            var tarefas = aplicacao.ServiceProvider.GetRequiredService<IList<TarefaModel>>();
            tarefas.Add(new TarefaModel(1, "Tarefa 1 do teste", false));
            tarefas.Add(TarefaEsperada);
            tarefas.Add(new TarefaModel(3, "Tarefa 3 do teste", false));
        }

        public async Task InitializeAsync() =>
            RespostaHttp = await aplicacao.ClienteHttp.GetAsync("Tarefas/2");

        public Task DisposeAsync()
        {
            RespostaHttp.Dispose();
            return Task.CompletedTask;
        }
    }

    public class DadoQueExistemTarefasCadastradas_QuandoSolicitarUmaTarefaCadastradaInformandoOIdentificador : TesteBase, IClassFixture<RequisicaoTarefaExistente>
    {
        private readonly RequisicaoTarefaExistente requisicaoTarefaExistente;

        public DadoQueExistemTarefasCadastradas_QuandoSolicitarUmaTarefaCadastradaInformandoOIdentificador(RequisicaoTarefaExistente requisicaoTarefaExistente) =>
            this.requisicaoTarefaExistente = requisicaoTarefaExistente;

        [Fact]
        public void DeveRetornarEstadoOk() =>
            Assert.Equal(HttpStatusCode.OK, requisicaoTarefaExistente.RespostaHttp.StatusCode);

        [Fact]
        public async ValueTask DeverRetorarAsInformacoesDaTarefa()
        {
            var respostaConteudo = await requisicaoTarefaExistente.RespostaHttp.Content.ReadAsStringAsync();

            var tarefaEsperadaJson =
                JsonSerializer.Serialize<TarefaModel>(
                    requisicaoTarefaExistente.TarefaEsperada,
                    new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            Assert.Equal(tarefaEsperadaJson, respostaConteudo);
        }
    }

    public class RequisicaoTarefaInexistente : IAsyncLifetime
    {
        public HttpResponseMessage RespostaHttp = default!;
        private readonly Aplicacao aplicacao;

        public RequisicaoTarefaInexistente(Aplicacao aplicacao)
        {
            this.aplicacao = aplicacao;

            aplicacao.RestaurarDadosDaBase();
        }

        public async Task InitializeAsync() =>
            RespostaHttp = await aplicacao.ClienteHttp.GetAsync("Tarefas/20");

        public Task DisposeAsync()
        {
            RespostaHttp.Dispose();
            return Task.CompletedTask;
        }
    }

    public class DadoQueExistemTarefasCadastradas_QuandoSolicitarUmaTarefaNaoCadastradaInformandoOIdentificador : TesteBase, IClassFixture<RequisicaoTarefaInexistente>
    {
        private readonly RequisicaoTarefaInexistente requisicaoTarefaInexistente;

        public DadoQueExistemTarefasCadastradas_QuandoSolicitarUmaTarefaNaoCadastradaInformandoOIdentificador(RequisicaoTarefaInexistente requisicaoTarefaInexistente) =>
            this.requisicaoTarefaInexistente = requisicaoTarefaInexistente;

        [Fact]
        public void DeverRetorarEstadoNaoEncontrado() =>
            Assert.Equal(HttpStatusCode.NotFound, requisicaoTarefaInexistente.RespostaHttp.StatusCode);

        [Fact]
        public async ValueTask NaoDeveRetornarInformacoesDeTarefa()
        {
            var tarefaEsperadaSemDados = new TarefaModel(default, default!, default);

            var respostaConteudo = await requisicaoTarefaInexistente.RespostaHttp.Content.ReadAsStringAsync();
            var tarefaRetornada = 
                JsonSerializer.Deserialize<TarefaModel>(respostaConteudo,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal(tarefaEsperadaSemDados, tarefaRetornada);
        }
    }
}
