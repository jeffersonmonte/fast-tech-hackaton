using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FastTech.Autenticacao.Domain.Entities;

namespace FastTech.Autenticacao.Infrastructure.Persistance.Mappings
{
    public class UsuarioConfiguracao : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuario");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Nome)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(u => u.Perfil)
                .IsRequired();

            builder.Property(u => u.Ativo)
                .IsRequired();

            builder.Property(u => u.DataCriacao)
                .IsRequired();

            builder.OwnsOne(u => u.Email, email =>
            {
                email.Property(e => e.Endereco)
                    .HasColumnName("Email")
                    .HasMaxLength(150)
                    .IsRequired();
            });

            builder.OwnsOne(u => u.Senha, senha =>
            {
                senha.Property(s => s.Hash)
                    .HasColumnName("SenhaHash")
                    .HasMaxLength(512)
                    .IsRequired();
            });

            builder.OwnsOne(u => u.Cpf, cpf =>
            {
                cpf.Property(c => c.Numero)
                    .HasMaxLength(11)
                    .HasColumnName("Cpf")
                    .IsRequired(false);
            });
        }
    }
}
