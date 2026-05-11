using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//-------------------------------------------------------------------------------------------------------------------------------------------
//created by chang on 20110422
//============================
//use to edit the further description
//-------------------------------------------------------------------------------------------------------------------------------------------

namespace VTACPluginBase.CommonForms
{
    public partial class EditFurtherDescription_Form : Form
    {
        public EditFurtherDescription_Form()
        {
            InitializeComponent();
        }
        
        public string Gstr_FurtherDescription = "";

        #region " Form Events "
        private void EditFurtherDescription_Form_Load(object sender, System.EventArgs e)
        {
            this.MemoEdit1.Text = Gstr_FurtherDescription;

            this.MemoEdit1.Focus();
        }
        #endregion //" Form Events "

        #region " Simple Buttons Events "
        private void Cancel_SBtn_Click(System.Object sender, System.EventArgs e)
        {
            // set dialog result to CANCEL
            this.DialogResult = DialogResult.Cancel;

            this.Close(); // close form
        }

        private void OK_SBtn_Click(System.Object sender, System.EventArgs e)
        {
            // set dialog result to OK
            this.DialogResult = DialogResult.OK;

            // set back the new value of further description
            Gstr_FurtherDescription = this.MemoEdit1.Text;

            // close form
            this.Close();
        }
        #endregion //" Simple Buttons Events "
    }
}
