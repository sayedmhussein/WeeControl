using System;

namespace WeeControl.Backend.Application.Exceptions
{
    public class DeleteFailureException : Exception
    {
        public DeleteFailureException() : base()
        {
        }

        public DeleteFailureException(string message) : base(message)
        {
        }

        [Obsolete(message: "Put the message one time without details.")]
        public DeleteFailureException(string name, object key, string message)
            : base($"Deletion of entity \"{name}\" ({key}) failed. {message}")
        {
        }
    }
}
