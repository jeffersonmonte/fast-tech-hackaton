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
public class UsuarioService_AtualizarSenhaAsyncTeste
{
    private readonly Mock<IUsuarioRepository> mockRepository;
    private readonly Services.UsuarioService usuarioService;
    private readonly Mock<ITokenService> mockTokenService;

    public UsuarioService_AtualizarSenhaAsyncTeste()
    {
        mockRepository = new Mock<IUsuarioRepository>();
        mockTokenService = new Mock<ITokenService>();
        usuarioService = new Services.UsuarioService(mockRepository.Object, mockTokenService.Object);
    }

    [Fact]
    public async Task AtualizarSenha_ComUsuarioExistente_DeveAtualizarComSucesso()
    {
        // Arrange
        var id = Guid.NewGuid();
        var usuario = new Usuario("Jéssica", new Email("jessica@email.com"), new Senha("senhaAntiga"), PerfilUsuario.Cliente);

        mockRepository
            .Setup(r => r.ObterPorIdAsync(id))
            .ReturnsAsync(usuario);

        mockRepository
            .Setup(r => r.SalvarAlteracoesAsync())
            .Returns(Task.CompletedTask);

        // Act
        await usuarioService.AtualizarSenhaAsync(id, "novaSenha123");

        // Assert
        mockRepository.Verify(r => r.ObterPorIdAsync(id), Times.Once);
        mockRepository.Verify(r => r.Atualizar(It.IsAny<Usuario>()), Times.Once);
        mockRepository.Verify(r => r.SalvarAlteracoesAsync(), Times.Once);
    }

    [Fact]
    public async Task AtualizarSenha_ComUsuarioInexistente_DeveLancarExcecao()
    {
        // Arrange
        var id = Guid.NewGuid();

        mockRepository
            .Setup(r => r.ObterPorIdAsync(id))
            .ReturnsAsync((Usuario?)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => usuarioService.AtualizarSenhaAsync(id, "novaSenha123"));

        mockRepository.Verify(r => r.Atualizar(It.IsAny<Usuario>()), Times.Never);
        mockRepository.Verify(r => r.SalvarAlteracoesAsync(), Times.Never);
    }
}