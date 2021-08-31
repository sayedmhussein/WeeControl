using System;
namespace WeeControl.Backend.Application.Common.Exceptions
{
    public class DeleteFailureException : Exception
    {
        public DeleteFailureException() : base()
        {
        }

        public DeleteFailureException(string name, object key, string message)
            : base($"Deletion of entity \"{name}\" ({key}) failed. {message}")
        {
        }
    }
}
