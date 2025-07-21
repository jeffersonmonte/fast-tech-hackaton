using FastTech.Autenticacao.Application.Interfaces;
using FastTech.Autenticacao.Application.Services;
using FastTech.Autenticacao.Domain.Entities;
using FastTech.Autenticacao.Domain.Enums;
using FastTech.Autenticacao.Domain.Interfaces;
using FastTech.Autenticacao.Domain.ValueObjects;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace FastTech.Autenticacao.Application.Test.Unitario.UsuarioService;

[Trait("Category", "Unit")]
public class UsuarioService_InativarAsyncTeste
{
    private readonly Mock<IUsuarioRepository> mockRepository;
    private readonly Services.UsuarioService usuarioService;
    private readonly Mock<ITokenService> mockTokenService;

    public UsuarioService_InativarAsyncTeste()
    {
        mockRepository = new Mock<IUsuarioRepository>();
        mockTokenService = new Mock<ITokenService>();
        usuarioService = new Services.UsuarioService(mockRepository.Object, mockTokenService.Object);
    }

    [Fact]
    public async Task InativarUsuario_ComUsuarioExistente_DeveInativar()
    {
        // Arrange
        var id = Guid.NewGuid();
        var usuario = new Usuario("Gabriel", new Email("gabriel@email.com"), new Senha("senhaValida"), PerfilUsuario.Funcionario);

        mockRepository.Setup(r => r.ObterPorIdAsync(id)).ReturnsAsync(usuario);
        mockRepository.Setup(r => r.SalvarAlteracoesAsync()).Returns(Task.CompletedTask);

        // Act
        await usuarioService.InativarAsync(id);

        // Assert
        mockRepository.Verify(r => r.ObterPorIdAsync(id), Times.Once);
        mockRepository.Verify(r => r.Atualizar(usuario), Times.Once);
        mockRepository.Verify(r => r.SalvarAlteracoesAsync(), Times.Once);
        Assert.False(usuario.Ativo);
    }

    [Fact]
    public async Task InativarUsuario_ComUsuarioInexistente_DeveLancarExcecao()
    {
        // Arrange
        var id = Guid.NewGuid();

        mockRepository.Setup(r => r.ObterPorIdAsync(id)).ReturnsAsync((Usuario?)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => usuarioService.InativarAsync(id));
        mockRepository.Verify(r => r.Atualizar(It.IsAny<Usuario>()), Times.Never);
    }
}