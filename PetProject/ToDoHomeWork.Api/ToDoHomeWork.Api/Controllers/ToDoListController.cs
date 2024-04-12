using Microsoft.AspNetCore.Mvc;
using ToDoHomeWork.Domain.Context;
using ToDoHomeWork.Domain.Models;
using ToDoHomeWork;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace ToDoHomeWork.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ToDoListController : ControllerBase
    {
        private ToDoListDbContext _db;
        public ToDoListController(ToDoListDbContext db)
        {
            _db = db;
        }
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateToDoList([FromBody]string description, DateOnly date)
        {
            var result = ToDoList.CreateToDo(description, date);

            if (result.IsFailure)
            {
                return BadRequest(result);
            }

            ToDoList todolist = result.GetValueOrDefault();

            _db.toDoListSet.Add(todolist);
            await _db.SaveChangesAsync();
            return Ok(todolist);
        }
        [HttpGet]
        [Route("GetById/{id:guid}")]
        public async Task<IActionResult> GetByIdToDoList(Guid id)
        {
            var todolist = await _db.toDoListSet.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (todolist == null) 
            {
                return BadRequest("ВВедён неверный ID");
            }
            return Ok(todolist);

        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetByAllToDoList()
        {
            var todolist = await _db.toDoListSet.ToListAsync();
            if (todolist == null)
            {
                return BadRequest("Список дел пуст");
            }
            return Ok(todolist);
        }
        [HttpDelete]
        [Route("DeleteById/{id:guid}")]
        public async Task<IActionResult> DeleteByIdToDoList(Guid id)
        {
            var todolist = await _db.toDoListSet.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (todolist == null)
            {
                return BadRequest("ВВедён неверный ID");
            }
            _db.toDoListSet.Remove(todolist);
            await _db.SaveChangesAsync();
            return Ok("Запись удалена");
        }
        [HttpPut]
        [Route("Update{id:guid}")]
        public async Task<IActionResult> UpdateToDoList([FromBody]Guid id,string description, DateOnly date)
        {
            var todo = await _db.toDoListSet.FindAsync(id);

            if (todo==null)
            {
                return BadRequest("Запись не найдена");
            }

            var dateResult = ToDoList.UpdateDate(date, todo);
            var descriptionResult = ToDoList.UpdateDescription(description, todo);
            if (dateResult.IsFailure)
            {
                return BadRequest("Введена неверная дата");
            }
            if (descriptionResult.IsFailure)
            {
                return BadRequest("Введена неверная дата");
            }
            await _db.SaveChangesAsync();
            return Ok(todo);
        }
    }
}
