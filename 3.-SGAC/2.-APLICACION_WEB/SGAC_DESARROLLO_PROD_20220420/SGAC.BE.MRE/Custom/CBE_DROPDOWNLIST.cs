using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE.Custom
{
    public class CBE_DROPDOWNLIST
    {
        public CBE_DROPDOWNLIST() { 
        }

        public CBE_DROPDOWNLIST(string text, string value) {
            this.TextField = text;
            this.ValueField = value;
        }

        public string TextField { get; set; }
        public string ValueField { get; set; }
    }
}
