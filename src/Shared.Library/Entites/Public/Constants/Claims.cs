using System;
using System.Collections.Generic;

namespace MySystem.SharedKernel.Entities.Public.Constants
{
    public static class Claims
    {
        public static IDictionary<ClaimType, string> Types => new Dictionary<ClaimType, string>()
        {
            { ClaimType.Session, "sss" },
            { ClaimType.Office, "ofc" }
        };

        public static IDictionary<ClaimTag, string> Tags => new Dictionary<ClaimTag, string>()
        {
            { ClaimTag.Add, "add" },
            { ClaimTag.Edit, "edit" },
            { ClaimTag.Delete, "del" },
            { ClaimTag.Read, "read" },
            { ClaimTag.Revoke, "rev" },
            { ClaimTag.Senior, "senior" },
        };

        public enum ClaimType
        {
            Session,
            Office
        }

        public enum ClaimTag
        {
            Add,
            Edit,
            Delete,
            Read,
            Revoke,
            Senior
        }
    }
}
