using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;


namespace DiabetApp
{
    public class Diary_Product_Property: PropertyClass
    {
       public int id;
       public int id_Product;
       public Product product;
       public int id_Diary_Line;
       public RowOfDiary rowOfDiary_Diary_Line;
       public double gramm;
      
       public double calories;
       public double protein;
       public double fats;
       public double carbohydrates;
        public int Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged("Id"); }
        }
        public int Id_Product
        { 
            get
            {
                return id_Product;
            }
            set { id_Product = value; OnPropertyChanged("Id_Product"); }
        }
        public Product Product
        {
            get { return product; }
            set { product = value; OnPropertyChanged("Product"); }
        }
        public int Id_Diary_Line
        {
            get { return id_Diary_Line; }
            set { id_Diary_Line = value; OnPropertyChanged("ID_Diary_Line"); }
        }
        public RowOfDiary RowOfDiary_Diary_Line
        { 
            get{ return rowOfDiary_Diary_Line; }
            set { rowOfDiary_Diary_Line = value; OnPropertyChanged("RowOfDiary_Diary_Line"); }
        }

        public double Gramm
        {
            get { return gramm; }
            set
            {
                    gramm = value;
                    ProductCalculation();
                    OnPropertyChanged("Gramm");
            }
        }

        public double Calories
        {
            get
            {
                return calories;
            }
            set { calories = value; OnPropertyChanged("Calories"); }
        }
        public double Protein
        {
            get
            {
               return protein;
            }
            set { protein = value; OnPropertyChanged("Protein"); }
        }
        public double Fats
        {
            get
            {
                return fats;
            }
            set { fats = value; OnPropertyChanged("Fats"); }

        }
        public double Carbohydrates
        {
            get 
            {
                return carbohydrates;
               
            }
            set { carbohydrates = value; OnPropertyChanged("Carbohydrates"); }
        }

        public void ProductCalculation()
        {
            if (Product != null)
            {
                double gramm = Convert.ToDouble(Gramm)/100;
                Protein = Math.Round((double)(Product.Protein * gramm),1);
                Fats = Math.Round((double)(Product.Fats * gramm),1);
                Calories = Math.Round((double)(Product.Calories * gramm),1);
                Carbohydrates = Math.Round((double)(Product.Carbohydrates * gramm),1);
            }
        }




    }
}
