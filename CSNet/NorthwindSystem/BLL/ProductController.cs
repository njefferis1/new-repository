using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


#region Additional Namespaces
using NorthwindSystem.Data; //the .Data class
using NorthwindSystem.DAL;  //the DAL context class
using System.Data.SqlClient;//required for SqlParamter()
#endregion

namespace NorthwindSystem.BLL
{
    //the classes within the BLL are referred to as your interface
    //these classes will be called by your webapp
    //for ease of maintenance, each class will represent a specific
    //     data class ie. Product
    public class ProductController
    {
        //create a method to find a specific record on the sql table
        //this will be done by the primary key
        //input: search argument value
        //output: results of the search -> a single Product record
        //this method is public
        public Product Product_Get(int productid)
        {
            //connect to the appropriate context class to access the database
            //create an instance of the appropriate context class
            //accessing the appropriate DbSet<T>, issue a request for execution
            //     against the sql table
            //the appropriate request return data will be received
            //return these results
            //the request will be done in a transaction
            //this lookup is by PRIMARY KEY
            using (var context = new NorthwindContext())
            {
                return context.Products.Find(productid);
            }
        }

        //Obtain a list of all table records
        //input: none
        //output: List<T> where <T> is the appropriate data class (Product)
        public List<Product> Product_List()
        {
            using (var context = new NorthwindContext())
            {
                return context.Products.ToList();
            }
        }

        //this database query is NOT based on the primary key
        //.Find() is based on the primary key, therfore it cannot be used
        //instead, we will use a Database.SqlQuery<T>() method
        //this method will have one or more arguments
        // a) the sql execution request for a procedure
        // b) optional; any argument(s) needed by (a)
        //argumants are specified using new SqlParameter(parametername, value)
        //each required argument needs a SqlParameter()
        //SqlParameter() needs a using clause System.Data.SqlClient
        public List<Product> Product_GetByCategories(int categoryid)
        {
            using(var context = new NorthwindContext())
            {
                //most data sets are returned from the DbSet as an IEnumerable<T> datatype
                //we canvert this datatype to a List<T> using .ToList();
                IEnumerable<Product> results = 
                    context.Database.SqlQuery<Product>("Products_GetByCategories @CategoryID", 
                                                        new SqlParameter("CategoryID", categoryid));
                return results.ToList();
            }
        }
    }
}
