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

        protected void CategoryProductList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //the grid view uses the page index to calculate which rows out of your dataset to display; all other rows are ignored
            //when switching pages, you MUST set the page index property
            //data for the new page index will come from the "e" parameter of this method
            CategoryProductList.PageIndex = e.NewPageIndex;

            //the second step i nthis method is to refresh the dataset of teh control
            //this can be done by reassigning the dataset to the control
            //since our data collection is coming from the data base depending on the selected category; 
            //we need to issue another call to the database;
            //then bind that data to the control
            Submit_Click(sender, new EventArgs());
        }

        protected void CategoryProductList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //accessing data on a gridview cell is dependant on the web control datatype
            //syntax: 
            //  (gvcontrolpointer.FindControl("cellcontrolid") as cellcontroltype).control accesstype
            // gvcontrolpointer: reference to gridview row
            // cellcontrolid: ID of the control in the cell
            // cell controltype: type of web control in the cell
            // controlaccesstype: how is the web control accessed

            //personal style
            GridViewRow agvrow = (CategoryProductList.Rows[CategoryProductList.SelectedIndex]);
            string productid = (agvrow.FindControl("ProductID") as Label).Text;
            string productname = (agvrow.FindControl("ProductName") as Label).Text;
            string discontinued = "";
            if ((agvrow.FindControl("Discontinued") as CheckBox).Checked)
            {
                discontinued = "discontinued";
            }
            else
            {
                discontinued = "available";
            }
            MessageLabel.Text = productname + " (" + productid + ") is " + discontinued;
        }
    }
}