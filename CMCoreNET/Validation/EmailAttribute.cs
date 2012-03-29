using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CMCoreNET.Validation
{
    public class EmailAttribute : RegularExpressionAttribute
    {
        public EmailAttribute()
            : base(string.Format("^{0}@{1}$",
                @"[\w\d!#&%&'*+-/+?^`{|}~]+(\.[\w\d!#&%&'*+-/+?^`{|}~])*", 
                @"([a-z\d][-a-z\d]*[a-z\d]\.)*[a-z][-a-z\d]*[a-z]"))
        { }
    }
}
