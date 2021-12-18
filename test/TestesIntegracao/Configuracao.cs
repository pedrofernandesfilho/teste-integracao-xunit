using System;
using Xunit;

namespace TesteIntegracaoxUnit.TestesIntegracao;

[CollectionDefinition(Nome)]
sealed class Colecao {
   public const string Nome = nameof(Colecao);
}

sealed class Fixture : IDisposable
{
    public void Dispose()
    {
    }
}

[Collection(Colecao.Nome)]
class TesteBase
{
    
}