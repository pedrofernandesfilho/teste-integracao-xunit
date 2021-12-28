using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using TesteIntegracaoxUnit.WebApi.Models;
using Xunit;

namespace TesteIntegracaoxUnit.TestesIntegracao.Api;

public class TarefasControllerTestes : TesteBase
{
    public TarefasControllerTestes(Configuracao fixture) : base(fixture) { }

    [Fact]
    public async Task DadoQueExistemTarefasCadastradas_QuandoSolicitarUmaTarefaCadastradaInformandoOIdentificador_DeverRetorarAsInformacoesDaTarefa()
    {
        var tarefaEsperada = new TarefaModel(2, "Tarefa 2 do teste", true);

        var tarefas = ServiceProvider.GetRequiredService<IList<TarefaModel>>();
        tarefas.Add(new TarefaModel(1, "Tarefa 1 do teste", false));
        tarefas.Add(tarefaEsperada);
        tarefas.Add(new TarefaModel(3, "Tarefa 3 do teste", false));

        using var resposta = await ClienteHttp.GetAsync("Tarefas/2");
        var respostaConteudo = await resposta.Content.ReadAsStringAsync();
        var tarefaRetornada = JsonSerializer.Deserialize<TarefaModel>(respostaConteudo, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.Equal(tarefaEsperada, tarefaRetornada);
    }

    [Fact]
    public async Task DadoQueExistemTarefasCadastradas_QuandoSolicitarUmaTarefaNaoCadastradaInformandoOIdentificador_DeverRetorarNaoEncontrado()
    {
        const HttpStatusCode respostaEsperada = HttpStatusCode.NotFound;
        
        using var resposta = await ClienteHttp.GetAsync("Tarefas/20");

        Assert.Equal(respostaEsperada, resposta.StatusCode);
    }
}