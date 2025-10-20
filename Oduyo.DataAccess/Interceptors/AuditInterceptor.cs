using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Oduyo.Domain.Entities;
using System.Security.Claims;

namespace Oduyo.DataAccess.Interceptors
{
    /// <summary>
    /// SaveChanges sırasında otomatik audit trail yönetimi sağlar.
    /// CreatedAt, CreatedBy, UpdatedAt, UpdatedBy, DeletedAt, DeletedBy alanlarını otomatik doldurur.
    /// </summary>
    public class AuditInterceptor : SaveChangesInterceptor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void UpdateEntities(DbContext context)
        {
            if (context == null) return;

            var currentUserId = GetCurrentUserId();
            var now = DateTime.UtcNow;

            foreach (var entry in context.ChangeTracker.Entries<EntityBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        // Yeni kayıt - CreatedAt ve CreatedBy ayarla
                        entry.Property(nameof(EntityBase.CreatedAt)).CurrentValue = now;
                        entry.Property(nameof(EntityBase.CreatedBy)).CurrentValue = currentUserId;
                        break;

                    case EntityState.Modified:
                        // Güncelleme - UpdatedAt ve UpdatedBy ayarla
                        // CreatedAt ve CreatedBy değişmemeli
                        entry.Property(nameof(EntityBase.CreatedAt)).IsModified = false;
                        entry.Property(nameof(EntityBase.CreatedBy)).IsModified = false;

                        entry.Property(nameof(EntityBase.UpdatedAt)).CurrentValue = now;
                        entry.Property(nameof(EntityBase.UpdatedBy)).CurrentValue = currentUserId;
                        break;

                    case EntityState.Deleted:
                        // Soft delete - DeletedAt ve DeletedBy ayarla
                        if (!entry.Entity.IsHardDelete)
                        {
                            entry.State = EntityState.Modified;
                            entry.Property(nameof(EntityBase.DeletedAt)).CurrentValue = now;
                            entry.Property(nameof(EntityBase.DeletedBy)).CurrentValue = currentUserId;
                        }
                        break;
                }
            }
        }

        private int GetCurrentUserId()
        {
            // HTTP context'ten kullanıcı ID'sini al
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext?.User?.Identity?.IsAuthenticated == true)
            {
                var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)
                    ?? httpContext.User.FindFirst("sub")
                    ?? httpContext.User.FindFirst("userId");

                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    return userId;
                }
            }

            // HTTP context yoksa veya authenticated değilse System user kullan
            // SystemUserId.System = -1 (arka plan işlemleri için)
            return -1;
        }
    }
}
