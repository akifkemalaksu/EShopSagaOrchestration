﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SagaStateMachineWorkerService.Contexts;

#nullable disable

namespace SagaStateMachineWorkerService.Migrations
{
    [DbContext(typeof(OrderStateDbContext))]
    [Migration("20240526103746_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SagaStateMachineWorkerService.Models.OrderStateInstance", b =>
                {
                    b.Property<Guid>("CorrelationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BuyerId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("CurrentState")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("Version")
                        .HasColumnType("int");

                    b.HasKey("CorrelationId");

                    b.ToTable("OrderStateInstance");
                });

            modelBuilder.Entity("SagaStateMachineWorkerService.Models.OrderStateInstance", b =>
                {
                    b.OwnsOne("SagaStateMachineWorkerService.Models.Payment", "Payment", b1 =>
                        {
                            b1.Property<Guid>("OrderStateInstanceCorrelationId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("CVV")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("CardName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("CardNumber")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Expiration")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<decimal>("TotalPrice")
                                .HasColumnType("decimal(18,2)");

                            b1.HasKey("OrderStateInstanceCorrelationId");

                            b1.ToTable("OrderStateInstance");

                            b1.WithOwner()
                                .HasForeignKey("OrderStateInstanceCorrelationId");
                        });

                    b.Navigation("Payment")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
