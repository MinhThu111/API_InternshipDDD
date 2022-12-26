using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using API.Internship.Domain.Models;

namespace API.Internship.Infrastructure.Data
{
    public partial class InternshipContext : DbContext
    {
        public InternshipContext()
        {
        }

        public InternshipContext(DbContextOptions<InternshipContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<Folk> Folks { get; set; }
        public virtual DbSet<Grade> Grades { get; set; }
        public virtual DbSet<GradeStudent> GradeStudents { get; set; }
        public virtual DbSet<Nationality> Nationalities { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<NewsCategory> NewsCategories { get; set; }
        public virtual DbSet<ParentContact> ParentContacts { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<PersonType> PersonTypes { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Province> Provinces { get; set; }
        public virtual DbSet<Religion> Religions { get; set; }
        public virtual DbSet<SchoolInfo> SchoolInfos { get; set; }
        public virtual DbSet<Score> Scores { get; set; }
        public virtual DbSet<ScoreType> ScoreTypes { get; set; }
        public virtual DbSet<StudentParentContact> StudentParentContacts { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<TeacherSubject> TeacherSubjects { get; set; }
        public virtual DbSet<Ward> Wards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=172.16.23.5;Database=Internship;User Id=thuntm; Password=inter@202210;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Address");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.AddressNumber)
                    .HasMaxLength(100)
                    .HasColumnName("address_number");

                entity.Property(e => e.AddressText)
                    .HasMaxLength(100)
                    .HasColumnName("address_text");

                entity.Property(e => e.CountryId).HasColumnName("country_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.DistrictId).HasColumnName("district_id");

                entity.Property(e => e.Latitude).HasColumnName("latitude");

                entity.Property(e => e.Longitude).HasColumnName("longitude");

                entity.Property(e => e.ProvinceId).HasColumnName("province_id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Timer)
                    .HasColumnType("datetime")
                    .HasColumnName("timer");

                entity.Property(e => e.Title)
                    .HasMaxLength(150)
                    .HasColumnName("title");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

                entity.Property(e => e.WardId).HasColumnName("ward_id");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("Country");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CountryCode)
                    .HasMaxLength(5)
                    .HasColumnName("country_code");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.NameSlug)
                    .HasMaxLength(100)
                    .HasColumnName("name_slug");

                entity.Property(e => e.Remark)
                    .HasMaxLength(100)
                    .HasColumnName("remark");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Timer)
                    .HasColumnType("datetime")
                    .HasColumnName("timer");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            });

            modelBuilder.Entity<District>(entity =>
            {
                entity.ToTable("District");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.DistrictCode)
                    .HasMaxLength(5)
                    .HasColumnName("district_code");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.NameSlug)
                    .HasMaxLength(100)
                    .HasColumnName("name_slug");

                entity.Property(e => e.ProvinceId).HasColumnName("province_id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Timer)
                    .HasColumnType("datetime")
                    .HasColumnName("timer");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

                entity.HasOne(d => d.Province)
                    .WithMany(p => p.Districts)
                    .HasForeignKey(d => d.ProvinceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_District_Province");
            });

            modelBuilder.Entity<Folk>(entity =>
            {
                entity.ToTable("Folk");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.NameSlug)
                    .HasMaxLength(100)
                    .HasColumnName("name_slug");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Timer)
                    .HasColumnType("datetime")
                    .HasColumnName("timer");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            });

            modelBuilder.Entity<Grade>(entity =>
            {
                entity.ToTable("Grade");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.GradeCode)
                    .HasMaxLength(10)
                    .HasColumnName("grade_code");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Remark)
                    .HasMaxLength(250)
                    .HasColumnName("remark");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.TeacherId)
                    .HasColumnName("teacher_id")
                    .HasComment("Id lấy từ person. Xác định GVCN");

