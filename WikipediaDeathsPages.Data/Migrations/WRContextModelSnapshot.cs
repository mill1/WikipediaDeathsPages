﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WikipediaDeathsPages.Data.Models;

namespace WikipediaDeathsPages.Data.Migrations
{
    [DbContext(typeof(WRContext))]
    partial class WRContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WikipediaDeathsPages.Data.Models.Olympians", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FullGivenName")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("FullSurname")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(1)")
                        .HasMaxLength(1);

                    b.Property<string>("Noc1")
                        .HasColumnName("NOC1")
                        .HasColumnType("nvarchar(3)")
                        .HasMaxLength(3);

                    b.Property<string>("Noc2")
                        .HasColumnName("NOC2")
                        .HasColumnType("nvarchar(3)")
                        .HasMaxLength(3);

                    b.Property<string>("Noc3")
                        .HasColumnName("NOC3")
                        .HasColumnType("nvarchar(3)")
                        .HasMaxLength(3);

                    b.Property<string>("Noc4")
                        .HasColumnName("NOC4")
                        .HasColumnType("nvarchar(3)")
                        .HasMaxLength(3);

                    b.Property<string>("Season")
                        .HasColumnType("nvarchar(1)")
                        .HasMaxLength(1);

                    b.Property<string>("Sport1")
                        .HasColumnType("nvarchar(3)")
                        .HasMaxLength(3);

                    b.Property<string>("Sport2")
                        .HasColumnType("nvarchar(3)")
                        .HasMaxLength(3);

                    b.Property<string>("Sport3")
                        .HasColumnType("nvarchar(3)")
                        .HasMaxLength(3);

                    b.Property<string>("Sport4")
                        .HasColumnType("nvarchar(3)")
                        .HasMaxLength(3);

                    b.Property<string>("UsedGivenName")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("UsedSurname")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<int?>("Year1")
                        .HasColumnType("int");

                    b.Property<int?>("Year10")
                        .HasColumnType("int");

                    b.Property<int?>("Year2")
                        .HasColumnType("int");

                    b.Property<int?>("Year3")
                        .HasColumnType("int");

                    b.Property<int?>("Year4")
                        .HasColumnType("int");

                    b.Property<int?>("Year5")
                        .HasColumnType("int");

                    b.Property<int?>("Year6")
                        .HasColumnType("int");

                    b.Property<int?>("Year7")
                        .HasColumnType("int");

                    b.Property<int?>("Year8")
                        .HasColumnType("int");

                    b.Property<int?>("Year9")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Olympians");
                });

            modelBuilder.Entity("WikipediaDeathsPages.Data.Models.References", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AccessDate")
                        .HasColumnType("date");

                    b.Property<string>("Agency")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ArchiveDate")
                        .HasColumnType("date");

                    b.Property<string>("ArticleTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("Author1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Authorlink1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("date");

                    b.Property<DateTime>("DeathDate")
                        .HasColumnType("date");

                    b.Property<string>("Language")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastNameSubject")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Page")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Publisher")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Quote")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SourceCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(35)")
                        .HasMaxLength(35);

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(35)")
                        .HasMaxLength(35);

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UrlAccess")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Work")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("SourceCode");

                    b.ToTable("References");
                });

            modelBuilder.Entity("WikipediaDeathsPages.Data.Models.Sources", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(35)")
                        .HasMaxLength(35);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.HasKey("Code");

                    b.ToTable("Sources");
                });

            modelBuilder.Entity("WikipediaDeathsPages.Data.Models.References", b =>
                {
                    b.HasOne("WikipediaDeathsPages.Data.Models.Sources", "SourceCodeNavigation")
                        .WithMany("References")
                        .HasForeignKey("SourceCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
