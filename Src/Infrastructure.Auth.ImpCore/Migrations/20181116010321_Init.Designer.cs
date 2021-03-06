﻿// <auto-generated />
using Infrastructure.Auth.ImpCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Auth.ImpCore.Migrations
{
    [DbContext(typeof(AuthDbContext))]
    [Migration("20181116010321_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024");

            modelBuilder.Entity("Infrastructure.Auth.Shared.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Group");
                });

            modelBuilder.Entity("Infrastructure.Auth.Shared.MappingGroupPermission", b =>
                {
                    b.Property<int>("GroupId");

                    b.Property<int>("PermissionId");

                    b.HasKey("GroupId", "PermissionId");

                    b.HasIndex("PermissionId");

                    b.ToTable("MappingGroupPermission");
                });

            modelBuilder.Entity("Infrastructure.Auth.Shared.MappingRoleGroup", b =>
                {
                    b.Property<int>("RoleId");

                    b.Property<int>("GroupId");

                    b.HasKey("RoleId", "GroupId");

                    b.HasIndex("GroupId");

                    b.ToTable("MappingRoleGroup");
                });

            modelBuilder.Entity("Infrastructure.Auth.Shared.MappingRolePermission", b =>
                {
                    b.Property<int>("RoleId");

                    b.Property<int>("PermissionId");

                    b.HasKey("RoleId", "PermissionId");

                    b.HasIndex("PermissionId");

                    b.ToTable("MappingRolePermission");
                });

            modelBuilder.Entity("Infrastructure.Auth.Shared.MappingUserRole", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("MappingUserRole");
                });

            modelBuilder.Entity("Infrastructure.Auth.Shared.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("Infrastructure.Auth.Shared.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("Infrastructure.Auth.Shared.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Infrastructure.Auth.Shared.MappingGroupPermission", b =>
                {
                    b.HasOne("Infrastructure.Auth.Shared.Group", "Group")
                        .WithMany("MappingGroupPermissions")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Infrastructure.Auth.Shared.Permission", "Permission")
                        .WithMany("MappingGroupPermissions")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Infrastructure.Auth.Shared.MappingRoleGroup", b =>
                {
                    b.HasOne("Infrastructure.Auth.Shared.Group", "Group")
                        .WithMany("MappingRoleGroups")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Infrastructure.Auth.Shared.Role", "Role")
                        .WithMany("MappingRoleGroups")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Infrastructure.Auth.Shared.MappingRolePermission", b =>
                {
                    b.HasOne("Infrastructure.Auth.Shared.Permission", "Permission")
                        .WithMany("MappingRolePermissions")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Infrastructure.Auth.Shared.Role", "Role")
                        .WithMany("MappingRolePermissions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Infrastructure.Auth.Shared.MappingUserRole", b =>
                {
                    b.HasOne("Infrastructure.Auth.Shared.Role", "Role")
                        .WithMany("MappingUserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Infrastructure.Auth.Shared.User", "User")
                        .WithMany("MappingUserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
