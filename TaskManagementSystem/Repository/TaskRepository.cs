using Dapper;
using System.Data;
using TaskManagementSystem.Context;
using TaskManagementSystem.Contracts;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly DapperContext _context;
        public TaskRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IList<TaskEntity>> GetAllAsync()
        {
            var query = "SELECT * FROM Task";
            using (var connection = _context.CreateConnection())
            {
                var tasks = await connection.QueryAsync<TaskEntity>(query);
                return tasks.ToList();
            }
        }
        public async Task<TaskEntity> GetAsync(int id)
        {
            var query = "SELECT * FROM Task WHERE Id = @Id";
            using (var connection = _context.CreateConnection())
            {
                var task = await connection.QuerySingleOrDefaultAsync<TaskEntity>(query, new { id });
                return task;
            }
        }

        public async Task CreateAsync(TaskModel task)
        {
            var query = "INSERT INTO Task(Name, Description, Status) Values (@Name, @Description, @Status)";
            var parameters = new DynamicParameters();
            parameters.Add("Name", task.Name);
            parameters.Add("Description", task.Description);
            parameters.Add("Status", Convert.ToInt32(task.Status));
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task UpdateAsync(int id, TaskModel task)
        {
            var query = "UPDATE Task SET Name = @Name, Description = @Description, Status = @Status WHERE Id = @Id";
            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);
            parameters.Add("Name", task.Name, DbType.String);
            parameters.Add("Description", task.Description, DbType.String);
            parameters.Add("Status", Convert.ToInt32(task.Status), DbType.Int32);
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var query = "DELETE FROM Task WHERE Id = @Id";
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { id });
            }
        }
    }
}
