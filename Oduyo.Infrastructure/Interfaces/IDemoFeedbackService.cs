using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;

namespace Oduyo.Infrastructure.Interfaces
{
    public interface IDemoFeedbackService
    {
        Task<DemoFeedback> CreateFeedbackAsync(CreateDemoFeedbackDto dto);
        Task<DemoFeedback> UpdateFeedbackAsync(int feedbackId, UpdateDemoFeedbackDto dto);
        Task<bool> DeleteFeedbackAsync(int feedbackId);
        Task<DemoFeedback> GetFeedbackByIdAsync(int feedbackId);
        Task<DemoFeedback> GetDemoFeedbackAsync(int demoId);
    }
}