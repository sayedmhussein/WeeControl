using System;

namespace WeeControl.Core.Application.Exceptions;

public class NotAllowedException(string msg) : Exception(msg);