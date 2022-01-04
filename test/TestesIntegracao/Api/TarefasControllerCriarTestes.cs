using System.Threading.Tasks;
using TesteIntegracaoxUnit.TestesIntegracao.Setup;
using Xunit;

namespace TesteIntegracaoxUnit.TestesIntegracao.Api;

public class TarefasControllerCriarTestes : TesteBase
{
    public class Requisicao : IAsyncLifetime
    {
        public Task InitializeAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task DisposeAsync()
        {
            throw new System.NotImplementedException();
        }
    }

    public class DadoQueExistemTarefasCadastradas_QuandoSolicitarUmaTarefaCadastradaInformandoOIdentificador : TesteBase, IClassFixture<Requisicao>
    {
        public DadoQueExistemTarefasCadastradas_QuandoSolicitarUmaTarefaCadastradaInformandoOIdentificador()
        {
        }

        //[Fact]
        //public void Teste1() { }
    }
}
