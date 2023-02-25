﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WeeControl.ApiApp.Persistence.DbContexts;

#nullable disable

namespace WeeControl.ApiApp.Persistence.Migrations
{
    [DbContext(typeof(EssentialDbContext))]
    [Migration("20230225110943_Third")]
    partial class Third
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("CustomerDboPersonDbo", b =>
                {
                    b.Property<Guid>("CustomerDboCustomerId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("PersonsPersonId")
                        .HasColumnType("char(36)");

                    b.HasKey("CustomerDboCustomerId", "PersonsPersonId");

                    b.HasIndex("PersonsPersonId");

                    b.ToTable("CustomerDboPersonDbo");
                });

            modelBuilder.Entity("WeeControl.Core.Domain.Contexts.Business.Elevator.BuildingDbo", b =>
                {
                    b.Property<Guid>("BuildingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("BuildingAddress")
                        .HasColumnType("longtext");

                    b.Property<string>("BuildingName")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<int>("BuildingType")
                        .HasColumnType("int");

                    b.Property<string>("CountryId")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("varchar(3)");

                    b.Property<double?>("Latitude")
                        .HasMaxLength(90)
                        .HasColumnType("double");

                    b.Property<double?>("Longitude")
                        .HasMaxLength(180)
                        .HasColumnType("double");

                    b.Property<string>("OfficeId")
                        .HasColumnType("longtext");

                    b.HasKey("BuildingId");

                    b.HasIndex("CountryId", "BuildingName")
                        .IsUnique();

                    b.ToTable("BuildingDbo", (string)null);
                });

            modelBuilder.Entity("WeeControl.Core.Domain.Contexts.Business.Elevator.UnitDbo", b =>
                {
                    b.Property<string>("UnitNumber")
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<Guid>("BuildingId")
                        .HasColumnType("char(36)");

                    b.Property<int>("UnitBrand")
                        .HasColumnType("int");

                    b.Property<string>("UnitIdentification")
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<int>("UnitState")
                        .HasColumnType("int");

                    b.Property<int>("UnitType")
                        .HasColumnType("int");

                    b.HasKey("UnitNumber");

                    b.HasIndex("BuildingId");

                    b.ToTable("UnitDbo", (string)null);
                });

            modelBuilder.Entity("WeeControl.Core.Domain.Contexts.Essentials.AddressDbo", b =>
                {
                    b.Property<Guid>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("varchar(25)");

                    b.Property<string>("CountryCode")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("varchar(3)");

                    b.Property<double?>("Latitude")
                        .HasColumnType("double");

                    b.Property<string>("Line1")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("varchar(25)");

                    b.Property<string>("Line2")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("varchar(25)");

                    b.Property<double?>("Longitude")
                        .HasColumnType("double");

                    b.Property<Guid>("PersonId")
                        .HasColumnType("char(36)");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("varchar(5)");

                    b.HasKey("AddressId");

                    b.HasIndex("PersonId");

                    b.ToTable("AddressDbo", (string)null);
                });

            modelBuilder.Entity("WeeControl.Core.Domain.Contexts.Essentials.CustomerDbo", b =>
                {
                    b.Property<Guid>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("CountryCode")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("varchar(3)");

                    b.Property<string>("CustomerAddress")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("CustomerLocalName")
                        .HasMaxLength(25)
                        .HasColumnType("varchar(25)");

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("varchar(25)");

                    b.Property<string>("InvoiceAddress")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("CustomerId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("CustomerDbo", (string)null);
                });

            modelBuilder.Entity("WeeControl.Core.Domain.Contexts.Essentials.EmployeeDbo", b =>
                {
                    b.Property<Guid>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("EmployeeNo")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<Guid>("PersonId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("SupervisorId")
                        .HasColumnType("char(36)");

                    b.HasKey("EmployeeId");

                    b.HasIndex("EmployeeId")
                        .IsUnique();

                    b.HasIndex("PersonId")
                        .IsUnique();

                    b.HasIndex("SupervisorId");

                    b.ToTable("EmployeeDbo", (string)null);
                });

            modelBuilder.Entity("WeeControl.Core.Domain.Contexts.Essentials.PersonContactDbo", b =>
                {
                    b.Property<Guid>("ContactId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("ContactType")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ContactValue")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("PersonId")
                        .HasColumnType("char(36)");

                    b.HasKey("ContactId");

                    b.HasIndex("PersonId");

                    b.ToTable("PersonContactDbo", (string)null);
                });

            modelBuilder.Entity("WeeControl.Core.Domain.Contexts.Essentials.PersonDbo", b =>
                {
                    b.Property<Guid>("PersonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateOnly>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<string>("NationalityCode")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("varchar(3)");

                    b.Property<string>("SecondName")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<string>("ThirdName")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.HasKey("PersonId");

                    b.ToTable("PersonDbo", (string)null);
                });

            modelBuilder.Entity("WeeControl.Core.Domain.Contexts.Essentials.PersonIdentityDbo", b =>
                {
                    b.Property<Guid>("IdentityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("CountryId")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("varchar(3)");

                    b.Property<DateTime?>("ExpireDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("IssueDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("varchar(25)");

                    b.Property<Guid>("PersonId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("varchar(25)");

                    b.HasKey("IdentityId");

                    b.HasIndex("PersonId");

                    b.ToTable("PersonIdentityDbo", (string)null);
                });

            modelBuilder.Entity("WeeControl.Core.Domain.Contexts.Essentials.UserClaimDbo", b =>
                {
                    b.Property<Guid>("ClaimId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("ClaimType")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("varchar(5)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("varchar(255)");

                    b.Property<Guid>("GrantedById")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("GrantedTs")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("RevokedById")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("RevokedTs")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("ClaimId");

                    b.HasIndex("GrantedById");

                    b.HasIndex("RevokedById");

                    b.HasIndex("UserId");

                    b.HasIndex("ClaimType", "ClaimValue")
                        .IsUnique();

                    b.ToTable("UserClaimDbo", (string)null);
                });

            modelBuilder.Entity("WeeControl.Core.Domain.Contexts.Essentials.UserDbo", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<string>("MobileNo")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<Guid>("PersonId")
                        .HasColumnType("char(36)");

                    b.Property<string>("PhotoUrl")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("SuspendArgs")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("TempPassword")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<DateTime?>("TempPasswordTs")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("PersonId")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.HasIndex("Username", "Password");

                    b.ToTable("UserDbo", (string)null);
                });

            modelBuilder.Entity("WeeControl.Core.Domain.Contexts.Essentials.UserFeedsDbo", b =>
                {
                    b.Property<Guid>("FeedId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("FeedBody")
                        .IsRequired()
                        .HasMaxLength(55)
                        .HasColumnType("varchar(55)");

                    b.Property<string>("FeedSubject")
                        .IsRequired()
                        .HasMaxLength(55)
                        .HasColumnType("varchar(55)");

                    b.Property<DateTime>("FeedTs")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)");

                    b.Property<string>("FeedUrl")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime?>("HideTs")
                        .HasColumnType("datetime(6)");

                    b.HasKey("FeedId");

                    b.ToTable("UserFeedsDbo", (string)null);
                });

            modelBuilder.Entity("WeeControl.Core.Domain.Contexts.Essentials.UserNotificationDbo", b =>
                {
                    b.Property<Guid>("NotificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("NotificationUrl")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("PublishedTs")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("ReadTs")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("varchar(25)");

                    b.Property<Guid?>("UserDboUserId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("NotificationId");

                    b.HasIndex("UserDboUserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserNotificationDbo", (string)null);
                });

            modelBuilder.Entity("WeeControl.Core.Domain.Contexts.Essentials.UserSessionDbo", b =>
                {
                    b.Property<Guid>("SessionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedTs")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)");

                    b.Property<string>("DeviceId")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("OneTimePassword")
                        .HasMaxLength(4)
                        .HasColumnType("varchar(4)");

                    b.Property<DateTime?>("TerminationTs")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("UserDboUserId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("SessionId");

                    b.HasIndex("UserDboUserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserSessionDbo", (string)null);
                });

            modelBuilder.Entity("WeeControl.Core.Domain.Contexts.Essentials.UserSessionLogDbo", b =>
                {
                    b.Property<Guid>("LogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Context")
                        .HasColumnType("longtext");

                    b.Property<string>("Details")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("LogTs")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("SessionId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("UserSessionDboSessionId")
                        .HasColumnType("char(36)");

                    b.HasKey("LogId");

                    b.HasIndex("SessionId");

                    b.HasIndex("UserSessionDboSessionId");

                    b.ToTable("UserSessionLogDbo", (string)null);
                });

            modelBuilder.Entity("CustomerDboPersonDbo", b =>
                {
                    b.HasOne("WeeControl.Core.Domain.Contexts.Essentials.CustomerDbo", null)
                        .WithMany()
                        .HasForeignKey("CustomerDboCustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WeeControl.Core.Domain.Contexts.Essentials.PersonDbo", null)
                        .WithMany()
                        .HasForeignKey("PersonsPersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WeeControl.Core.Domain.Contexts.Business.Elevator.UnitDbo", b =>
                {
                    b.HasOne("WeeControl.Core.Domain.Contexts.Business.Elevator.BuildingDbo", "Building")
                        .WithMany("Units")
                        .HasForeignKey("BuildingId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Building");
                });

            modelBuilder.Entity("WeeControl.Core.Domain.Contexts.Essentials.AddressDbo", b =>
                {
                    b.HasOne("WeeControl.Core.Domain.Contexts.Essentials.PersonDbo", "Person")
                        .WithMany("Addresses")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("WeeControl.Core.Domain.Contexts.Essentials.CustomerDbo", b =>
                {
                    b.HasOne("WeeControl.Core.Domain.Contexts.Essentials.UserDbo", "User")
                        .WithOne()
                        .HasForeignKey("WeeControl.Core.Domain.Contexts.Essentials.CustomerDbo", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("WeeControl.Core.Domain.Contexts.Essentials.EmployeeDbo", b =>
                {
                    b.HasOne("WeeControl.Core.Domain.Contexts.Essentials.PersonDbo", "Person")
                        .WithOne()
                        .HasForeignKey("WeeControl.Core.Domain.Contexts.Essentials.EmployeeDbo", "PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WeeControl.Core.Domain.Contexts.Essentials.EmployeeDbo", "Supervisor")
                        .WithMany("Supervise")
                        .HasForeignKey("SupervisorId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Person");

                    b.Navigation("Supervisor");
                });

            modelBuilder.Entity("WeeControl.Core.Domain.Contexts.Essentials.PersonContactDbo", b =>
                {
                    b.HasOne("WeeControl.Core.Domain.Contexts.Essentials.PersonDbo", "Person")
                        .WithMany("Contacts")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("WeeControl.Core.Domain.Contexts.Essentials.PersonIdentityDbo", b =>
                {
                    b.HasOne("WeeControl.Core.Domain.Contexts.Essentials.PersonDbo", "Person")
                        .WithMany("Identities")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("WeeControl.Core.Domain.Contexts.Essentials.UserClaimDbo", b =>
                {
                    b.HasOne("WeeControl.Core.Domain.Contexts.Essentials.EmployeeDbo", "GrantedBy")
                        .WithMany()
                        .HasForeignKey("GrantedById")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("WeeControl.Core.Domain.Contexts.Essentials.EmployeeDbo", "RevokedBy")
                        .WithMany()
                        .HasForeignKey("RevokedById")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("WeeControl.Core.Domain.Contexts.Essentials.UserDbo", null)
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("GrantedBy");

                    b.Navigation("RevokedBy");
                });

            modelBuilder.Entity("WeeControl.Core.Domain.Contexts.Essentials.UserDbo", b =>
                {
                    b.HasOne("WeeControl.Core.Domain.Contexts.Essentials.PersonDbo", "Person")
                        .WithOne()
                        .HasForeignKey("WeeControl.Core.Domain.Contexts.Essentials.UserDbo", "PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("WeeControl.Core.Domain.Contexts.Essentials.UserNotificationDbo", b =>
                {
                    b.HasOne("WeeControl.Core.Domain.Contexts.Essentials.UserDbo", null)
                        .WithMany("Notifications")
                        .HasForeignKey("UserDboUserId");

                    b.HasOne("WeeControl.Core.Domain.Contexts.Essentials.UserDbo", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("WeeControl.Core.Domain.Contexts.Essentials.UserSessionDbo", b =>
                {
                    b.HasOne("WeeControl.Core.Domain.Contexts.Essentials.UserDbo", null)
                        .WithMany("Sessions")
                        .HasForeignKey("UserDboUserId");

                    b.HasOne("WeeControl.Core.Domain.Contexts.Essentials.UserDbo", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("WeeControl.Core.Domain.Contexts.Essentials.UserSessionLogDbo", b =>
                {
                    b.HasOne("WeeControl.Core.Domain.Contexts.Essentials.UserSessionDbo", "UserSession")
                        .WithMany()
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("WeeControl.Core.Domain.Contexts.Essentials.UserSessionDbo", null)
                        .WithMany("Logs")
                        .HasForeignKey("UserSessionDboSessionId");

                    b.Navigation("UserSession");
                });

            modelBuilder.Entity("WeeControl.Core.Domain.Contexts.Business.Elevator.BuildingDbo", b =>
                {
                    b.Navigation("Units");
                });

            modelBuilder.Entity("WeeControl.Core.Domain.Contexts.Essentials.EmployeeDbo", b =>
                {
                    b.Navigation("Supervise");
                });

            modelBuilder.Entity("WeeControl.Core.Domain.Contexts.Essentials.PersonDbo", b =>
                {
                    b.Navigation("Addresses");

                    b.Navigation("Contacts");

                    b.Navigation("Identities");
                });

            modelBuilder.Entity("WeeControl.Core.Domain.Contexts.Essentials.UserDbo", b =>
                {
                    b.Navigation("Claims");

                    b.Navigation("Notifications");

                    b.Navigation("Sessions");
                });

            modelBuilder.Entity("WeeControl.Core.Domain.Contexts.Essentials.UserSessionDbo", b =>
                {
                    b.Navigation("Logs");
                });
#pragma warning restore 612, 618
        }
    }
}
