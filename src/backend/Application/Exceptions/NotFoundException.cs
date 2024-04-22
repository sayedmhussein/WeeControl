using System;

namespace WeeControl.Core.Application.Exceptions;

public class NotFoundException(string reason) : Exception(reason);