using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DiabetApp.Window
{
    /// <summary>
    /// Логика взаимодействия для AddProductInLine.xaml
    /// </summary>
    public partial class AddProductInLine
    {
        public AddProductInLine()
        {
            InitializeComponent();
            App.diary_View.Selected_Diary_Product_Property = new Diary_Product_Property()
            {
                Id_Diary_Line = App.diary_View.Selected_RowsOfDiary.Id,
                RowOfDiary_Diary_Line = App.diary_View.Selected_RowsOfDiary
            };
            DataContext = App.diary_View;
            gramText.DataContext = App.diary_View.Selected_Diary_Product_Property;
        }

        private void ProdBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProdBox.SelectedItem as Product != null)
            {
                Diary_Product_Property diary_Product_Property = App.diary_View.Selected_Diary_Product_Property;
                App.diary_View.Selected_Product = ProdBox.SelectedItem as Product;
                diary_Product_Property.Id_Product = App.diary_View.Selected_Product.ID;
                diary_Product_Property.Product = App.diary_View.Selected_Product;
                App.diary_View.Selected_Diary_Product_Property = diary_Product_Property;
                App.diary_View.Selected_Diary_Product_Property.ProductCalculation();
            }
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            Diary_Product_Property diary = App.diary_View.Selected_Diary_Product_Property;
            var addDiary_Produact =  App.db.Diary_Product.Add(new Diary_Product()
            {
                Product = diary.Product,
                ID_Diary_Line = App.diary_View.Selected_RowsOfDiary.Id,
                Grams = (float)Convert.ToDouble(diary.Gramm),
                He = diary.Product.Carbohydrates * (float)Convert.ToDouble(diary.Gramm) / 100 / 10
            });
            App.db.SaveChanges();
            App.diary_View.Selected_RowsOfDiary.Diary_Products.Add(addDiary_Produact);
            App.diary_View.collectionRowOfDiary.Refresh();
        }

        private static string gramtextError = string.Empty;

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //try
            //{
            //    if (Convert.ToDouble(gramText.Text) >= 0)
            //    {
            //        gramtextError = gramText.Text;
            //        Diary_Product_Property diary_Product_Property = App.diary_View.Selected_Diary_Product_Property;
            //        diary_Product_Property.Gramm =  gramtextError;
            //        App.diary_View.Selected_Diary_Product_Property = diary_Product_Property;
            //    }
            //    else
            //    {
            //        gramText.Text = gramtextError;
            //    }
            //}
            //catch
            //{
            //    gramText.Text = gramtextError;
            //}

        }

        
    }
}
