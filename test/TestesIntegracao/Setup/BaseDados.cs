using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using TesteIntegracaoxUnit.WebApi.Models;

namespace TesteIntegracaoxUnit.TestesIntegracao.Setup;

public sealed class BaseDados
{
    private readonly IServiceProvider serviceProvider;

    public BaseDados(IServiceProvider serviceProvider) => this.serviceProvider = serviceProvider;

    public void RestaurarDados()
    {
        var tarefas = serviceProvider.GetRequiredService<IList<TarefaModel>>();
        tarefas.Clear();
    }
}
