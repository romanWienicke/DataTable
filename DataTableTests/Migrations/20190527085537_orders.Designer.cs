﻿// <auto-generated />
using DataTableTests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataTableTests.Migrations
{
    [DbContext(typeof(TestDataContext))]
    [Migration("20190527085537_orders")]
    partial class orders
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DataTableTests.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Sum");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Order");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Sum = 11m,
                            UserId = 1
                        },
                        new
                        {
                            Id = 2,
                            Sum = 21m,
                            UserId = 2
                        },
                        new
                        {
                            Id = 3,
                            Sum = 11m,
                            UserId = 1
                        },
                        new
                        {
                            Id = 4,
                            Sum = 41m,
                            UserId = 2
                        },
                        new
                        {
                            Id = 5,
                            Sum = 51m,
                            UserId = 1
                        },
                        new
                        {
                            Id = 6,
                            Sum = 61m,
                            UserId = 2
                        });
                });

            modelBuilder.Entity("DataTableTests.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Roman"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Raimund"
                        });
                });

            modelBuilder.Entity("DataTableTests.Order", b =>
                {
                    b.HasOne("DataTableTests.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}