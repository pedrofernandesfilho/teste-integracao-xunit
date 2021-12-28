using Microsoft.AspNetCore.Mvc;
using TesteIntegracaoxUnit.WebApi.Models;

namespace TesteIntegracaoxUnit.WebApi;

[ApiController]
[Route("[controller]")]
public class TarefasController : ControllerBase
{
    private readonly IList<TarefaModel> tarefas;

    public TarefasController(IList<TarefaModel> tarefas) => this.tarefas = tarefas;

    [HttpGet("{id:min(1)}")]
    public ActionResult<TarefaModel> Get(ulong id)
    {
        var tarefa = tarefas.FirstOrDefault(tarefa => tarefa.Id == id);

        if (tarefa is null)
            return NotFound();

        return Ok(tarefa);
    }
}