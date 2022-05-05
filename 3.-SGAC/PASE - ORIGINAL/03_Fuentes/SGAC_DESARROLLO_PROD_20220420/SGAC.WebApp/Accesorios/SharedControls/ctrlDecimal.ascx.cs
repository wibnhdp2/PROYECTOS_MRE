using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGAC.WebApp.Accesorios.SharedControls
{
    public partial class ctrlDecimal : System.Web.UI.UserControl
    {
        public string Text
        {
            set { txtDecimal.Text = value; }
            get { return txtDecimal.Text; }
        }

        public Unit Width
        {
            set { txtDecimal.Width = value; }
            get { return txtDecimal.Width; }
        }

        public bool Enabled
        {
            set { txtDecimal.Enabled = value; }
            get { return txtDecimal.Enabled; }
        }

        public int MaxLength
        {
            set { txtDecimal.MaxLength = value; }
            get { return txtDecimal.MaxLength; }
        }

        private char _tipo = 'D';
        public char Tipo
        {
            set { _tipo = value; }
            get { return _tipo; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (_tipo == 'D')
                {
                    txtDecimal_MaskedEditExtender.Mask = "999999999.99";
                    txtDecimal.Text = "_________.__";
                }
                else if (_tipo == 'N')
                {
                    txtDecimal_MaskedEditExtender.Mask = "999999999";
                    txtDecimal.Text = "_________";
                }
            }
        }

        protected void txtDecimal_TextChanged(object sender, EventArgs e)
        {
            
        }

    }
}