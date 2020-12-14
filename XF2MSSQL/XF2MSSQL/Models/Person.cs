using System;
using System.Collections.Generic;
using System.Text;

namespace XF2MSSQL.Models
{
    public class Person
    {
        public string Name { get; private set; }

        public int BusinessEntityID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Person(int businessEntityID, string firstName, string lastName)
        {
            BusinessEntityID = businessEntityID;
            FirstName = firstName;
            LastName = lastName;
            Name = string.Concat(businessEntityID.ToString(), " - ", lastName, ", ", firstName);
        }

    }
}
