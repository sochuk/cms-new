using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;

namespace CMS.Management.Model
{
    public class M_Gender
    {
        public enum Gender
        {
            [Description("Laki-Laki")]
            Male,
            [Description("Perempuan")]
            Female
        }
    }
}