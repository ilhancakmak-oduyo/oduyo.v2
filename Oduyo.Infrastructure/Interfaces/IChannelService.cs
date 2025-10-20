using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;

namespace Oduyo.Infrastructure.Interfaces
{
    public interface IChannelService
    {
        Task<Channel> CreateChannelAsync(CreateChannelDto dto);
        Task<Channel> UpdateChannelAsync(int channelId, UpdateChannelDto dto);
        Task<bool> DeleteChannelAsync(int channelId);
        Task<Channel> GetChannelByIdAsync(int channelId);
        Task<List<Channel>> GetAllChannelsAsync();
        Task<List<Channel>> GetActiveChannelsAsync();
    }
}