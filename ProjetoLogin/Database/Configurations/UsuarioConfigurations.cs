﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ProjetoLogin.Models;

namespace ProjetoLogin.Database.Configurations
{
	public class UsuarioConfigurations : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            _ = builder.HasKey(u => u.Id);

            _ = builder.Property(u => u.NomeCommpleto)
                .HasMaxLength(Pessoa.NomeMaxLength)
                .IsRequired();

            _ = builder.Property(u => u.Password)
				.HasMaxLength(Usuario.PasswordHashMaxLength)
				.IsRequired();

			_ = builder.Property(u => u.DataNascimento)
                .IsRequired();

            _ = builder.Property(u => u.CpfCnpj)
                .HasMaxLength(Pessoa.CpfCnpjMaxLength)
                .IsRequired();

            _ = builder.Property(u => u.Telefone)
                .HasMaxLength(Pessoa.TelefoneMaxLength)
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