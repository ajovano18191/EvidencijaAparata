﻿// <auto-generated />
using System;
using EvidencijaAparata.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EvidencijaAparata.Server.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("EvidencijaAparata.Server.Models.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Naziv")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Cities");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Naziv = "City 1"
                        },
                        new
                        {
                            Id = 2,
                            Naziv = "City 2"
                        },
                        new
                        {
                            Id = 3,
                            Naziv = "City 3"
                        },
                        new
                        {
                            Id = 4,
                            Naziv = "City 4"
                        },
                        new
                        {
                            Id = 5,
                            Naziv = "City 5"
                        });
                });

            modelBuilder.Entity("EvidencijaAparata.Server.Models.GMLocation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Adresa")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("IP")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("MestoId")
                        .HasColumnType("integer");

                    b.Property<string>("Naziv")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("rul_base_id")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("MestoId");

                    b.ToTable("GMLocations");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Adresa = "Adresa 1",
                            IP = "192.168.0.1",
                            MestoId = 1,
                            Naziv = "Lokacija 1",
                            rul_base_id = 1
                        },
                        new
                        {
                            Id = 2,
                            Adresa = "Adresa 2",
                            IP = "192.168.0.2",
                            MestoId = 2,
                            Naziv = "Lokacija 2",
                            rul_base_id = 2
                        },
                        new
                        {
                            Id = 3,
                            Adresa = "Adresa 3",
                            IP = "192.168.0.3",
                            MestoId = 3,
                            Naziv = "Lokacija 3",
                            rul_base_id = 3
                        },
                        new
                        {
                            Id = 4,
                            Adresa = "Adresa 4",
                            IP = "192.168.0.4",
                            MestoId = 4,
                            Naziv = "Lokacija 4",
                            rul_base_id = 4
                        },
                        new
                        {
                            Id = 5,
                            Adresa = "Adresa 5",
                            IP = "192.168.0.5",
                            MestoId = 5,
                            Naziv = "Lokacija 5",
                            rul_base_id = 5
                        });
                });

            modelBuilder.Entity("EvidencijaAparata.Server.Models.GMLocationAct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("DatumAkt")
                        .HasColumnType("date");

                    b.Property<DateOnly?>("DatumDeakt")
                        .HasColumnType("date");

                    b.Property<int>("GMLocationId")
                        .HasColumnType("integer");

                    b.Property<string>("Napomena")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ResenjeAkt")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ResenjeDeakt")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("GMLocationId");

                    b.ToTable("GMLocationActs");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DatumAkt = new DateOnly(2024, 1, 9),
                            DatumDeakt = new DateOnly(2024, 3, 9),
                            GMLocationId = 1,
                            Napomena = "Napomena 1",
                            ResenjeAkt = "ResenjeAkt1",
                            ResenjeDeakt = "ResenjeDeakt1"
                        },
                        new
                        {
                            Id = 2,
                            DatumAkt = new DateOnly(2024, 1, 9),
                            DatumDeakt = new DateOnly(2024, 3, 9),
                            GMLocationId = 2,
                            Napomena = "Napomena 2",
                            ResenjeAkt = "ResenjeAkt2",
                            ResenjeDeakt = "ResenjeDeakt2"
                        },
                        new
                        {
                            Id = 3,
                            DatumAkt = new DateOnly(2024, 4, 9),
                            GMLocationId = 2,
                            Napomena = "Napomena 3",
                            ResenjeAkt = "ResenjeAkt3"
                        });
                });

            modelBuilder.Entity("EvidencijaAparata.Server.Models.GMLocation", b =>
                {
                    b.HasOne("EvidencijaAparata.Server.Models.City", "Mesto")
                        .WithMany()
                        .HasForeignKey("MestoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Mesto");
                });

            modelBuilder.Entity("EvidencijaAparata.Server.Models.GMLocationAct", b =>
                {
                    b.HasOne("EvidencijaAparata.Server.Models.GMLocation", "GMLocation")
                        .WithMany("GMLocationActs")
                        .HasForeignKey("GMLocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GMLocation");
                });

            modelBuilder.Entity("EvidencijaAparata.Server.Models.GMLocation", b =>
                {
                    b.Navigation("GMLocationActs");
                });
#pragma warning restore 612, 618
        }
    }
}
