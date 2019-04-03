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
        #region Queries
        
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

        public List<Product> Products_GetByPartialProductName(string partialname)
        {
            using (var context = new NorthwindContext())
            {
                IEnumerable<Product> results =
                    context.Database.SqlQuery<Product>("Products_GetByPartialProductName @PartialName",
                                    new SqlParameter("PartialName", partialname));
                return results.ToList();
            }
        }

        public List<Product> Products_GetBySupplierPartialProductName(int supplierid, string partialproductname)
        {
            using (var context = new NorthwindContext())
            {
                //sometimes there may be a sql error that does not like the new SqlParameter()
                //       coded directly in the SqlQuery call
                //if this happens to you then code your parameters as shown below then
                //       use the parm1 and parm2 in the SqlQuery call instead of the new....
                //don't know why but its weird
                //var parm1 = new SqlParameter("SupplierID", supplierid);
                //var parm2 = new SqlParameter("PartialProductName", partialproductname);
                IEnumerable<Product> results =
                    context.Database.SqlQuery<Product>("Products_GetBySupplierPartialProductName @SupplierID, @PartialProductName",
                                    new SqlParameter("SupplierID", supplierid),
                                    new SqlParameter("PartialProductName", partialproductname));
                return results.ToList();
            }
        }

        public List<Product> Products_GetForSupplierCategory(int supplierid, int categoryid)
        {
            using (var context = new NorthwindContext())
            {
                IEnumerable<Product> results =
                    context.Database.SqlQuery<Product>("Products_GetForSupplierCategory @SupplierID, @CategoryID",
                                    new SqlParameter("SupplierID", supplierid),
                                    new SqlParameter("CategoryID", categoryid));
                return results.ToList();
            }
        }

        public List<Product> Products_GetByCategoryAndName(int category, string partialname)
        {
            using (var context = new NorthwindContext())
            {
                IEnumerable<Product> results =
                    context.Database.SqlQuery<Product>("Products_GetByCategoryAndName @CategoryID, @PartialName",
                                    new SqlParameter("CategoryID", category),
                                    new SqlParameter("PartialName", partialname));
                return results.ToList();
            }
        }
        #endregion

        #region Add, Update, Delete

        //the Add method will me responsible for adding an instance of product to the database (via DbSet<T>)
        //input: Instance of Product class
        //output: optional, on a identity pKey, return the new pKey value
        public int Product_Add(Product item)
        {
            //work will be done in a transaction block
            using(var context = new NorthwindContext())
            {
                //step 1: staging
                //the appropriate DbSet will be used to add the instance <T> to the database
                //in this step the data is NOT yet on the database
                //if your pkey is an identity, then the pkey value will NOT be set
                context.Products.Add(item);

                //step 2: commit add
                //this command when executed will send your record to the database for processing
                //this command when executed will cause ANY enitity validation to fire
                //if this command is not executed then your add is rolled back
                //if this command is executed but fails then your add is rolled back
                //is this command is executed and your database objects, then your add is rolled back
                //if this command is executed and no problems exist, your add will be committed.
                context.SaveChanges();

                //optionally you can return your new pkey value
                //if your data is committed then the new pkey value will be in your instance at this time
                return item.ProductID;
            }
        }
        #endregion
    }//eoc 
}// eon (end of namespace)
