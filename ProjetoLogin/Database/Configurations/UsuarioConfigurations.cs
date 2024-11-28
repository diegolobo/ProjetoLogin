using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ProjetoLogin.Models.Usuarios;

namespace ProjetoLogin.Database.Configurations
{
	public class UsuarioConfigurations : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            _ = builder.HasKey(u => u.Id);

            _ = builder.Property(u => u.Nome)
                .HasMaxLength(Usuario.NomeMaxLength)
                .IsRequired();

            _ = builder.Property(u => u.Senha)
				.HasMaxLength(Usuario.PasswordHashMaxLength)
				.IsRequired();

            _ = builder.Property(u => u.Email)
                .HasMaxLength(Usuario.EmailMaxLength)
                .IsRequired();

            _ = builder.Property(u => u.TipoUsuario)
                .HasConversion<int>()
                .IsRequired();
            
            _ = builder.Property(u => u.Status)
                .HasConversion<int>()
                .IsRequired();
        }
    }
}
