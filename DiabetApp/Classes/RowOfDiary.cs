using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DiabetApp
{
    public class RowOfDiary: PropertyClass
    {
        int id;
        TimeSpan time;
        List<Diary_Product> diary_Products;
        double he;
        double glucose;
        double dose;
        int id_diary_person;
        GeneralDiary_Person diaty_person;
        double doseHe;
        double doseLower;
        double activeDose;
        double carbohydrates_Coef;
        double basal;
        bool isDoseLower;
        public int Id
        { 
            get { return id; }
            set { id = value; OnPropertyChanged("Id"); }
        }
        public TimeSpan Time
        {
            get { return time; }
            set { time = value; OnPropertyChanged("Time"); }
        }
        public List<Diary_Product> Diary_Products
        {
            get { return diary_Products;}
            set { diary_Products = value; OnPropertyChanged("Diary_Products"); }
        }

        public double He
        {
            get { return Math.Round(HECalculation(Diary_Products),1); }
            set { he = value; OnPropertyChanged("He"); }
        }
        public double Glucose
        {
            get { return glucose; }
            set { glucose = value; OnPropertyChanged("Glcose"); }
        }
        public double Dose
        {
            get { return Math.Round(DoseCalculation(), 1); }
            set { dose = value; OnPropertyChanged("Dose"); }
        }

        public int Id_diary_person
        {
            get { return id_diary_person;}
            set { id_diary_person = value; OnPropertyChanged("Id_diary_person"); }
        }

        public GeneralDiary_Person Diary_Person
        {
            get { return diaty_person; }
            set { diaty_person = value;OnPropertyChanged("Diary_Person"); }
        }

        public double DoseHe
        {
            get { return Math.Round(DoseHeCalculation(),1); }
            set { doseHe = value; OnPropertyChanged("DoseHe"); }
        }
        public float? Carbohydrates_Coef
        {
            get { return App.db.Dose_Profile.ToList().Find(c => c.Time_Begin <= Time && c.Time_End >= Time && c.Profile == App.diary_View.Selected_Profile && c.ID_Type_Coefficient == 2).Coefficient; }
            set { carbohydrates_Coef = (double)value; OnPropertyChanged("Carbohydrates_Coef"); }
        }

        public float? Basal
        {
            get { return App.db.Dose_Profile.ToList().Find(c => c.Time_Begin <= Time && c.Time_End >= Time && c.Profile == App.diary_View.Selected_Profile && c.ID_Type_Coefficient == 1).Coefficient; }
            set { basal = (double)value; OnPropertyChanged("Basal"); }
        }

        public double DoseLower
        {
            get
            {
                if (IsDoseLower == false)
                {
                    return 0;
                }
                else
                {
                    double active = (double)Math.Round(DoseLowerCalculation() - ActiveDose, 1);
                    if (active > 0)
                    {
                        return active;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            set { doseLower = value; OnPropertyChanged("DoseLower"); }
        }

        public bool IsDoseLower
        {
            get { return isDoseLower; }
            set { isDoseLower = value; OnPropertyChanged("IsDoseLower"); }
        }

        public float HECalculation(List<Diary_Product> diary_Product)
        {
            float he = 0;
            if (diary_Product == null)
            {
                return 0;
            }
            else
            {
                foreach (var item in diary_Product)
                {
                    he += (float)item.He;
                }
                return he;
            }
        }
        
        public float DoseHeCalculation()
        {
            return (float)(He * Carbohydrates_Coef);
        }
        public float StandartDoseLower()
        {
            return (float)((Glucose - App.diary_View.Selected_Profile.MaxGlucose) / App.diary_View.Selected_Profile.Sensitivity);
        }

        public float DoseLowerCalculation()
        {
            
            if (Glucose >= App.diary_View.Selected_Profile.MaxGlucose)
            {
                return StandartDoseLower();
            }
            else if (Glucose <= App.diary_View.Selected_Profile.MinGlucose)
            {
                return (float)((Glucose - App.diary_View.Selected_Profile.MinGlucose) / App.diary_View.Selected_Profile.Sensitivity);
            }
            else
            {
                return 0;
            }
        }



        public float DoseCalculation()
        {
            float fulldose = (float)(DoseHeCalculation() + DoseLower);
            if (fulldose < 0)
            {
                return 0;
            }
            else
            {
                return fulldose;
            }
        }

        public string Dose_String()
        {
            return "";
        }

        public float ActiveDoseCalculation()
        {

            RowOfDiary rowOfDiaries = App.diary_View.RowsOfDiary.ToList().Where(c => c.Time < Time && c.Time >= Time - new TimeSpan(4, 0, 0) && c.Diary_Person.Date == App.diary_View.Selected_DateTime && c.IsDoseLower == true).OrderByDescending(c => c.Time).FirstOrDefault();
            //разница во времени между текущей строкой и найденной строкой
            if (rowOfDiaries != null)
            {
                TimeSpan timeSpan = Time - rowOfDiaries.Time;
                //Перевели в часы
                double timedoseHour = timeSpan.TotalMinutes / 60 / 4;// 1 час
                //return (float)(rowOfDiaries.DoseLower - rowOfDiaries.DoseLower * timedoseHour / 4);
                return (float)((float)(rowOfDiaries.ActiveDose + rowOfDiaries.DoseLower) - (timedoseHour * (rowOfDiaries.ActiveDose + rowOfDiaries.DoseLower)));
                
            }
            else
            {
                return 0;
            }
        }


        public float ActiveDose
        {
            get
            {
                float dose = (float)Math.Round(ActiveDoseCalculation(), 1);
                //return (float)Math.Round(ActiveDoseCalculation(),1); 
                if (dose < 0)
                {
                    return 0;
                }
                else
                {
                    return dose;
                }
                //return (float)donelowerdose;
            }
            set
            {
                 activeDose = value; OnPropertyChanged("ActiveDose");
            }
        }

        public string Dose_calculation_str
        {
            get { return Dose_calculation_str; }
            set { Dose_calculation_str = value; OnPropertyChanged("Dose_calculation_str"); }
        }




    }
}
