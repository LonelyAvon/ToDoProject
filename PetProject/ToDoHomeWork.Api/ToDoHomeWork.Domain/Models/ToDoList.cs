using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ToDoHomeWork.Domain.Models
{
    public partial class ToDoList
    {
        public const int MAX_LENGHT_DESCRIPTION = 100;

        [Required]
        [Key]
        public Guid Id { get; private set; }
        public string Description { get; private set; }

        public DateOnly? Date { get; private set; }


        public static Result<ToDoList> CreateToDo(string description, DateOnly date)
        {
            if (string.IsNullOrEmpty(description) || description.Length > MAX_LENGHT_DESCRIPTION)
            {
                return Result.Failure<ToDoList>($"{nameof(Description)} не может быть пустым");
            }
            if (date == DateOnly.MinValue)
            {
                date = DateOnly.FromDateTime(DateTime.Now);
            }
            var todolist = new ToDoList(Guid.NewGuid(), description, date);
            return Result.Success(todolist);
        }
        public static Result UpdateDescription(string description, ToDoList todo)
        {
            if (string.IsNullOrEmpty(description) || description.Length > MAX_LENGHT_DESCRIPTION)
            {
                return Result.Failure($"{nameof(Description)} не может быть пустым");
            }

            todo.Description = description;

            return Result.Success();
        }
        public static Result UpdateDate(DateOnly date, ToDoList todo)
        {
            if (date.Day == 0 || date.Month == 0 || date.Year == 0)
            {
                return Result.Failure($"{nameof(Date)} должна иметь правильный формат");
            }
            todo.Date = date;
            return Result.Success();
        }

        public ToDoList(Guid id, string description, DateOnly? date)
        {
            Id = id;
            Description = description;
            Date = date;
        }
    }
}
