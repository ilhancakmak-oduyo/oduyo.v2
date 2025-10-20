using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;

namespace Oduyo.Infrastructure.Interfaces
{
    public interface IEmailTemplateService
    {
        Task<EmailTemplate> CreateTemplateAsync(CreateEmailTemplateDto dto);
        Task<EmailTemplate> UpdateTemplateAsync(int templateId, UpdateEmailTemplateDto dto);
        Task<bool> DeleteTemplateAsync(int templateId);
        Task<EmailTemplate> GetTemplateByIdAsync(int templateId);
        Task<EmailTemplate> GetTemplateByKeyAsync(string templateKey);
        Task<List<EmailTemplate>> GetAllTemplatesAsync();
        Task<List<EmailTemplate>> GetActiveTemplatesAsync();
    }
}