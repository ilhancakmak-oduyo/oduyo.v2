using Microsoft.EntityFrameworkCore;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly ApplicationDbContext _context;

        public EmailTemplateService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<EmailTemplate> CreateTemplateAsync(CreateEmailTemplateDto dto)
        {
            var template = new EmailTemplate
            {
                Name = dto.Name,
                Subject = dto.Subject,
                Body = dto.Body,
                TemplateKey = dto.TemplateKey,
                IsActive = true
            };

            _context.EmailTemplates.Add(template);
            await _context.SaveChangesAsync();

            return template;
        }

        public async Task<EmailTemplate> UpdateTemplateAsync(int templateId, UpdateEmailTemplateDto dto)
        {
            var template = await _context.EmailTemplates.FindAsync(templateId);
            if (template == null)
                throw new InvalidOperationException("Şablon bulunamadı.");

            template.Name = dto.Name;
            template.Subject = dto.Subject;
            template.Body = dto.Body;
            template.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
            return template;
        }

        public async Task<bool> DeleteTemplateAsync(int templateId)
        {
            var template = await _context.EmailTemplates.FindAsync(templateId);
            if (template == null)
                return false;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<EmailTemplate> GetTemplateByIdAsync(int templateId)
        {
            return await _context.EmailTemplates.FirstOrDefaultAsync(t => t.Id == templateId);
        }

        public async Task<EmailTemplate> GetTemplateByKeyAsync(string templateKey)
        {
            return await _context.EmailTemplates
                .FirstOrDefaultAsync(t => t.TemplateKey == templateKey && t.IsActive);
        }

        public async Task<List<EmailTemplate>> GetAllTemplatesAsync()
        {
            return await _context.EmailTemplates.ToListAsync();
        }

        public async Task<List<EmailTemplate>> GetActiveTemplatesAsync()
        {
            return await _context.EmailTemplates.Where(t => t.IsActive).ToListAsync();
        }
    }
}