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

       
    }
}