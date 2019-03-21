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
    public partial class SqlProcQueries : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //clear out old messages
            MessageLabel.Text = "";

            //load the dropdownlist (ddl) with a sorted list of categories
            //this load will be done once when the page first is processed
            if (!Page.IsPostBack)
            {
                //use user friendly error handling
                try
                {
                    //the data ollection will come from the database
                    //create and connect to the appropriate BLL class
                    CategoryController sysmgr = new CategoryController();
                    //issue a request for data via the appropriate BLL class method
                    List<Category> datainfo = sysmgr.Category_List();
                    //optionally: Sort the collection
                    datainfo.Sort((x,y) => x.CategoryName.CompareTo(y.CategoryName));
                    //attach the data to the ddl control
                    CategoryList.DataSource = datainfo;
                    //indicate the data properties for DataTextField and DataValueField
                    CategoryList.DataTextField = nameof(Category.CategoryName);
                    CategoryList.DataValueField = nameof(Category.CategoryID);
                    //physically bind the data to the ddl
                    CategoryList.DataBind();
                    //optionally: place a prompt on the ddl
                    CategoryList.Items.Insert(0, "select ...");
                }
                catch(Exception ex)
                {
                    MessageLabel.Text = ex.Message;
                }
            }
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            //ensure a selection was made
            if(CategoryList.SelectedIndex == 0)
            {
                //  no selection: Message to user
                MessageLabel.Text = "Please make a selection to view category products";
            }
            else
            {
                //  yes selection:process request for lookup
                //      user friendly error handling
                try
                {
                    //      create and connect to the appropriate BLL class
                    ProductController sysmgr = new ProductController();
                    //      issue the lookup request using the appropriate BLL class method and capture results
                    List<Product> results = sysmgr.Product_GetByCategories(int.Parse(CategoryList.SelectedValue));
                    //      test the results ( .Count() == 0)
                    if(results.Count() == 0)
                    {
                        //      no results: bad, not found message
                        MessageLabel.Text = "No products found for requested category";
                        //optionally: clear the previous successful data display
                        CategoryProductList.DataSource = null;
                        CategoryProductList.DataBind();
                    }
                    else
                    {
                        //      yes results: display returned data
                        CategoryProductList.DataSource = results;
                        CategoryProductList.DataBind();
                    }


                }
                catch (Exception ex)
                {
                    MessageLabel.Text = ex.Message;
                }
            }


        }

        protected void Clear_Click(object sender, EventArgs e)
        {
            CategoryList.ClearSelection();
            CategoryProductList.DataSource = null;
            CategoryProductList.DataBind();
        }
    }
}