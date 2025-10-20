using Microsoft.EntityFrameworkCore;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class DemoFeedbackService : IDemoFeedbackService
    {
        private readonly ApplicationDbContext _context;

        public DemoFeedbackService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DemoFeedback> CreateFeedbackAsync(CreateDemoFeedbackDto dto)
        {
            // Demo için zaten feedback var mı kontrol et
            var existingFeedback = await _context.DemoFeedbacks
                .FirstOrDefaultAsync(df => df.DemoId == dto.DemoId);

            if (existingFeedback != null)
                throw new InvalidOperationException("Bu demo için zaten feedback mevcut. Güncelleme için UpdateFeedbackAsync kullanın.");

            var feedback = new DemoFeedback
            {
                DemoId = dto.DemoId,
                Feedback = dto.Feedback
            };

            _context.DemoFeedbacks.Add(feedback);
            await _context.SaveChangesAsync();
            return feedback;
        }

        public async Task<DemoFeedback> UpdateFeedbackAsync(int feedbackId, UpdateDemoFeedbackDto dto)
        {
            var feedback = await _context.DemoFeedbacks.FindAsync(feedbackId);
            if (feedback == null)
                throw new InvalidOperationException("Feedback bulunamadı.");

            feedback.Feedback = dto.Feedback;

            await _context.SaveChangesAsync();
            return feedback;
        }

        public async Task<bool> DeleteFeedbackAsync(int feedbackId)
        {
            var feedback = await _context.DemoFeedbacks.FindAsync(feedbackId);
            if (feedback == null)
                return false;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<DemoFeedback> GetFeedbackByIdAsync(int feedbackId)
        {
            return await _context.DemoFeedbacks
                .FirstOrDefaultAsync(df => df.Id == feedbackId);
        }

        public async Task<DemoFeedback> GetDemoFeedbackAsync(int demoId)
        {
            return await _context.DemoFeedbacks
                .FirstOrDefaultAsync(df => df.DemoId == demoId);
        }
    }
}