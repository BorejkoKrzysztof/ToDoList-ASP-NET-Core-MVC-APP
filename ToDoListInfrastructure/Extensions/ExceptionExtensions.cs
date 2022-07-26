using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListCore.Domain_Models;
using ToDoListCore.Enums;

namespace ToDoListInfrastructure.Extensions
{
    public static class ExceptionExtensions
    {


        public static void CheckExceptions(this ToDoList toDoList)
        {
            if (toDoList is null)
            {
                throw new ArgumentNullException(nameof(toDoList), "Given ToDoList is null.");
            }

            toDoList.AccountId.CheckExceptions();
            toDoList.Title.CheckExceptions();
        }

        public static void CheckExceptions(this ToDoEntry toDoEntry)
        {
            if (toDoEntry is null)
            {
                throw new ArgumentNullException(nameof(toDoEntry), "Given ToDoEntry is null.");
            }
        }

        public static void CheckExceptions(this NotesTde note)
        {
            if (note is null)
            {
                throw new ArgumentNullException(nameof(note), "Given note is null");
            }
        }

        public static void CheckExceptions(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text), "Given string is null or empty.");
            }
        }

        public static void CheckExceptions(this Guid guid)
        {
            if (guid == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException(nameof(guid), "Given guid is empty.");
            }
        }

        public static void CheckMaxLengthExceptions(this string text, int maxCharacters)
        {
            if (text.Length > maxCharacters)
            {
                throw new ArgumentOutOfRangeException(nameof(text), "Given text is too long");
            }
        }


        public static void ValidateEdgeDateTime(this DateTime givenDateTime)
        {
            if (DateTime.Compare(givenDateTime, DateTime.MinValue) == 0 || DateTime.Compare(givenDateTime, DateTime.MaxValue) == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(givenDateTime), "Given dueDate is too early or is equal of now.");
            }
        }

        public static void ValidateProgressStatus(this ProgressStatus status)
        {
            if ((int)status < 0 || (int)status > 2)
            {
                throw new ArgumentOutOfRangeException(nameof(status), "Given progress is wrong.");
            }
        }

        public static void DateTimeValidatorLaterThanNow(this DateTime dateTime)
        {
            if (DateTime.Compare(dateTime, DateTime.Now) < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(dateTime), "Given DateTime must be later than now.");
            }
        }

        public static void IsStringRepresentationOfGuid(this string guidId)
        {
            Guid guid;
            var result = Guid.TryParse(guidId, out guid);

            if (!result)
            {
                throw new ArgumentException("Given string is not represantion of Guid value.");
            }
        }
    }
}
