using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Additional Namespaces
using NorthwindSystem.Data;                 //entitty definitions
using NorthwindSystem.Data.Views;           //view definitions
using NorthwindSystem.BLL;                  //controller classes
using System.Data.Entity.Validation;        //handle enitity validation
using System.Data.Entity.Infrastructure;    //CRUD
using System.Data.Entity.Core;              //CRUD
#endregion

namespace WebApp.NorthwindPages
{
    public partial class ProductCRUD : System.Web.UI.Page
    {
        List<string> errormsgs = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            //remove all old messages from DataList
            Message.DataSource = null;
            Message.DataBind();

            //other page initialization
            if (!Page.IsPostBack)
            {
                BindProductList();
                BindSupplierList();
                BindCategoryList();
            }
        }

        protected void BindProductList()
        {
            try
            {
                ProductController sysmgr = new ProductController();
                List<Product> datainfo = sysmgr.Product_List();
                datainfo.Sort((x, y) => x.ProductName.CompareTo(y.ProductName));
                ProductList.DataSource = datainfo;
                ProductList.DataTextField = nameof(Product.ProductName);
                ProductList.DataValueField = nameof(Product.ProductID);
                ProductList.DataBind();
                ProductList.Items.Insert(0, "select ...");
            }
            catch (Exception ex)
            {
                errormsgs.Add(GetInnerException(ex).ToString());
                LoadMessageDisplay(errormsgs, "alert alert-danger");
            }

        }
        protected void BindSupplierList()
        {
            try
            {
                SupplierController sysmgr = new SupplierController();
                List<Supplier> datainfo = sysmgr.Supplier_List();
                //datainfo.Sort((x, y) => x.SupplierName.CompareTo(y.SupplierName));
                SupplierList.DataSource = datainfo;
                //SupplierList.DataTextField = nameof(Supplier.SupplierName);
                SupplierList.DataValueField = nameof(Supplier.SupplierID);
                SupplierList.DataBind();
                SupplierList.Items.Insert(0, "select ...");
            }
            catch (Exception ex)
            {
                errormsgs.Add(GetInnerException(ex).ToString());
                LoadMessageDisplay(errormsgs, "alert alert-danger");
            }

        }
        protected void BindCategoryList()
        {
            try
            {
                CategoryController sysmgr = new CategoryController();
                List<Category> datainfo = sysmgr.Category_List();
                datainfo.Sort((x, y) => x.CategoryName.CompareTo(y.CategoryName));
                CategoryList.DataSource = datainfo;
                CategoryList.DataTextField = nameof(Category.CategoryName);
                CategoryList.DataValueField = nameof(Category.CategoryID);
                CategoryList.DataBind();
                CategoryList.Items.Insert(0, "select ...");
            }
            catch (Exception ex)
            {
                errormsgs.Add(GetInnerException(ex).ToString());
                LoadMessageDisplay(errormsgs, "alert alert-danger");
            }

        }

        //use this method to discover the inner most error message.
        //this rotuing has been created by the user
        protected Exception GetInnerException(Exception ex)
        {
            //drill down to the inner most exception
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            return ex;
        }

        //use this method to load a DataList with a variable
        //number of message lines.
        //each line is a string
        //the strings (lines) are passed to this routine in
        //   a List<string>
        //second parameter is the bootstrap cssclass
        protected void LoadMessageDisplay(List<string> errormsglist, string cssclass)
        {
            Message.CssClass = cssclass;
            Message.DataSource = errormsglist;
            Message.DataBind();
        }

        protected void Search_Click(object sender, EventArgs e)
        {

        }

        protected void Clear_Click(object sender, EventArgs e)
        {

        }

