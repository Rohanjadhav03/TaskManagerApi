using AutoMapper;
using TaskManagerApi.DTOs;
using TaskManagerApi.Models;
using TaskManagerApi.Repositories;

namespace TaskManagerApi.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repo;
        private readonly IMapper _mapper;

        public TaskService(ITaskRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskReadDto>> GetAllAsync()
        {
            var tasks = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<TaskReadDto>>(tasks);
        }

        public async Task<TaskReadDto?> GetByIdAsync(int id)
        {
            var task = await _repo.GetByIdAsync(id);
            return task == null ? null : _mapper.Map<TaskReadDto>(task);
        }

        public async Task<TaskReadDto> CreateAsync(TaskCreateDto dto)
        {
            var task = _mapper.Map<TaskItem>(dto);
            await _repo.AddAsync(task);
            return _mapper.Map<TaskReadDto>(task);
        }

        public async Task<bool> UpdateAsync(int id, TaskUpdateDto dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;

            // map fields
            existing.Title = dto.Title;
            existing.Description = dto.Description;
            existing.IsCompleted = dto.IsCompleted;
            existing.DueDate = dto.DueDate;
            existing.Priority = dto.Priority;

            await _repo.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;

            await _repo.DeleteAsync(existing);
            return true;
        }
    }
}