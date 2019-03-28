using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using NorthwindSystem.Data; //access to <T> definitions
using System.Data.Entity;   //access to Entityframework ADO.net stuff
#endregion

//this class needs to have access to ADO.Net in EntityFramework
//the Nuget package EntityFramework has alread been added to this project
//this project also needs the assembly System.Data.Entity
//this project will need using clauses that point to
//  a) System.Data.Entity namespace
//  b) your data project namespace

namespace NorthwindSystem.DAL
{
    //the class access is restrict to request from within the
    //    library by using internal
    //the class inherits (ties into EntityFramework) the class DbContext
    internal class NorthwindContext:DbContext
    {
        //setup your class default contructor to supply
        //   your connection string name to the DbContext
        //   inherited (base) class
        public NorthwindContext() : base("NWDB")
        {

        }

        //create an EntityFramework DbSet<T> for each mapped
        //  sql table
        // <T> is your class in the .Data project
        //this is a property
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Territory> Territories { get; set; }

    }
}
