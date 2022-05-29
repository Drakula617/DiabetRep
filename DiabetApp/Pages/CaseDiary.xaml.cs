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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DiabetApp.Classes;

namespace DiabetApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для CaseDiary.xaml
    /// </summary>
    public partial class CaseDiary : Page
    {
        private static CheckInput checkInput = new CheckInput();
        public CaseDiary()
        {
            InitializeComponent();
            //App.diary_View = new Diary_View();
            DataContext = App.diary_View;
            LineLIst.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Time", System.ComponentModel.ListSortDirection.Ascending));
        }
        private static bool openwin = false;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (openwin == false)
            {
                App.diary_View.ProductsAdd();
                App.diary_View.Selected_RowsOfDiary = LineLIst.SelectedItem as RowOfDiary;
                App.addProductInLine = new Window.AddProductInLine();
                App.addProductInLine.Show();
                openwin = true;
                App.addProductInLine.Closed += AddProductInLine_Closed;
            }
            else
            { 
                
            }
            
        }

        private void AddProductInLine_Closed(object sender, EventArgs e)
        {
            App.addProductInLine = null;
            App.diary_View.Selected_Diary_lines = null;
            openwin = false;
        }

       

        private void removeprod_Click(object sender, RoutedEventArgs e)
        {
            Diary_Product diary_Product = (sender as Button).DataContext as Diary_Product;
            App.db.Diary_Product.Remove(diary_Product);
            App.db.SaveChanges();
            //App.diary_View.Diary_LinesAdd();
            App.diary_View.RowOfDiaryAdd();
            
        }


        private void glucoseText_LostFocus(object sender, RoutedEventArgs e)
        {

            TextBox textBlock = sender as TextBox;
            
            App.diary_View.Selected_RowsOfDiary = (sender as TextBox).DataContext as RowOfDiary;

            if(App.diary_View.Selected_RowsOfDiary != null)
            {

                foreach (var item in App.db.Diary_Line.ToList().Where(c=> c.ID == App.diary_View.Selected_RowsOfDiary.Id))
                {
                   item.Glucose = checkInput.CheckNumber(textBlock.Text);
                }
                App.db.SaveChanges();
                foreach (var item in App.diary_View.RowsOfDiary.ToList().Where(c=> c == App.diary_View.Selected_RowsOfDiary))
                {
                   item.Glucose = checkInput.CheckNumber(textBlock.Text);
                }
                App.diary_View.collectionRowOfDiary.Refresh();
            }
            else
            {
                
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.GraphLine());
        }

        private void glucoseText_GotFocus(object sender, RoutedEventArgs e)
        {
            App.diary_View.Selected_RowsOfDiary = (sender as TextBox).DataContext as RowOfDiary;
            if(App.diary_View.Selected_RowsOfDiary != null)
            checkInput.Previous_number = (float)((sender as TextBox).DataContext as RowOfDiary).Glucose;
        }

        private void addRow_Click(object sender, RoutedEventArgs e)
        {
            Window.AddRowWin addRowWin = new Window.AddRowWin();
            addRowWin.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var delete = MessageBox.Show("Вы действительно хотите удалить строку?", "Предупреждение", MessageBoxButton.YesNo);
            if (delete == MessageBoxResult.Yes)
            {
                App.diary_View.Selected_RowsOfDiary = (sender as Button).DataContext as RowOfDiary;
                //App.diary_View.RowsOfDiary.Remove(diary);
                //App.diary_View.collectionRowOfDiary.Refresh();
                App.db.Diary_Product.RemoveRange(App.db.Diary_Product.ToList().Where(c => c.ID == App.diary_View.Selected_RowsOfDiary.Id));
                App.db.SaveChanges();
                App.db.Diary_Line.Remove(App.db.Diary_Line.ToList().Find(c => c.ID == App.diary_View.Selected_RowsOfDiary.Id));
                App.db.SaveChanges();
                App.diary_View.RowsOfDiary.Remove(App.diary_View.Selected_RowsOfDiary);
                App.diary_View.collectionRowOfDiary.Refresh();
            }
        }

        private void calculation_Click_1(object sender, RoutedEventArgs e)
        {
            Windows.CalculationDoseWin doseWin = new Windows.CalculationDoseWin((sender as Button).DataContext as RowOfDiary);
            doseWin.Show();
        }

        private void anketebut_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CaseProfilePage());
        }

        private void checkDose_Click(object sender, RoutedEventArgs e)
        {

            App.diary_View.Selected_RowsOfDiary = (sender as CheckBox).DataContext as RowOfDiary;
            if (App.diary_View.Selected_RowsOfDiary.IsDoseLower == true)
            {
                foreach (var item in App.db.Diary_Line.ToList().Where(c=> c.ID == App.diary_View.Selected_RowsOfDiary.Id).ToList())
                {
                    item.IsDoseLower = false; 
                    App.db.SaveChanges();
                }
               
                foreach (var item in App.diary_View.RowsOfDiary.ToList().Where(c=> c == App.diary_View.Selected_RowsOfDiary).ToList())
                {
                    item.IsDoseLower = false;
                }
            }
            else
            {
                foreach (var item in App.db.Diary_Line.ToList().Where(c => c.ID == App.diary_View.Selected_RowsOfDiary.Id).ToList())
                {
                    item.IsDoseLower = true;
                    App.db.SaveChanges();
                }
                
                foreach (var item in App.diary_View.RowsOfDiary.ToList().Where(c => c == App.diary_View.Selected_RowsOfDiary).ToList())
                {
                    item.IsDoseLower = true;
                }
            }
            App.diary_View.collectionRowOfDiary.Refresh();
        }
    }
}
