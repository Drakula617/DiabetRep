﻿using System;
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
using DiabetApp.Classes;
using System.Windows.Shapes;

namespace DiabetApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для UpdateProfile.xaml
    /// </summary>
    public partial class UpdateProfile
    {
        private static CheckInput check = new CheckInput();
        public UpdateProfile()
        {
            InitializeComponent();
            DataContext = App.diary_View;
            carbList.DataContext = App.db.Dose_Profile.ToList().Where(c=> c.ID_Type_Coefficient ==2 && c.Profile == App.diary_View.Selected_Profile);
            basalList.DataContext = App.db.Dose_Profile.ToList().Where(c => c.ID_Type_Coefficient == 1 && c.Profile == App.diary_View.Selected_Profile);
            //CoefStack.DataContext = App.db.Profile.ToList().Where(x => x == App.diary_View.Selected_Profile);
        }


        private void coefText_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBlock = sender as TextBox;
            Dose_Profile carb_coef = (sender as TextBox).DataContext as Dose_Profile;

            if (carb_coef != null)
            {
                foreach (var item in App.db.Dose_Profile.ToList().Where(c => c == carb_coef))
                {
                    item.Coefficient = check.CheckNumber(textBlock.Text);
                    App.db.SaveChanges();
                }
                App.diary_View.Carb_CoefLoad();
                App.diary_View.GraphCoef();
                App.diary_View.collectionRowOfDiary.Refresh();
                carbList.DataContext = App.db.Dose_Profile.ToList().Where(c => c.ID_Type_Coefficient == 2 && c.Profile == App.diary_View.Selected_Profile);

            }
            else
            {

            }
        }

        private void basalText_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBlock = sender as TextBox;
            Dose_Profile profile_basal = (sender as TextBox).DataContext as Dose_Profile;
            
            if (profile_basal != null)
            {
                foreach (var item in App.db.Dose_Profile.ToList().Where(c => c == profile_basal))
                {
                    item.Coefficient = check.CheckNumber(textBlock.Text);
                    App.db.SaveChanges();
                }
                App.diary_View.collectionRowOfDiary.Refresh();
                App.diary_View.BasalLoad();
                App.diary_View.GraphBasal();
                
                basalList.DataContext = App.db.Dose_Profile.ToList().Where(c => c.ID_Type_Coefficient == 1 && c.Profile == App.diary_View.Selected_Profile);
            }
            else
            {
                MessageBox.Show("Пусто");
            }
        }

        private void NewCarbCoef(object sender, RoutedEventArgs e)
        {
            Windows.CreateCoefficient createCoefficient = new Windows.CreateCoefficient(1);
            createCoefficient.Show();
            createCoefficient.Closed += CreateCoefficient_Closed;
        }

        private void CreateCoefficient_Closed(object sender, EventArgs e)
        {
            carbList.DataContext = App.db.Dose_Profile.ToList().Where(c => c.ID_Type_Coefficient == 2 && c.Profile == App.diary_View.Selected_Profile);
            basalList.DataContext = App.db.Dose_Profile.ToList().Where(c => c.ID_Type_Coefficient == 1 && c.Profile == App.diary_View.Selected_Profile);
        }

        private void New_Basal(object sender, RoutedEventArgs e)
        {
            Windows.CreateCoefficient createCoefficient = new Windows.CreateCoefficient(2);
            createCoefficient.Show();
            createCoefficient.Closed += CreateCoefficient_Closed1;
        }

        private void CreateCoefficient_Closed1(object sender, EventArgs e)
        {
            carbList.DataContext = App.db.Dose_Profile.ToList().Where(c => c.ID_Type_Coefficient == 2 && c.Profile == App.diary_View.Selected_Profile);
            basalList.DataContext = App.db.Dose_Profile.ToList().Where(c => c.ID_Type_Coefficient == 1 && c.Profile == App.diary_View.Selected_Profile);
        }

        private void coefText_GotFocus(object sender, RoutedEventArgs e)
        {
            check.Previous_number = (float)((sender as TextBox).DataContext as Dose_Profile).Coefficient;
        }

        private void basalText_GotFocus(object sender, RoutedEventArgs e)
        {
            check.Previous_number = (float)((sender as TextBox).DataContext as Dose_Profile).Coefficient;
        }
    }
}
