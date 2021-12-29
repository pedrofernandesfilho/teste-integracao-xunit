using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TesteIntegracaoxUnit.WebApi.Models;
using Xunit;

namespace TesteIntegracaoxUnit.TestesIntegracao.Api;

public class TarefasControllerTestes
{
    public class DadoQueExistemTarefasCadastradas_QuandoSolicitarUmaTarefaCadastradaInformandoOIdentificador : TesteBase, IDisposable
    {
        private readonly TarefaModel tarefaEsperada;
        private HttpResponseMessage respostaHttp = default!;

        public DadoQueExistemTarefasCadastradas_QuandoSolicitarUmaTarefaCadastradaInformandoOIdentificador(Configuracao fixture) : base(fixture)
        {
            tarefaEsperada = new TarefaModel(2, "Tarefa 2 do teste", true);

            var tarefas = ServiceProvider.GetRequiredService<IList<TarefaModel>>();
            tarefas.Add(new TarefaModel(1, "Tarefa 1 do teste", false));
            tarefas.Add(tarefaEsperada);
            tarefas.Add(new TarefaModel(3, "Tarefa 3 do teste", false));

            Task.Run( async () => {
                respostaHttp = await ClienteHttp.GetAsync("Tarefas/2");
            }).Wait();
        }

        [Fact]
        public async Task DeverRetorarAsInformacoesDaTarefa()
        {
            var respostaConteudo = await respostaHttp.Content.ReadAsStringAsync();
            var tarefaRetornada =
                JsonSerializer.Deserialize<TarefaModel>(respostaConteudo,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

            Assert.Equal(tarefaEsperada, tarefaRetornada);
        }

        [Fact]
        public void DeveRetornarEstadoOk() =>
            Assert.Equal(HttpStatusCode.OK, respostaHttp.StatusCode);


        public void Dispose() => respostaHttp.Dispose();
    }

    // [Fact]
    // public async Task DadoQueExistemTarefasCadastradas_QuandoSolicitarUmaTarefaNaoCadastradaInformandoOIdentificador_DeverRetorarNaoEncontrado()
    // {
    //     const HttpStatusCode respostaEsperada = HttpStatusCode.NotFound;
        
    //     using var resposta = await ClienteHttp.GetAsync("Tarefas/20");

    //     Assert.Equal(respostaEsperada, resposta.StatusCode);
    // }
}