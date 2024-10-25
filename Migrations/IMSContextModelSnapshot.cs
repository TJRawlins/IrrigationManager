﻿// <auto-generated />
using IrrigationManager.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace IrrigationManager.Migrations
{
    [DbContext(typeof(IMSContext))]
    partial class IMSContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("IrrigationManager.Models.Plant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<decimal>("EmitterGPH")
                        .HasColumnType("decimal(5,2)");

                    b.Property<int>("EmittersPerPlant")
                        .HasColumnType("int");

                    b.Property<string>("Exposure")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<decimal>("GalsPerWk")
                        .HasColumnType("decimal(5,2)");

                    b.Property<decimal>("GalsPerWkCalc")
                        .HasColumnType("decimal(5,2)");

                    b.Property<int>("HardinessZone")
                        .HasColumnType("int");

                    b.Property<string>("HarvestMonth")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("TimeStamp")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<int>("ZoneId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ZoneId");

                    b.ToTable("Plants");
                });

            modelBuilder.Entity("IrrigationManager.Models.Season", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<string>("TimeStamp")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TotalGalPerMonth")
                        .HasColumnType("decimal(11,2)");

                    b.Property<decimal>("TotalGalPerWeek")
                        .HasColumnType("decimal(11,2)");

                    b.Property<decimal>("TotalGalPerYear")
                        .HasColumnType("decimal(11,2)");

                    b.Property<int>("TotalZones")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Season");
                });

            modelBuilder.Entity("IrrigationManager.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("IrrigationManager.Models.Zone", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("EndHours")
                        .HasColumnType("int");

                    b.Property<int>("EndMinutes")
                        .HasColumnType("int");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<int>("RuntimeHours")
                        .HasColumnType("int");

                    b.Property<int>("RuntimeMinutes")
                        .HasColumnType("int");

                    b.Property<int>("RuntimePerMonth")
                        .HasColumnType("int");

                    b.Property<int>("RuntimePerWeek")
                        .HasColumnType("int");

                    b.Property<string>("Season")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<int>("SeasonId")
                        .HasColumnType("int");

                    b.Property<int>("StartHours")
                        .HasColumnType("int");

                    b.Property<int>("StartMinutes")
                        .HasColumnType("int");

                    b.Property<string>("TimeStamp")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TotalGalPerMonth")
                        .HasColumnType("decimal(11,2)");

                    b.Property<decimal>("TotalGalPerWeek")
                        .HasColumnType("decimal(11,2)");

                    b.Property<decimal>("TotalGalPerYear")
                        .HasColumnType("decimal(11,2)");

                    b.Property<int>("TotalPlants")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SeasonId");

                    b.ToTable("Zones");
                });

            modelBuilder.Entity("IrrigationManager.Models.Plant", b =>
                {
                    b.HasOne("IrrigationManager.Models.Zone", "Zones")
                        .WithMany("Plants")
                        .HasForeignKey("ZoneId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Zones");
                });

            modelBuilder.Entity("IrrigationManager.Models.Zone", b =>
                {
                    b.HasOne("IrrigationManager.Models.Season", "Seasons")
                        .WithMany("Zones")
                        .HasForeignKey("SeasonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Seasons");
                });

            modelBuilder.Entity("IrrigationManager.Models.Season", b =>
                {
                    b.Navigation("Zones");
                });

            modelBuilder.Entity("IrrigationManager.Models.Zone", b =>
                {
                    b.Navigation("Plants");
                });
#pragma warning restore 612, 618
        }
    }
}
