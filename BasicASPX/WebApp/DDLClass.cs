using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp
{
    public class DDLClass
    {
        public int ValueField { get; set; }
        public string DisplayField { get; set; }
        public DDLClass()
        {
            //default
        }
        public DDLClass(int valueField, string displayField)
        {
            //greedy
            ValueField = valueField;
            DisplayField = displayField;
        }
    }
}