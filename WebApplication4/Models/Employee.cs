using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    public class Employee
    {
        public int ID { get; set; }
        
        //[StringLength(15)]
        public string FullName { get; set; }

        //[Range(18,100)]
        public int Age { get; set; }

        //[StringLength(25)]
        public string Email{ get; set; }
    }
}