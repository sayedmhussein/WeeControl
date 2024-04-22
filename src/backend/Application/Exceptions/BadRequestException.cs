using System;

namespace WeeControl.Core.Application.Exceptions;

public class BadRequestException(string message) : Exception(message);