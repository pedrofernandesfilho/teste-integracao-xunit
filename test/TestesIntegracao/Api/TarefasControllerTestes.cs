using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace TesteIntegracaoxUnit.TestesIntegracao.Api;

public class TarefasControllerTestes : TesteBase
{
    public TarefasControllerTestes(Configuracao fixture) : base(fixture) { }

    [Fact]
    public async Task DadoQueExisteTarefasCadastradas_QuandoSolicitarUmaTarefaCadastradaInformandoOIdentificador_DeverRetorarADescricaoDaTarefa()
    {
        var tarefas = ServiceProvider.GetRequiredService<IDictionary<uint, string>>();
        tarefas.Add(1, "Tarefa 1 do teste");
        tarefas.Add(2, "Tarefa 2 do teste");
        tarefas.Add(3, "Tarefa 3 do teste");

        const string respostaEsperada = "Tarefa 2 do teste";
        
        var corpoResposta = await ClienteHttp.GetStringAsync("Tarefas/2");

        Assert.Equal(respostaEsperada, corpoResposta);
    }

    [Fact]
    public async Task DadoQueExisteTarefasCadastradas_QuandoSolicitarUmaTarefaNaoCadastradaInformandoOIdentificador_DeverRetorarNaoEncontrado()
    {
        const HttpStatusCode respostaEsperada = HttpStatusCode.NotFound;
        
        var resposta = await ClienteHttp.GetAsync("Tarefas/20");

        Assert.Equal(respostaEsperada, resposta.StatusCode);
    }
}