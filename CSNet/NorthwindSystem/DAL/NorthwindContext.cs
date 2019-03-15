using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using NorthwindSystem.Data; //access to <T> definistions
using System.Data.Entity;   //access to Entityframework ADO.net stuff
#endregion

//this class needs to have access to ADO.net in Entityframework
//the Nuget package EntityFramework has already been added to this project
//this project also needs the assembly System.Data.Entity
//this project will need using clauses that point to
//  a) System.Data.Entity namespace
//  b) your data project namespace

namespace NorthwindSystem.DAL
{
    //the class access is restricted to request from within the library by using internal
    //the class inherits (ties into EntityFramework) the class DbContext
    internal class NorthwindContext:DbContext
    {
        //setup your class default constructor to supply your connection string name to the DbContext inherited (base) class
        public NorthwindContext() : base("NWDB")
        {

        }

        //create an EntityFramework DbSet<T> for each mapped sql table
        //<T> is yout class in the .Data project
        //this is a property
        public DbSet<Product> Products { get; set; }

    }
}
