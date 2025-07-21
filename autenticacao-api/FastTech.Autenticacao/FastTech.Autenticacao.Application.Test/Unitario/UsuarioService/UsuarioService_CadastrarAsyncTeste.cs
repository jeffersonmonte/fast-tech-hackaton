using FastTech.Autenticacao.Application.Dtos;
using FastTech.Autenticacao.Application.Interfaces;
using FastTech.Autenticacao.Application.Services;
using FastTech.Autenticacao.Domain.Entities;
using FastTech.Autenticacao.Domain.Enums;
using FastTech.Autenticacao.Domain.Interfaces;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace FastTech.Autenticacao.Application.Test.Unitario.UsuarioService;

[Trait("Category", "Unit")]
public class UsuarioService_CadastrarAsyncTeste
{
    private readonly Mock<IUsuarioRepository> mockRepository;
    private readonly Services.UsuarioService usuarioService;
    private readonly Mock<ITokenService> mockTokenService;
    private readonly Mock<IEventPublisher> mockEventPublisher;


    public UsuarioService_CadastrarAsyncTeste()
    {
        mockRepository = new Mock<IUsuarioRepository>();
        mockTokenService = new Mock<ITokenService>();
        mockEventPublisher = new Mock<IEventPublisher>();

        usuarioService = new Services.UsuarioService(mockRepository.Object, mockTokenService.Object, mockEventPublisher.Object);
    }

    [Fact]
    public async Task CadastrarUsuario_ComDadosValidos_DeveRetornarId()
    {
        // Arrange
        var dto = new UsuarioCadastroDto
        {
            Nome = "Lucas",
            Email = "lucas@email.com",
            Senha = "123456",
            Cpf = "12345678900",
            Perfil = PerfilUsuario.Cliente
        };

        mockRepository.Setup(r => r.ExisteComEmailAsync(dto.Email)).ReturnsAsync(false);
        mockRepository.Setup(r => r.ExisteComCpfAsync(dto.Cpf!)).ReturnsAsync(false);
        mockRepository.Setup(r => r.AdicionarAsync(It.IsAny<Usuario>())).Returns(Task.CompletedTask);
        mockRepository.Setup(r => r.SalvarAlteracoesAsync()).Returns(Task.CompletedTask);

        // Act
        var id = await usuarioService.CadastrarAsync(dto);

        // Assert
        mockRepository.Verify(r => r.AdicionarAsync(It.IsAny<Usuario>()), Times.Once);
        mockRepository.Verify(r => r.SalvarAlteracoesAsync(), Times.Once);
        Assert.NotEqual(Guid.Empty, id);
    }

    [Fact]
    public async Task CadastrarUsuario_ComEmailDuplicado_DeveLancarExcecao()
    {
        // Arrange
        var dto = new UsuarioCadastroDto
        {
            Nome = "Lucas",
            Email = "lucas@email.com",
            Senha = "123456",
            Perfil = PerfilUsuario.Cliente
        };

        mockRepository.Setup(r => r.ExisteComEmailAsync(dto.Email)).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => usuarioService.CadastrarAsync(dto));
        mockRepository.Verify(r => r.AdicionarAsync(It.IsAny<Usuario>()), Times.Never);
    }

    [Fact]
    public async Task CadastrarUsuario_ComCpfDuplicado_DeveLancarExcecao()
    {
        // Arrange
        var dto = new UsuarioCadastroDto
        {
            Nome = "Lucas",
            Email = "lucas@email.com",
            Senha = "123456",
            Cpf = "12345678900",
            Perfil = PerfilUsuario.Cliente
        };

        mockRepository.Setup(r => r.ExisteComEmailAsync(dto.Email)).ReturnsAsync(false);
        mockRepository.Setup(r => r.ExisteComCpfAsync(dto.Cpf)).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => usuarioService.CadastrarAsync(dto));
        mockRepository.Verify(r => r.AdicionarAsync(It.IsAny<Usuario>()), Times.Never);
    }
}