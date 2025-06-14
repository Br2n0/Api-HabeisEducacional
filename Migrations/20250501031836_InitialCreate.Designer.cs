﻿// <auto-generated />
using System;
using Api_HabeisEducacional.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Api_HabeisEducacional.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250501031836_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Api_HabeisEducacional.Models.Aluno", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Data_Cadastro")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Senha")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Alunos");
                });

            modelBuilder.Entity("Api_HabeisEducacional.Models.Certificado", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Aluno_ID")
                        .HasColumnType("int");

                    b.Property<string>("Codigo_Validacao")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<int>("Curso_ID")
                        .HasColumnType("int");

                    b.Property<DateTime>("Data_Emissao")
                        .HasColumnType("datetime(6)");

                    b.HasKey("ID");

                    b.HasIndex("Aluno_ID");

                    b.HasIndex("Codigo_Validacao")
                        .IsUnique();

                    b.HasIndex("Curso_ID");

                    b.ToTable("Certificados");
                });

            modelBuilder.Entity("Api_HabeisEducacional.Models.Curso", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Descricao")
                        .HasColumnType("longtext");

                    b.Property<int>("Duracao")
                        .HasColumnType("int");

                    b.Property<string>("Instrutor")
                        .HasColumnType("longtext");

                    b.Property<decimal>("Preco")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("Cursos");
                });

            modelBuilder.Entity("Api_HabeisEducacional.Models.Matricula", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Aluno_ID")
                        .HasColumnType("int");

                    b.Property<int>("Curso_ID")
                        .HasColumnType("int");

                    b.Property<DateTime>("Data_Matricula")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("Aluno_ID");

                    b.HasIndex("Curso_ID");

                    b.ToTable("Matriculas");
                });

            modelBuilder.Entity("Api_HabeisEducacional.Models.Certificado", b =>
                {
                    b.HasOne("Api_HabeisEducacional.Models.Aluno", "Aluno")
                        .WithMany("Certificados")
                        .HasForeignKey("Aluno_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Api_HabeisEducacional.Models.Curso", "Curso")
                        .WithMany("Certificados")
                        .HasForeignKey("Curso_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Aluno");

                    b.Navigation("Curso");
                });

            modelBuilder.Entity("Api_HabeisEducacional.Models.Matricula", b =>
                {
                    b.HasOne("Api_HabeisEducacional.Models.Aluno", "Aluno")
                        .WithMany("Matriculas")
                        .HasForeignKey("Aluno_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Api_HabeisEducacional.Models.Curso", "Curso")
                        .WithMany("Matriculas")
                        .HasForeignKey("Curso_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Aluno");

                    b.Navigation("Curso");
                });

            modelBuilder.Entity("Api_HabeisEducacional.Models.Aluno", b =>
                {
                    b.Navigation("Certificados");

                    b.Navigation("Matriculas");
                });

            modelBuilder.Entity("Api_HabeisEducacional.Models.Curso", b =>
                {
                    b.Navigation("Certificados");

                    b.Navigation("Matriculas");
                });
#pragma warning restore 612, 618
        }
    }
}
