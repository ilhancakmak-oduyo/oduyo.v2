using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Oduyo.Domain.Entities
{
    // Base Class
    public class EntityBase : INotifyPropertyChanged
    {
        [Key] public int Id { get; set; }
        [BindNever, IgnoreDataMember] public int CreatedBy { get; private set; }
        [BindNever, IgnoreDataMember] public int? UpdatedBy { get; private set; }
        [BindNever, IgnoreDataMember] public int? DeletedBy { get; private set; }
        [BindNever, IgnoreDataMember] public DateTime CreatedAt { get; private set; }
        [BindNever, IgnoreDataMember] public DateTime? UpdatedAt { get; private set; }
        [BindNever, IgnoreDataMember] public DateTime? DeletedAt { get; private set; }
        [NotMapped] public bool IsHardDelete { get; set; } = false;

        public void SetCreated(int userId) => SetCreated(userId, DateTime.UtcNow);
        public void SetCreated(int userId, DateTime createdAt)
        {
            CreatedAt = createdAt;
            CreatedBy = userId;
        }

        public void SetUpdated(int userId) => SetUpdated(userId, DateTime.UtcNow);
        public void SetUpdated(int userId, DateTime updatedAt)
        {
            UpdatedAt = updatedAt;
            UpdatedBy = userId;
        }

        public void SetDeleted(int userId) => SetDeleted(userId, DateTime.UtcNow);
        public void SetDeleted(int userId, DateTime deletedAt)
        {
            DeletedAt = deletedAt;
            DeletedBy = userId;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetProperty<T>(ref T backingField, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingField, value)) return false;
            backingField = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
