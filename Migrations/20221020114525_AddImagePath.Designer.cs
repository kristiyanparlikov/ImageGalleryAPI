﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace photo_gallery_api.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20221020114525_AddImagePath")]
    partial class AddImagePath
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("photo_gallery_api.Entities.Favourite", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("ImageId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "ImageId");

                    b.HasIndex("ImageId");

                    b.ToTable("Favourites");
                });

            modelBuilder.Entity("photo_gallery_api.Entities.Image", b =>
                {
                    b.Property<int>("ImageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ImageId"), 1L, 1);

                    b.Property<string>("ImageName")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("ImageId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("photo_gallery_api.Entities.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("photo_gallery_api.Entities.Favourite", b =>
                {
                    b.HasOne("photo_gallery_api.Entities.Image", "Image")
                        .WithMany("Favourites")
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("photo_gallery_api.Entities.User", "User")
                        .WithMany("Favourites")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Image");

                    b.Navigation("User");
                });

            modelBuilder.Entity("photo_gallery_api.Entities.Image", b =>
                {
                    b.Navigation("Favourites");
                });

            modelBuilder.Entity("photo_gallery_api.Entities.User", b =>
                {
                    b.Navigation("Favourites");
                });
#pragma warning restore 612, 618
        }
    }
}
