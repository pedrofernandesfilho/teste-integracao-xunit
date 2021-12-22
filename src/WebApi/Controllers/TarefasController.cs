using Microsoft.AspNetCore.Mvc;

namespace TesteIntegracaoxUnit.WebApi;

[ApiController]
[Route("[controller]")]
public class TarefasController : ControllerBase
{
    private readonly IDictionary<uint, string> tarefas;

    public TarefasController(IDictionary<uint, string> tarefas) => this.tarefas = tarefas;

    [HttpGet("{id:min(1)}")]
    public ActionResult<string> Get(uint id)
    {
        if (!tarefas.ContainsKey(id))
            return NotFound();

        return Ok(tarefas[id]);
    }
}