using System;

namespace WeeControl.Core.Application.Exceptions;

public class DeleteFailureException(string message) : Exception(message);