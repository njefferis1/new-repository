using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using NorthwindSystem.Data; //obtains the <T> devinitions
using NorthwindSystem.DAL;  //obtains the context class
using System.ComponentModel;//exposes classes and methods for use by the ODS IDE delevoper
#endregion

namespace NorthwindSystem.BLL
{
    //expose the class for use by the ODS IDE developer
    [DataObject]
    public class CategoryController
    {
        public Category Category_Get(int categoryid)
        {
            using (var context = new NorthwindContext())
            {
                return context.Categories.Find(categoryid);
            }
        }

        //expose any specific method you wish to the ODS IDE developer
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Category> Category_List()
        {
            using (var context = new NorthwindContext())
            {
                return context.Categories.ToList();
            }
        }
    }
}
