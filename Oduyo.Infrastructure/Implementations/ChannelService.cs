using Microsoft.EntityFrameworkCore;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class ChannelService : IChannelService
    {
        private readonly ApplicationDbContext _context;

        public ChannelService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Channel> CreateChannelAsync(CreateChannelDto dto)
        {
            var channel = new Channel
            {
                Name = dto.Name,
                IsActive = true
            };

            _context.Channels.Add(channel);
            await _context.SaveChangesAsync();
            return channel;
        }

        public async Task<Channel> UpdateChannelAsync(int channelId, UpdateChannelDto dto)
        {
            var channel = await _context.Channels.FindAsync(channelId);
            if (channel == null)
                throw new InvalidOperationException("Kanal bulunamadı.");

            channel.Name = dto.Name;
            channel.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
            return channel;
        }

        public async Task<bool> DeleteChannelAsync(int channelId)
        {
            var channel = await _context.Channels.FindAsync(channelId);
            if (channel == null)
                return false;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Channel> GetChannelByIdAsync(int channelId)
        {
            return await _context.Channels.FirstOrDefaultAsync(c => c.Id == channelId);
        }

        public async Task<List<Channel>> GetAllChannelsAsync()
        {
            return await _context.Channels.ToListAsync();
        }

        public async Task<List<Channel>> GetActiveChannelsAsync()
        {
            return await _context.Channels.Where(c => c.IsActive).ToListAsync();
        }
    }
}