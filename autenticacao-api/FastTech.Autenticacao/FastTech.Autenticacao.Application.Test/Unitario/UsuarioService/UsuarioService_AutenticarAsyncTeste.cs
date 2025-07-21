using FastTech.Autenticacao.Application.Dtos;
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
public class UsuarioService_AutenticarAsyncTeste
{
    private readonly Mock<IUsuarioRepository> mockRepository;
    private readonly Services.UsuarioService usuarioService;
    private readonly Mock<ITokenService> mockTokenService;

    public UsuarioService_AutenticarAsyncTeste()
    {
        mockRepository = new Mock<IUsuarioRepository>();
        mockTokenService = new Mock<ITokenService>();
        usuarioService = new Services.UsuarioService(mockRepository.Object, mockTokenService.Object);
    }

    [Fact]
    public async Task Autenticar_ComEmailESenhaCorretos_DeveRetornarUsuarioOutput()
    {
        // Arrange
        var senhaEmTexto = "123456";
        var usuario = new Usuario("Ana", new Email("ana@email.com"), new Senha(senhaEmTexto), PerfilUsuario.Cliente);

        var dto = new UsuarioLoginDto
        {
            Email = "ana@email.com",
            Senha = senhaEmTexto
        };

        mockRepository.Setup(r => r.ObterPorEmailAsync(dto.Email)).ReturnsAsync(usuario);

        // Act
        var resultado = await usuarioService.AutenticarAsync(dto);

        // Assert
        mockRepository.Verify(r => r.ObterPorEmailAsync(dto.Email), Times.Once);
        Assert.NotNull(resultado);
        Assert.Equal(usuario.Id, resultado!.Id);
        Assert.Equal(usuario.Nome, resultado.Nome);
        Assert.Equal(usuario.Email.Endereco, resultado.Email);
    }

    [Fact]
    public async Task Autenticar_ComCpfESenhaCorretos_DeveRetornarUsuarioOutput()
    {
        // Arrange
        var senha = "senha123";
        var usuario = new Usuario("Carlos", new Email("carlos@email.com"), new Senha(senha), PerfilUsuario.Cliente, new Cpf("12345678900"));

        var dto = new UsuarioLoginDto
        {
            Cpf = "12345678900",
            Senha = senha
        };

        mockRepository.Setup(r => r.ObterPorCpfAsync(dto.Cpf)).ReturnsAsync(usuario);

        // Act
        var resultado = await usuarioService.AutenticarAsync(dto);

        // Assert
        mockRepository.Verify(r => r.ObterPorCpfAsync(dto.Cpf), Times.Once);
        Assert.NotNull(resultado);
        Assert.Equal(usuario.Id, resultado!.Id);
    }

    [Fact]
    public async Task Autenticar_ComSenhaIncorreta_DeveRetornarNull()
    {
        // Arrange
        var usuario = new Usuario("João", new Email("joao@email.com"), new Senha("senhaOriginal"), PerfilUsuario.Funcionario);

        var dto = new UsuarioLoginDto
        {
            Email = "joao@email.com",
            Senha = "senhaErrada"
        };

        mockRepository.Setup(r => r.ObterPorEmailAsync(dto.Email)).ReturnsAsync(usuario);

        // Act
        var resultado = await usuarioService.AutenticarAsync(dto);

        // Assert
        Assert.Null(resultado);
    }

    [Fact]
    public async Task Autenticar_ComUsuarioInexistente_DeveRetornarNull()
    {
        // Arrange
        var dto = new UsuarioLoginDto
        {
            Email = "inexistente@email.com",
            Senha = "123456"
        };

        mockRepository.Setup(r => r.ObterPorEmailAsync(dto.Email)).ReturnsAsync((Usuario?)null);

        // Act
        var resultado = await usuarioService.AutenticarAsync(dto);

        // Assert
        Assert.Null(resultado);
    }
}