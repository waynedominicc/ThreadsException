using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ThreadsException
{
    public partial class Form1 : Form
    {
        private BindingSource showProductList;
        public Form1()
        {
            InitializeComponent();
            showProductList = new BindingSource();
        }

        private string _ProductName;
        private string _Description;
        private string _Category;
        private string _MfgDate;
        private string _ExpDate;
        private int _Quantity;
        private double _SellPrice;

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] ListProductCategory = new string[] { "Beverages","Bread/Bakery","Canned/Jarred Goods"
            , "Dairy", "Frozen Goods", "Meat", "Personal Care", "Other"};

            foreach (string category in ListProductCategory)
            {
                cbCategory.Items.Add(category);
            }

        }
        class NumberFormatException : Exception
        {
            public NumberFormatException(string Product_Name) : base(Product_Name) { }
        }
        class StringFormatException : Exception
        {
            public StringFormatException(string Quantity) : base(Quantity) { }
        }
        class CurrencyFormatException : Exception
        {
            public CurrencyFormatException(string SellingPrice): base(SellingPrice) { }
        }

        public string Product_Name (string name)
        {
            if (!Regex.IsMatch(name, @"^[a-zA-Z]+$"))
            {
                throw new StringFormatException("Invalid product name format!");
            }
            return name;
        }
        public int Quantity (string qty)
        {
            if (!Regex.IsMatch(qty, @"^[0-9]"))
            {
                throw new NumberFormatException("Invalid product quantity format!");
            }
            return Convert.ToInt32(qty);
        }
        public double SellingPrice (string price)
        {
            if (!Regex.IsMatch(price, @"^(\d*\.)?\d+$"))
            {
                throw new StringFormatException("Invalid product price format!");
            }
            return Convert.ToDouble(price);
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            try
            {
                _ProductName = Product_Name(txtProductName.Text);
                _Category = cbCategory.Text;
                _MfgDate = dtPickerMfgDate.Value.ToString("yyyy-MM-dd");
                _ExpDate = dtPickerExpDate.Value.ToString("yyyy-MM-dd");
                _Description = richTxtDescription.Text;
                _Quantity = Quantity(txtQuantity.Text);
                _SellPrice = SellingPrice(txtSellPrice.Text);
                showProductList.Add(new ProductClass(_ProductName, _Category, _MfgDate,
                _ExpDate, _SellPrice, _Quantity, _Description));
                gridViewProductList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                gridViewProductList.DataSource = showProductList;
            }

            catch (StringFormatException ex)
            {
                MessageBox.Show("String Format Exception: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (NumberFormatException ex)
            {
                MessageBox.Show("Number Format Exception: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (CurrencyFormatException ex)
            {
                MessageBox.Show("Currency Format Exception: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Console.WriteLine("The adding product operation has completed");
            }
        }
    }
}
