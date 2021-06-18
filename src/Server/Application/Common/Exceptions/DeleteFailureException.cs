using System;
namespace WeeControl.Server.Application.Common.Exceptions
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
