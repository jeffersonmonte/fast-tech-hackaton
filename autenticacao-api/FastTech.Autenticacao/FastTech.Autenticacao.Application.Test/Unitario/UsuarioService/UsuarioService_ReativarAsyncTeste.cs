using System;
using System.Threading.Tasks;
using FastTech.Autenticacao.Application.Interfaces;
using FastTech.Autenticacao.Application.Services;
using FastTech.Autenticacao.Domain.Entities;
using FastTech.Autenticacao.Domain.Enums;
using FastTech.Autenticacao.Domain.Interfaces;
using FastTech.Autenticacao.Domain.ValueObjects;
using Moq;
using Xunit;

namespace FastTech.Autenticacao.Application.Test.Unitario.UsuarioService;

[Trait("Category", "Unit")]
public class UsuarioService_ReativarAsyncTeste
{
    private readonly Mock<IUsuarioRepository> mockRepository;
    private readonly Mock<ITokenService> mockTokenService;
    private readonly Services.UsuarioService usuarioService;

    public UsuarioService_ReativarAsyncTeste()
    {
        mockRepository = new Mock<IUsuarioRepository>();
        mockTokenService = new Mock<ITokenService>();
        usuarioService = new Services.UsuarioService(mockRepository.Object, mockTokenService.Object);
    }

    [Fact]
    public async Task ReativarUsuario_ComUsuarioExistente_DeveReativar()
    {
        // Arrange
        var id = Guid.NewGuid();
        var usuario = new Usuario("Fernanda", new Email("fernanda@email.com"), new Senha("senhaValida"), PerfilUsuario.Gerente);
        usuario.Inativar();

        mockRepository.Setup(r => r.ObterPorIdAsync(id)).ReturnsAsync(usuario);
        mockRepository.Setup(r => r.SalvarAlteracoesAsync()).Returns(Task.CompletedTask);

        // Act
        await usuarioService.ReativarAsync(id);

        // Assert
        mockRepository.Verify(r => r.ObterPorIdAsync(id), Times.Once);
        mockRepository.Verify(r => r.Atualizar(usuario), Times.Once);
        mockRepository.Verify(r => r.SalvarAlteracoesAsync(), Times.Once);
        Assert.True(usuario.Ativo);
    }

    [Fact]
    public async Task ReativarUsuario_ComUsuarioInexistente_DeveLancarExcecao()
    {
        // Arrange
        var id = Guid.NewGuid();

        mockRepository.Setup(r => r.ObterPorIdAsync(id)).ReturnsAsync((Usuario?)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => usuarioService.ReativarAsync(id));
        mockRepository.Verify(r => r.Atualizar(It.IsAny<Usuario>()), Times.Never);
    }
}