using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApp.SamplePages
{
    public partial class JobApplication : System.Web.UI.Page
    {
        public static List<GridViewData> gvCollection; //only because there is no database

        protected void Page_Load(object sender, EventArgs e)
        {
            Message.Text = "";
            if (!Page.IsPostBack)
            {
                gvCollection = new List<GridViewData>();
            }
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            //assume all data is valid
            //the class level list<T> will hold the collection of data for the page (we have no database)
            //the data collection will be displayed in a table like grid control: GridView

            string fullname = FullName.Text;
            string emailaddress = EmailAddress.Text;
            string phonenumber = PhoneNumber.Text;
            string fullorparttime = FullOrPartTime.SelectedValue;

            //the check box list can have several options selected
            //each option needs to be recorded
            //traverse the options of the control, record each selected option
            //CheckBoxList options are a collection of rows
            //foreacj will loop through a collection of rows

            string jobs = "";
            foreach(ListItem jobrow in Jobs.Items)
            {
                if (jobrow.Selected)
                {
                    jobs += jobrow.Text + " ";
                }
                
            }

            gvCollection.Add(new GridViewData(fullname, emailaddress, phonenumber, fullorparttime, jobs));

            //display the data collection to an appropriate control that will deisplay multiple columns
            JobApplicantList.DataSource = gvCollection;
            JobApplicantList.DataBind();
        }

        protected void Clear_Click(object sender, EventArgs e)
        {
            FullName.Text = "";
            EmailAddress.Text = "";
            PhoneNumber.Text = "";
            FullOrPartTime.SelectedIndex = -1;
            //FullOrPartTime.ClearSelection();
            //does the exact same thing
            //Jobs.SelectedIndex = -1;
            Jobs.ClearSelection();

            JobApplicantList.DataSource = null;
            JobApplicantList.DataBind();
        }
    }
}