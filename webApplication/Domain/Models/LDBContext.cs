using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

public partial class LDBContext : DbContext
{
    public LDBContext()
    {
    }

    public LDBContext(DbContextOptions<LDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<attendance> attendances { get; set; }

    public virtual DbSet<city> cities { get; set; }

    public virtual DbSet<group> groups { get; set; }

    public virtual DbSet<institution> institutions { get; set; }

    public virtual DbSet<lecture> lectures { get; set; }

    public virtual DbSet<notification> notifications { get; set; }

    public virtual DbSet<portfolio> portfolios { get; set; }

    public virtual DbSet<room> rooms { get; set; }

    public virtual DbSet<room_equipment> room_equipments { get; set; }

    public virtual DbSet<students_group> students_groups { get; set; }

    public virtual DbSet<user> users { get; set; }

    public virtual DbSet<lectures_group> lectures_groups { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<attendance>(entity =>
        {
            entity.HasKey(e => e.attendanceid).HasName("attendance_pkey");

            entity.ToTable("attendance");

            entity.HasIndex(e => new { e.lectureid, e.userid }, "attendance_lectureid_userid_key").IsUnique();

            entity.Property(e => e.note).HasMaxLength(500);
            entity.Property(e => e.recordedat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.lecture).WithMany(p => p.attendances)
                .HasForeignKey(d => d.lectureid)
                .HasConstraintName("attendance_lectureid_fkey");

            entity.HasOne(d => d.user).WithMany(p => p.attendances)
                .HasForeignKey(d => d.userid)
                .HasConstraintName("attendance_userid_fkey");
        });

        modelBuilder.Entity<city>(entity =>
        {
            entity.HasKey(e => e.cityid).HasName("cities_pkey");

            entity.HasIndex(e => e.cityname, "cities_cityname_key").IsUnique();

            entity.Property(e => e.cityname).HasMaxLength(100);
            entity.Property(e => e.country).HasMaxLength(100);
            entity.Property(e => e.postalcode).HasMaxLength(20);
        });

        modelBuilder.Entity<group>(entity =>
        {
            entity.HasKey(e => e.groupid).HasName("groups_pkey");

            entity.Property(e => e.groupname).HasMaxLength(256);
            entity.Property(e => e.specialty).HasMaxLength(256);

            entity.HasOne(d => d.curator).WithMany(p => p.groups)
                .HasForeignKey(d => d.curatorid)
                .HasConstraintName("groups_curatorid_fkey");

            entity.HasOne(d => d.institution).WithMany(p => p.groups)
                .HasForeignKey(d => d.institutionid)
                .HasConstraintName("groups_institutionid_fkey");

        });

        modelBuilder.Entity<lectures_group>(entity =>
        {
            entity.HasKey(e => new { e.groupid, e.lectureid }).HasName("lectures_groups_pkey");
            entity.ToTable("lectures_groups");

            entity.HasOne(d => d.group)
                .WithMany()
                .HasForeignKey(d => d.groupid)
                .HasConstraintName("lectures_groups_groupid_fkey")
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.lecture)
                .WithMany()
                .HasForeignKey(d => d.lectureid)
                .HasConstraintName("lectures_groups_lectureid_fkey")
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<institution>(entity =>
        {
            entity.HasKey(e => e.institutionid).HasName("institution_pkey");

            entity.ToTable("institution");

            entity.Property(e => e.institutionname).HasMaxLength(256);
            entity.Property(e => e.phone).HasMaxLength(50);
            entity.Property(e => e.street).HasMaxLength(256);
            entity.Property(e => e.website).HasMaxLength(256);

            entity.HasOne(d => d.cityNavigation).WithMany(p => p.institutions)
                .HasForeignKey(d => d.cityid)
                .HasConstraintName("institution_cityid_fkey");
        });

        modelBuilder.Entity<lecture>(entity =>
        {
            entity.HasKey(e => e.lectureid).HasName("lectures_pkey");

            entity.Property(e => e.createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.isactive).HasDefaultValue(true);
            entity.Property(e => e.lecturename).HasMaxLength(256);

            entity.HasOne(d => d.room).WithMany(p => p.lectures)
                .HasForeignKey(d => d.roomid)
                .HasConstraintName("lectures_roomid_fkey");

            entity.HasOne(d => d.teacher).WithMany(p => p.lectures)
                .HasForeignKey(d => d.teacherid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("lectures_teacherid_fkey");
        });

        modelBuilder.Entity<notification>(entity =>
        {
            entity.HasKey(e => e.notificationid).HasName("notifications_pkey");

            entity.Property(e => e.createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.isread).HasDefaultValue(false);

            entity.HasOne(d => d.user).WithMany(p => p.notifications)
                .HasForeignKey(d => d.userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("notifications_userid_fkey");
        });

        modelBuilder.Entity<portfolio>(entity =>
        {
            entity.HasKey(e => new { e.userid, e.achievement }).HasName("portfolio_pkey");

            entity.ToTable("portfolio");

            entity.Property(e => e.achievement).HasMaxLength(256);
            entity.Property(e => e.addedat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.user).WithMany(p => p.portfolios)
                .HasForeignKey(d => d.userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("portfolio_userid_fkey");
        });

        modelBuilder.Entity<room>(entity =>
        {
            entity.HasKey(e => e.roomid).HasName("rooms_pkey");

            entity.HasIndex(e => new { e.roomnumber, e.institutionid }, "rooms_roomnumber_institutionid_key").IsUnique();

            entity.Property(e => e.roomnumber).HasMaxLength(50);

            entity.HasOne(d => d.institution).WithMany(p => p.rooms)
                .HasForeignKey(d => d.institutionid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rooms_institutionid_fkey");
        });

        modelBuilder.Entity<room_equipment>(entity =>
        {
            entity.HasKey(e => new { e.roomid, e.equipment }).HasName("room_equipment_pkey");

            entity.ToTable("room_equipment");

            entity.Property(e => e.equipment).HasMaxLength(256);

            entity.HasOne(d => d.room).WithMany(p => p.room_equipments)
                .HasForeignKey(d => d.roomid)
                .HasConstraintName("room_equipment_roomid_fkey");
        });

        modelBuilder.Entity<students_group>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.enrolledat).HasDefaultValueSql("CURRENT_DATE");

            entity.HasOne(d => d.group).WithMany()
                .HasForeignKey(d => d.groupid)
                .HasConstraintName("students_groups_groupid_fkey");

            entity.HasOne(d => d.user).WithMany()
                .HasForeignKey(d => d.userid)
                .HasConstraintName("students_groups_userid_fkey");
        });

        modelBuilder.Entity<user>(entity =>
        {
            entity.HasKey(e => e.userid).HasName("users_pkey");

            entity.HasIndex(e => e.email, "users_email_key").IsUnique();

            entity.Property(e => e.avatar)
                .HasMaxLength(256)
                .HasDefaultValueSql("'/def.png'::character varying");
            entity.Property(e => e.createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.email).HasMaxLength(256);
            entity.Property(e => e.isactive).HasDefaultValue(true);
            entity.Property(e => e.name).HasMaxLength(256);
            entity.Property(e => e.passwordhash).HasMaxLength(512);
            entity.Property(e => e.patronymic).HasMaxLength(256);
            entity.Property(e => e.role).HasMaxLength(50);
            entity.Property(e => e.surname).HasMaxLength(256);
            entity.Property(e => e.telephonnumber).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
