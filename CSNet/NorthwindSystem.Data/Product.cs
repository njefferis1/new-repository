﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// the annotations used within the .Data project will require
//       the System.ComponentModel.DataAnnotation assembly
// this assembly is added via your References

#region Additional Namespaces
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
#endregion

namespace NorthwindSystem.Data
{
    //use an annotation to link this class to the appropriate
    //   sql table
    [Table("Products")]
    public class Product
    {
        //mapping of the sql table attributes will be to
        //   class properties

        private string _QuantityPerUnit;

        // use an annotation to identify the primary key
        // 1) identity pkey on your sql table (default)
        //    [Key] assume identity pkey ending in ID or Id
        // 2) a compound pkey on your sql table
        //    [Key,Column(Order=n)] where n is the numeric value
        //       of the physical order of the attribute in the key
        // 3) a user supplied pkey on your sql table
        //    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]

        [Key]
        public int ProductID { get; set; }
        [Required(ErrorMessage ="Product Name is required")] //dont use [required] on identities
        [StringLength(40, ErrorMessage ="Product Name is limited to 40 characters")]
        public string ProductName { get; set; }
        public int? SupplierID { get; set; }
        public int? CategoryID { get; set; }
        [StringLength(20, ErrorMessage = "Quantity per unit is limited to 20 characters")]
        public string QuantityPerUnit
        {
            get
            {
                return _QuantityPerUnit;
            }
            set
            {
                _QuantityPerUnit = string.IsNullOrEmpty(value) ? null : value; //remove .trim() - causes crash
            }
        }
        [Range(0.00, double.MaxValue, ErrorMessage = "Unit Price must be 0.00 or greater")]
        public decimal? UnitPrice { get; set; }
        [Range(0, Int16.MaxValue, ErrorMessage = "QoH must b 0 or greater")]
        public Int16? UnitsInStock { get; set; }
        [Range(0, Int16.MaxValue, ErrorMessage = "QoO must b 0 or greater")]
        public Int16? UnitsOnOrder { get; set; }
        [Range(0, Int16.MaxValue, ErrorMessage = "ROL must b 0 or greater")]
        public Int16? ReorderLevel { get; set; }
        public bool Discontinued { get; set; }

        //sample of a computed field on your sql
        //to annotate this property to be taken as a
        //    sql computed field use
        // [DatabaseGenerated(DatabaseGeneratedOPtion.Computed)]
        // public decimal somecomputedsqlfield {get;set;}

        //creating a read only property that is NOT
        //   an actual field on your sql table
        //   means that no data is actually transfered
        //example FirstName and LastName attributes are oftern
        //        combined into a single field for display
        //        assume: FullName
        // use the NotMapped annotation to handle this

        //[NotMapped]
        //public string FullName
        //{
        //    get FirstName + " " + LastName;
        //}
    }
}
