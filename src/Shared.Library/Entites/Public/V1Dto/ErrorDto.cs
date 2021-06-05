using System;
using System.Collections.Generic;

namespace MySystem.SharedKernel.Entites.Public.V1Dto
{
    public class ErrorDto
    {
        public List<ErrorDetail> Errors { get; set; }

        public string Type { get; set; }

        public string Title { get; set; }

        public int Status { get; set; }

        public string TraceId { get; set; }
    }

    public class ErrorDetail
    {
        public string Name { get; set; }

        public List<string> Details { get; set; }
    }
}