        protected void AddProduct_Click(object sender, EventArgs e)
        {
            //if (Page.IsValid)
            //{
                //any other logical validation for your data
                //in this example, I will assume that the foreign keys SupplierID and CategoryID are required
                if(SupplierList.SelectedIndex == 0)
                {
                    errormsgs.Add("Select a supplier");
                }
                if(CategoryList.SelectedIndex == 0)
                {
                    errormsgs.Add("select a category");
                }

                //check if all logical validation was successful
                if(errormsgs.Count() > 0)
                {
                    //some bad validation
                    LoadMessageDisplay(errormsgs, "alert alert-info");
                }
                else
                {
                    //assume your validation is successful and you can procede with adding the data to the database
                    //try/catch
                    try
                    {
                        //create an instance of your <T>
                        Product item = new Product();
                        //extract data from form and load your instance of <T>
                        item.ProductName = ProductName.Text.Trim();
                        item.SupplierID = int.Parse(SupplierList.SelectedValue);
                        item.CategoryID = int.Parse(CategoryList.SelectedValue);
                        item.QuantityPerUnit = string.IsNullOrEmpty(QuantityPerUnit.Text.Trim()) ? null : QuantityPerUnit.Text.Trim();
                        if (string.IsNullOrEmpty(UnitPrice.Text.Trim()))
                        {
                            item.UnitPrice = null;
                        }
                        else
                        {
                            item.UnitPrice = decimal.Parse(UnitPrice.Text.Trim());
                        }
                        if (string.IsNullOrEmpty(UnitsInStock.Text.Trim()))
                        {
                            item.UnitsInStock = null;
                        }
                        else
                        {
                            item.UnitsInStock = Int16.Parse(UnitsInStock.Text.Trim());
                        }
                        if (string.IsNullOrEmpty(UnitsOnOrder.Text.Trim()))
                        {
                            item.UnitsOnOrder = null;
                        }
                        else
                        {
                            item.UnitsOnOrder = Int16.Parse(UnitsOnOrder.Text.Trim());
                        }
                        if (string.IsNullOrEmpty(ReorderLevel.Text.Trim()))
                        {
                            item.ReorderLevel = null;
                        }
                        else
                        {
                            item.ReorderLevel = Int16.Parse(ReorderLevel.Text.Trim());
                        }
                        //logically this is a new product NOT a discontinued product
                        item.Discontinued = false;
                        //connect to the appropriate BLL Controller for <T>
                        ProductController sysmgr = new ProductController();
                        //issue the appropriate BLL controller method to process <T>
                        int newProductID = sysmgr.Product_Add(item);
                        //process any returning information from the controller method and issue a success message
                        errormsgs.Add(ProductName.Text + " has been added to the database: ID = " + ProductID.ToString());
                        LoadMessageDisplay(errormsgs, "alert alert-success");
                        ProductID.Text = newProductID.ToString();

                        //you may need to refresh various controlls on your form
                        BindProductList();
                        ProductList.SelectedValue = ProductID.Text;

                    }
                    catch (DbUpdateException ex)
                    {
                        UpdateException updateException = (UpdateException)ex.InnerException;
                        if (updateException.InnerException != null)
                        {
                            errormsgs.Add(updateException.InnerException.Message.ToString());
                        }
                        else
                        {
                            errormsgs.Add(updateException.Message);
                        }
                        LoadMessageDisplay(errormsgs, "alert alert-danger");
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (var entityValidationErrors in ex.EntityValidationErrors)
                        {
                            foreach (var validationError in entityValidationErrors.ValidationErrors)
                            {
                                errormsgs.Add(validationError.ErrorMessage);
                            }
                        }
                        LoadMessageDisplay(errormsgs, "alert alert-danger");
                    }
                    catch (Exception ex)
                    {
                        errormsgs.Add(GetInnerException(ex).ToString());
                        LoadMessageDisplay(errormsgs, "alert alert-danger");
                    }

                }
            //}

        }

