﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using reading_list_api.Models;

namespace reading_list_api.Migrations
{
    [DbContext(typeof(ReadingListApiContext))]
    partial class ReadingListApiContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("reading_list_api.Models.Book", b =>
                {
                    b.Property<Guid>("BookId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Authors");

                    b.Property<DateTime>("DateCreated");

                    b.Property<string>("Image");

                    b.Property<Guid>("ReadingListId");

                    b.Property<string>("Title");

                    b.HasKey("BookId");

                    b.HasIndex("ReadingListId");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("reading_list_api.Models.ReadingList", b =>
                {
                    b.Property<Guid>("ReadingListId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Title");

                    b.Property<Guid>("UserId");

                    b.HasKey("ReadingListId");

                    b.HasIndex("UserId");

                    b.ToTable("ReadingLists");
                });

            modelBuilder.Entity("reading_list_api.Models.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Avatar");

                    b.Property<string>("Email");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("reading_list_api.Models.Book", b =>
                {
                    b.HasOne("reading_list_api.Models.ReadingList", "ReadingList")
                        .WithMany("Books")
                        .HasForeignKey("ReadingListId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("reading_list_api.Models.ReadingList", b =>
                {
                    b.HasOne("reading_list_api.Models.User", "User")
                        .WithMany("ReadingLists")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
