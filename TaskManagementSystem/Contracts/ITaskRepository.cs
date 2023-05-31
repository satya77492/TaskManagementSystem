using TaskManagementSystem.Models;

namespace TaskManagementSystem.Contracts
{
    public interface ITaskRepository
    {
        public Task<IList<TaskEntity>> GetAllAsync();
        public Task<TaskEntity> GetAsync(int id);
        public Task CreateAsync(TaskModel task);
        public Task UpdateAsync(int id, TaskModel task);
        public Task DeleteAsync(int id);
    }
}
