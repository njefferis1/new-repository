using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Additional Namespaces
using NorthwindSystem.BLL;  //controller class
using NorthwindSystem.Data; //data definition class
#endregion

namespace WebApp.SamplePages
{
    public partial class SimpleQueries : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //clear out old messagess
            MessageLabel.Text = "";
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            int productid = 0;
            //validation
            if (string.IsNullOrEmpty(SearchArg.Text.Trim()))
            {
                //  bad: message to user
                MessageLabel.Text = "Enter a product id to search.";
            }
            else if (int.TryParse(SearchArg.Text.Trim(), out productid))
            { 
                //  good: process database request
                try
                {
                    //        connect to BLL controller
                    ProductController sysmgr = new ProductController();
                    //        issue request to controller
                    Product results = sysmgr.Product_Get(int.Parse(SearchArg.Text.Trim()));
                    //        check results: single record check is == null
                    if (results == null)
                    {
                        //           none: message to user
                        MessageLabel.Text = "No data found for supplied search value";
                    }
                    else
                    {
                        //           found: display data
                        ProductID.Text = results.ProductID.ToString();
                        ProductName.Text = results.ProductName;
                    }
                    
                    
                }
                catch(Exception ex)
                {
                    //  bad: message to user
                    MessageLabel.Text = ex.Message;
                }
               
               
            }
            else
            {
                //  bad: message to user
                MessageLabel.Text = "Product ID is not a number greater than 0";
            }
            
            

        }

        protected void Clear_Click(object sender, EventArgs e)
        {
            SearchArg.Text = "";
            ProductID.Text = "";
            ProductName.Text = "";
        }
    }
}