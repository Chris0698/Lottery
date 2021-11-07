﻿// <auto-generated />
using Lottery;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Lottery.Migrations
{
    [DbContext(typeof(DrawContext))]
    [Migration("20211107165746_Draws")]
    partial class Draws
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Lottery.Draw", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Cost")
                        .HasColumnType("int");

                    b.Property<string>("DrawnNumbers")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PrizeMoney")
                        .HasColumnType("int");

                    b.Property<string>("SelectedGame")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserNumbers")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Draw");
                });
#pragma warning restore 612, 618
        }
    }
}