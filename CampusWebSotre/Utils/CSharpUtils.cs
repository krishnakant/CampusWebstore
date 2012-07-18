using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CampusWebStore.Utils
{
    public class CSharpUtils
    {
        public static void DumpObj(object obj)
        {
            foreach (System.ComponentModel.PropertyDescriptor descriptor in System.ComponentModel.TypeDescriptor.GetProperties(obj))
            {
                string name = descriptor.Name;
                object value = descriptor.GetValue(obj);
                System.Diagnostics.Debug.WriteLine("{0}={1}", name, value);
            }
        }
    }
}