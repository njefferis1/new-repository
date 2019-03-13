using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApp.SamplePages
{
    public partial class ContestEntry : System.Web.UI.Page
    {
        //only a list<T> because there is no database yet to store data
        //we are also not using viewstate, cookies, or session variables.
        public static List<ContestEntryData> EntryCollection;

        protected void Page_Load(object sender, EventArgs e)
        {
            Message.Text = "";
            if (!Page.IsPostBack)
            {
                EntryCollection = new List<ContestEntryData>();
            }
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            //validate the incomng data
            if (Page.IsValid)
            {
                //validate that the terms were accepted
                if (Terms.Checked)
                {
                    //  yes: create/load entry; add to storage; display entries;
                    
                }
                else
                {
                    //  no: rejection message
                    Message.Text = "You did not accept the terms of the contest. Entry Rejected.";
                }
                
            }
        }

        protected void Clear_Click(object sender, EventArgs e)
        {
            FirstName.Text = "";
            LastName.Text = "";
            StreetAddress1.Text = "";
            StreetAddress2.Text = "";
            City.Text = "";
            Province.SelectedIndex = 0;
            Terms.Checked = false;
            PostalCode.Text = "";
            EmailAddress.Text = "";
            CheckAnswer.Text = "";


            //ContestEntryList.DataSource = null;
            //ContestEntryList.DataBind();
        }
    }
}