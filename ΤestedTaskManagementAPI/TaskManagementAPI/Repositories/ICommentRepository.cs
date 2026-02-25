using TaskManagementAPI.DTOs;

namespace TaskManagementAPI.Repositories;

public interface ICommentRepository
{
    Task<IEnumerable<CommentDto>> GetByTaskIdAsync(int taskId);
    Task<int> CreateAsync(int taskId, CreateCommentDto dto);
    Task<bool> DeleteAsync(int id);
}