                entity.Property(e => e.Timer)
                    .HasColumnType("datetime")
                    .HasColumnName("timer");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.Grades)
                    .HasForeignKey(d => d.TeacherId)
                    .HasConstraintName("FK_Class_Person");
            });

            modelBuilder.Entity<GradeStudent>(entity =>
            {
                entity.ToTable("GradeStudent");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.GradeId).HasColumnName("grade_id");

                entity.Property(e => e.PositionId).HasColumnName("position_id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.StudentId)
                    .HasColumnName("student_id")
                    .HasComment("id lấy từ person");

                entity.Property(e => e.Timer)
                    .HasColumnType("datetime")
                    .HasColumnName("timer");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

                entity.HasOne(d => d.Grade)
                    .WithMany(p => p.GradeStudents)
                    .HasForeignKey(d => d.GradeId)
                    .HasConstraintName("FK_ClassPerson_Class");

                entity.HasOne(d => d.Position)
                    .WithMany(p => p.GradeStudents)
                    .HasForeignKey(d => d.PositionId)
                    .HasConstraintName("FK_GradeStudent_Position");
            });

            modelBuilder.Entity<Nationality>(entity =>
            {
                entity.ToTable("Nationality");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.NameSlug)
                    .HasMaxLength(100)
                    .HasColumnName("name_slug");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Timer)
                    .HasColumnType("datetime")
                    .HasColumnName("timer");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            });

            modelBuilder.Entity<News>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.AvatarUrl)
                    .HasMaxLength(250)
                    .HasColumnName("avatar_url");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .HasColumnName("description")
                    .HasComment("id lấy từ person. Xác định GVCN");

                entity.Property(e => e.Detail).HasColumnName("detail");

                entity.Property(e => e.NewsCategoryId).HasColumnName("news_category_id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Timer)
                    .HasColumnType("datetime")
                    .HasColumnName("timer");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .HasColumnName("title");

                entity.Property(e => e.TitleSlug)
                    .HasMaxLength(100)
                    .HasColumnName("title_slug");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

                entity.HasOne(d => d.NewsCategory)
                    .WithMany(p => p.News)
                    .HasForeignKey(d => d.NewsCategoryId)
                    .HasConstraintName("FK_News_NewsCategory");
            });

            modelBuilder.Entity<NewsCategory>(entity =>
            {
                entity.ToTable("NewsCategory");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("name");

                entity.Property(e => e.ParentId).HasColumnName("parent_id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Timer)
                    .HasColumnType("datetime")
                    .HasColumnName("timer");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasComment("0:Giới thiệu;1:Tin tức;2:Thông báo");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            });

            modelBuilder.Entity<ParentContact>(entity =>
            {
                entity.ToTable("ParentContact");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.AddressId).HasColumnName("address_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.Email)
                    .HasMaxLength(500)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("first_name");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("last_name");

                entity.Property(e => e.MobileNumber)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("mobile_number")
                    .IsFixedLength();

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("phone_number")
                    .IsFixedLength();

                entity.Property(e => e.Remark)
                    .HasMaxLength(250)
                    .HasColumnName("remark");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Timer)
                    .HasColumnType("datetime")
                    .HasColumnName("timer");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("Person");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.AddressId).HasColumnName("address_id");

                entity.Property(e => e.AvatarUrl)
                    .HasMaxLength(250)
                    .HasColumnName("avatar_url");

                entity.Property(e => e.Birthday)
                    .HasColumnType("datetime")
                    .HasColumnName("birthday");

                entity.Property(e => e.Code)
                    .HasMaxLength(10)
                    .HasColumnName("code");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("first_name");

                entity.Property(e => e.FirstNameSlug)
                    .HasMaxLength(20)
                    .HasColumnName("first_name_slug");

                entity.Property(e => e.FolkId).HasColumnName("folk_id");

                entity.Property(e => e.Gender).HasColumnName("gender");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("last_name");

                entity.Property(e => e.LastNameSlug)
                    .HasMaxLength(30)
                    .HasColumnName("last_name_slug");

                entity.Property(e => e.NationalityId).HasColumnName("nationality_id");

                entity.Property(e => e.PersonTypeId)
                    .HasColumnName("person_type_id")
                    .HasComment("Dùng để xác định GV hay HS");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .HasColumnName("phone_number");

                entity.Property(e => e.ReligionId).HasColumnName("religion_id");

                entity.Property(e => e.Remark)
                    .HasMaxLength(250)
                    .HasColumnName("remark");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Timer)
                    .HasColumnType("datetime")
                    .HasColumnName("timer");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

                entity.HasOne(d => d.PersonType)
                    .WithMany(p => p.People)
                    .HasForeignKey(d => d.PersonTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Person_PersonType");
            });

            modelBuilder.Entity<PersonType>(entity =>
            {
                entity.ToTable("PersonType");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Remark)
                    .HasMaxLength(150)
                    .HasColumnName("remark");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Timer)
                    .HasColumnType("datetime")
                    .HasColumnName("timer");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.ToTable("Position");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.PositionCode)
                    .HasMaxLength(10)
                    .HasColumnName("position_code");

                entity.Property(e => e.Remark)
                    .HasMaxLength(250)
                    .HasColumnName("remark")
                    .IsFixedLength();

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Timer)
                    .HasColumnType("datetime")
                    .HasColumnName("timer");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            });

            modelBuilder.Entity<Province>(entity =>
            {
                entity.ToTable("Province");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CountryId).HasColumnName("country_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.NameSlug)
                    .HasMaxLength(100)
                    .HasColumnName("name_slug");

                entity.Property(e => e.ProvinceCode)
                    .HasMaxLength(5)
                    .HasColumnName("province_code");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Timer)
                    .HasColumnType("datetime")
                    .HasColumnName("timer");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            });

            modelBuilder.Entity<Religion>(entity =>
            {
                entity.ToTable("Religion");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.Description)
                    .HasMaxLength(150)
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.NameSlug)
                    .HasMaxLength(100)
                    .HasColumnName("name_slug");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Timer)
                    .HasColumnType("datetime")
                    .HasColumnName("timer");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            });

            modelBuilder.Entity<SchoolInfo>(entity =>
            {
                entity.ToTable("SchoolInfo");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.AddressId).HasColumnName("address_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.EstablishDate)
                    .HasColumnType("datetime")
                    .HasColumnName("establish_date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasColumnName("name");

                entity.Property(e => e.NameEn)
                    .HasMaxLength(250)
                    .HasColumnName("name_en");

                entity.Property(e => e.Sologan)
                    .HasMaxLength(150)
                    .HasColumnName("sologan");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Timer)
                    .HasColumnType("datetime")
                    .HasColumnName("timer");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            });

            modelBuilder.Entity<Score>(entity =>
            {
                entity.ToTable("Score");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.Remark)
                    .HasMaxLength(250)
                    .HasColumnName("remark")
                    .IsFixedLength();

                entity.Property(e => e.Score1).HasColumnName("score");

                entity.Property(e => e.ScoreTypeId).HasColumnName("score_type_id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.StudentId).HasColumnName("student_id");

                entity.Property(e => e.SubjectId).HasColumnName("subject_id");

                entity.Property(e => e.Timer)
                    .HasColumnType("datetime")
                    .HasColumnName("timer");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

                entity.HasOne(d => d.ScoreType)
                    .WithMany(p => p.Scores)
                    .HasForeignKey(d => d.ScoreTypeId)
                    .HasConstraintName("FK_Score_ScoreType");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Scores)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Score_Person");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.Scores)
                    .HasForeignKey(d => d.SubjectId)
                    .HasConstraintName("FK_Score_Subject");
            });

            modelBuilder.Entity<ScoreType>(entity =>
            {
                entity.ToTable("ScoreType");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Remark)
                    .HasMaxLength(150)
                    .HasColumnName("remark");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Timer)
                    .HasColumnType("datetime")
                    .HasColumnName("timer");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            });

            modelBuilder.Entity<StudentParentContact>(entity =>
            {
                entity.ToTable("StudentParentContact");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.ParentContactId).HasColumnName("parent_contact_id");

                entity.Property(e => e.Remark)
                    .HasMaxLength(250)
                    .HasColumnName("remark");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.StudentId).HasColumnName("student_id");

                entity.Property(e => e.Timer)
                    .HasColumnType("datetime")
                    .HasColumnName("timer");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

                entity.HasOne(d => d.ParentContact)
                    .WithMany(p => p.StudentParentContacts)
                    .HasForeignKey(d => d.ParentContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentParentContact_ParentContact");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.StudentParentContacts)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentParentContact_Person");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.ToTable("Subject");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.SubjectCode)
                    .HasMaxLength(10)
                    .HasColumnName("subject_code")
                    .IsFixedLength();

                entity.Property(e => e.Timer)
                    .HasColumnType("datetime")
                    .HasColumnName("timer");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            });

            modelBuilder.Entity<TeacherSubject>(entity =>
            {
                entity.ToTable("TeacherSubject");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.Remark)
                    .HasMaxLength(50)
                    .HasColumnName("remark");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.SubjectId).HasColumnName("subject_id");

                entity.Property(e => e.TeacherId).HasColumnName("teacher_id");

                entity.Property(e => e.Timer)
                    .HasColumnType("datetime")
                    .HasColumnName("timer");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.TeacherSubjects)
                    .HasForeignKey(d => d.SubjectId)
                    .HasConstraintName("FK_TeacherSubject_Subject");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.TeacherSubjects)
                    .HasForeignKey(d => d.TeacherId)
                    .HasConstraintName("FK_TeacherSubject_Person");
            });

            modelBuilder.Entity<Ward>(entity =>
            {
                entity.ToTable("Ward");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.DistrictId).HasColumnName("district_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.NameSlug)
                    .HasMaxLength(100)
                    .HasColumnName("name_slug");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Timer)
                    .HasColumnType("datetime")
                    .HasColumnName("timer");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

                entity.Property(e => e.WardCode)
                    .HasMaxLength(5)
                    .HasColumnName("ward_code");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.Wards)
                    .HasForeignKey(d => d.DistrictId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ward_District");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
