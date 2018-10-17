using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Switch.WebAPI.Models
{
    public class StatusCodes
    {
        public static string UPDATED = "Updated Successfully";
        public static string DELETED = "Deleted Successfully";
        public static string CREATED = "Created Successfully";
        public static string ACTIVE = "Active";
        public static string INACTIVE = "In-active";
        public static string ERROR_DOESNT_EXIST = "Item doesn't exist";
        public static string NAME_ALREADY_EXIST = "Name already exist";
    }
}