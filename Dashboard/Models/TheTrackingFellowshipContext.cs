using Microsoft.EntityFrameworkCore;

namespace Dashboard.Models;

/*
 * Database context.
 */
public partial class TheTrackingFellowshipContext : DbContext
{
    public TheTrackingFellowshipContext()
    {
    }

    public TheTrackingFellowshipContext(DbContextOptions<TheTrackingFellowshipContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Gamertag?> Gamertags { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Gamertag>(entity =>
        {
            entity.HasKey(e => e.GamertagId).HasName("PRIMARY");

            entity.ToTable("gamertag");

            entity.HasIndex(e => e.GamertagId, "gamertag_id").IsUnique();

            entity.HasIndex(e => e.Tag, "tag");

            entity.HasIndex(e => e.UserKey, "user_key");

            entity.Property(e => e.GamertagId).HasColumnName("gamertag_id");
            entity.Property(e => e.Game)
                .HasColumnType("enum('LeagueOfLegends','Valorant','TeamfightTactics')")
                .HasColumnName("game")
                .HasConversion(
                    v => v.ToString(),
                    v => Enum.Parse<GameHandler.Game>(v));
            entity.Property(e => e.Tag)
                .HasMaxLength(50)
                .HasColumnName("tag");
            entity.Property(e => e.UserKey).HasColumnName("user_key");

            entity.HasOne(d => d.UserKeyNavigation).WithMany(p => p.Gamertags)
                .HasForeignKey(d => d.UserKey)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("gamertag_ibfk_1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.Email, "email").IsUnique();

            entity.HasIndex(e => e.UserId, "user_id").IsUnique();

            entity.HasIndex(e => e.Username, "username").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.CreatedOn)
                .HasColumnType("timestamp")
                .HasColumnName("created_on");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.LastLogin)
                .HasColumnType("timestamp")
                .HasColumnName("last_login");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.RefreshId).HasName("PRIMARY");

            entity.ToTable("refreshToken");

            entity.HasIndex(e => e.UserId, "user_id").IsUnique();
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasIndex(e => e.RefreshId, "refresh_id").IsUnique();

            entity.Property(e => e.RefreshKey)
                .HasMaxLength(100)
                .HasColumnName("refresh_key");

            entity.Property(e => e.Token)
                .HasMaxLength(256)
                .HasColumnName("token");

            entity.Property(e => e.RefreshId).HasColumnName("refresh_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}