        protected void UpdateProduct_Click(object sender, EventArgs e)
        {
            //if (Page.IsValid)
            //{
            //any other logical validation for your data
            //in this example, I will assume that the foreign keys SupplierID and CategoryID are required
            if (SupplierList.SelectedIndex == 0)
            {
                errormsgs.Add("Select a supplier");
            }
            if (CategoryList.SelectedIndex == 0)
            {
                errormsgs.Add("select a category");
            }

            //on the update, you must have the pkey of the record that is being processed
            if (string.IsNullOrEmpty(ProductID.Text.Trim()))
            {
                errormsgs.Add("Search for the product you wish to maintain.");
            }
            //check if all logical validation was successful
            if (errormsgs.Count() > 0)
            {
                //some bad validation
                LoadMessageDisplay(errormsgs, "alert alert-info");
            }
            else
            {
                //assume your validation is successful and you can procede with adding the data to the database
                //try/catch
                try
                {
                    //create an instance of your <T>
                    Product item = new Product();
                    //extract data from form and load your instance of <T>

                    //in addition to the non-pkey fields being accessed and loaded, the pkey value MUST also be loaded
                    item.ProductID = int.Parse(ProductID.Text.Trim());

                    item.ProductName = ProductName.Text.Trim();
                    item.SupplierID = int.Parse(SupplierList.SelectedValue);
                    item.CategoryID = int.Parse(CategoryList.SelectedValue);
                    item.QuantityPerUnit = string.IsNullOrEmpty(QuantityPerUnit.Text.Trim()) ? null : QuantityPerUnit.Text.Trim();
                    if (string.IsNullOrEmpty(UnitPrice.Text.Trim()))
                    {
                        item.UnitPrice = null;
                    }
                    else
                    {
                        item.UnitPrice = decimal.Parse(UnitPrice.Text.Trim());
                    }
                    if (string.IsNullOrEmpty(UnitsInStock.Text.Trim()))
                    {
                        item.UnitsInStock = null;
                    }
                    else
                    {
                        item.UnitsInStock = Int16.Parse(UnitsInStock.Text.Trim());
                    }
                    if (string.IsNullOrEmpty(UnitsOnOrder.Text.Trim()))
                    {
                        item.UnitsOnOrder = null;
                    }
                    else
                    {
                        item.UnitsOnOrder = Int16.Parse(UnitsOnOrder.Text.Trim());
                    }
                    if (string.IsNullOrEmpty(ReorderLevel.Text.Trim()))
                    {
                        item.ReorderLevel = null;
                    }
                    else
                    {
                        item.ReorderLevel = Int16.Parse(ReorderLevel.Text.Trim());
                    }
                    //during an update, you need to take the actual value that is in the field
                    item.Discontinued = Discontinued.Checked; ;
                    //connect to the appropriate BLL Controller for <T>
                    ProductController sysmgr = new ProductController();
                    //issue the appropriate BLL controller method to process <T>
                    int rowsaffected = sysmgr.Product_Update(item);
                    //process any returning information from the controller method and issue a success message
                    //did the database REALLY get updated
                    if(rowsaffected == 0)
                    {
                        errormsgs.Add(ProductName.Text + " has not been updated. Search for the product again.");
                        LoadMessageDisplay(errormsgs, "alert alert-warning");
                        BindProductList();
                    }
                    else
                    {
                        errormsgs.Add(ProductName.Text + " has been updated");
                        LoadMessageDisplay(errormsgs, "alert alert-success");
                        BindProductList();
                        ProductList.SelectedValue = ProductID.Text;
                    }
                    
                    

                    //you may need to refresh various controlls on your form
                    

                }
                catch (DbUpdateException ex)
                {
                    UpdateException updateException = (UpdateException)ex.InnerException;
                    if (updateException.InnerException != null)
                    {
                        errormsgs.Add(updateException.InnerException.Message.ToString());
                    }
                    else
                    {
                        errormsgs.Add(updateException.Message);
                    }
                    LoadMessageDisplay(errormsgs, "alert alert-danger");
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            errormsgs.Add(validationError.ErrorMessage);
                        }
                    }
                    LoadMessageDisplay(errormsgs, "alert alert-danger");
                }
                catch (Exception ex)
                {
                    errormsgs.Add(GetInnerException(ex).ToString());
                    LoadMessageDisplay(errormsgs, "alert alert-danger");
                }

            }
            //}
        }

        protected void RemoveProduct_Click(object sender, EventArgs e)
        {
            //on the delete, you must have the pkey of the record that is being processed
            if (string.IsNullOrEmpty(ProductID.Text.Trim()))
            {
                errormsgs.Add("Search for the product you wish to maintain.");
            }
            //check if all logical validation was successful
            if (errormsgs.Count() > 0)
            {
                //some bad validation
                LoadMessageDisplay(errormsgs, "alert alert-info");
            }
            else
            {
                //assume your validation is successful and you can procede with deleting the data to the database
                //try/catch
                try
                {
                    //connect to the appropriate BLL Controller for <T>
                    ProductController sysmgr = new ProductController();
                    //issue the appropriate BLL controller method to process <T>
                    int rowsaffected = sysmgr.Product_Delete(int.Parse(ProductID.Text.Trim()));
                    //process any returning information from the controller method and issue a success message
                    //did the database REALLY get updated
                    if (rowsaffected == 0)
                    {
                        errormsgs.Add(ProductName.Text + " has not been deleted. Search for the product again.");
                        LoadMessageDisplay(errormsgs, "alert alert-warning");
                        BindProductList();
                    }
                    else
                    {
                        errormsgs.Add(ProductName.Text + " has been discontinued");
                        LoadMessageDisplay(errormsgs, "alert alert-success");
                        BindProductList();
                        //dependant on whether the record is a physical or logical delete

                        //kept for logical
                        Discontinued.Checked = true;
                        ProductList.SelectedValue = ProductID.Text;

                        //on a physical delete, optionally clear the fields
                        //Clear_Click(sender, new EventArgs());
                    }
                }
                catch (DbUpdateException ex)
                {
                    UpdateException updateException = (UpdateException)ex.InnerException;
                    if (updateException.InnerException != null)
                    {
                        errormsgs.Add(updateException.InnerException.Message.ToString());
                    }
                    else
                    {
                        errormsgs.Add(updateException.Message);
                    }
                    LoadMessageDisplay(errormsgs, "alert alert-danger");
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            errormsgs.Add(validationError.ErrorMessage);
                        }
                    }
                    LoadMessageDisplay(errormsgs, "alert alert-danger");
                }
                catch (Exception ex)
                {
                    errormsgs.Add(GetInnerException(ex).ToString());
                    LoadMessageDisplay(errormsgs, "alert alert-danger");
                }

            }
            //}
        }
    }
}