using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;
using System.ComponentModel;

namespace DiabetApp
{
    public class Diary_View : PropertyClass
    {
        //ICollectionView
        public ICollectionView collectionProduct;
        public ICollectionView collectionRowOfDiary;
        public ICollectionView collectionDiary_Product;

        //Объекты классов БД
        public Person selected_Person; 
        public Profile selected_Profile;
        public GeneralDiary_Person selected_GenDiary_Person = new GeneralDiary_Person();
        public Diary_Line selected_Diary_lines;
        public Diary_Product selected_Diary_Product;
        public Type_Product selected_Type_Product;
        public Product selected_Product;

        public Person Selected_Person
        {
            get { return selected_Person; }
            set
            {
                selected_Person = value;
                OnPropertyChanged("Selected_Person");
                //App.diary_View = new Diary_View(); 
            }
        }
        public Profile Selected_Profile
        {
            get { return selected_Profile; }
            set
            {
                selected_Profile = value;
                UpdateGraph();
                if (collectionRowOfDiary != null)
                {
                    collectionRowOfDiary.Refresh();
                }
                OnPropertyChanged("Selected_Profile");
            }
        }
        public GeneralDiary_Person Selected_GenDiary_Person
        {
            get { return selected_GenDiary_Person; }
            set { selected_GenDiary_Person = value; OnPropertyChanged("Selected_GenDiary_Person"); }
        }
        public Diary_Line Selected_Diary_lines
        {
            get { return selected_Diary_lines; }
            set { selected_Diary_lines = value; OnPropertyChanged("Selected_Diary_lines"); }
        }
        public Diary_Product Selected_Diary_Product
        {
            get { return selected_Diary_Product; }
            set
            {
                selected_Diary_Product = value;
                OnPropertyChanged("Selected_Diary_Product");
            }
        }
        public Type_Product Selected_Type_Product
        {
            get { return selected_Type_Product; }
            set { selected_Type_Product = value; OnPropertyChanged("Selected_Type_Product"); }

        }
        public Product Selected_Product
        {
            get { return selected_Product; }
            set { selected_Product = value; OnPropertyChanged("Selected_Product"); }
        }



        //Объекты клвссов приложения

        public RowOfDiary selected_RowsOfDiary;
        public Diary_Product_Property selected_Diary_Product_Property;
        public DateTime selected_DateTime;
        public Coefficient selected_Coefficient = new Coefficient();

        public RowOfDiary Selected_RowsOfDiary
        {
            get { return selected_RowsOfDiary; }
            set { selected_RowsOfDiary = value; OnPropertyChanged("Selected_RowsOfDiary"); }
        }
        public Diary_Product_Property Selected_Diary_Product_Property
        {
            get { return selected_Diary_Product_Property; }
            set { selected_Diary_Product_Property = value; OnPropertyChanged("Selected_Diary_Product_Property"); }
        }
        public DateTime Selected_DateTime
        {
            get { return selected_DateTime; }
            set
            {
                selected_DateTime = value;
                UpdateGraph();
                Selected_GenDiary_Person = App.db.GeneralDiary_Person.ToList().Find(c => c.Date == Selected_DateTime && c.ID_Person == Selected_Person.ID);

                collectionRowOfDiary.Filter = UpdateDatetime;
                collectionRowOfDiary.Refresh();

                OnPropertyChanged("Selected_DateTime");
            }
        }
        public Coefficient Selected_Coefficient
        {
            get { return selected_Coefficient; }
            set { selected_Coefficient = value; OnPropertyChanged("Selected_Coefficient"); }
        }



        //ObservableCollection коллекции
        public ObservableCollection<Profile> Profiles { get; set; } = new ObservableCollection<Profile>(); 
        public ObservableCollection<GeneralDiary_Person> Gen_D_P { get; set; } = new ObservableCollection<GeneralDiary_Person>();        
        public ObservableCollection<RowOfDiary> RowsOfDiary { get; set;} = new ObservableCollection<RowOfDiary>();
        public ObservableCollection<Diary_Product> Diary_Products { get; set; }= new ObservableCollection<Diary_Product>();
        public ObservableCollection<Type_Product> Type_Products { set; get; } = new ObservableCollection<Type_Product>();
        public ObservableCollection<Product> Products { get; set; }= new ObservableCollection<Product>();


        public ObservableCollection<Dose_Profile> Basals { get; set; } = new ObservableCollection<Dose_Profile>();
        public ObservableCollection<Coefficient> basalCoef = new ObservableCollection<Coefficient>();
        public ObservableCollection<Coefficient> BasalCoef
        {
            get { return basalCoef; }
            set { basalCoef = value; OnPropertyChanged("BasalCoef"); }
        }

        public ObservableCollection<Dose_Profile> carbCoef = new ObservableCollection<Dose_Profile>();
        public ObservableCollection<Dose_Profile> CarbCoef
        {
            get { return carbCoef; }
            set { carbCoef = value; OnPropertyChanged("CarbCoef"); }
        }


        //Коллекции точек для вывода графиков

        public SeriesCollection seriesPointsGlucose = new SeriesCollection();
        public SeriesCollection seriesPointsBasal = new SeriesCollection();
        public SeriesCollection seriesPointsCoef = new SeriesCollection();

        public SeriesCollection SeriesPointsGlucose
        { get { return seriesPointsGlucose; } set { seriesPointsGlucose = value; OnPropertyChanged("SeriesPointsGlucose"); } }
        public SeriesCollection SeriesPointsBasal
        { get { return seriesPointsBasal; } set { seriesPointsBasal = value; OnPropertyChanged("SeriesPointsBasal"); } }
        public SeriesCollection SeriesPointsCoef
        { get { return seriesPointsCoef; } set { seriesPointsCoef = value; OnPropertyChanged("SeriesPointsCoef"); } }
        public List<string> TimesGluc { get; set; } = new List<string>();
        public List<string> TimesCoef { get; set; } = new List<string>();
        public List<string> TimesBasal { get; set; } = new List<string>();



        //Сумма коэффициентов за сутки
        public float summDose = 0;
        public float summBasal = 0;
        public float SummBasal
        {
            get { return summBasal; }
            set { summBasal = value; OnPropertyChanged("SummBasal"); }
        }
        public float SummDose
        {
            get { return summDose; }
            set { summDose = value; OnPropertyChanged("SummDose"); }
        }
        public async void CalculationSummDose()
        {
            SummDose = (float)RowsOfDiary.ToList().Where(c => c.Diary_Person.Date == Selected_DateTime).ToList().Sum(c => c.Dose);
            await Task.Delay(0);
        }

        //загрузка списка продуктов
        public void ProductsAdd()
        {
            Products.Clear();
            foreach (var item in App.db.Product.ToList())
            {
                Products.Add(new Product()
                {
                    ID = item.ID,
                    Name = item.Name,
                    Calories = item.Calories,
                    Carbohydrates = item.Carbohydrates,
                    Fats = item.Fats,
                    Protein= item.Protein,
                    Glycemic_Index = item.Glycemic_Index,

                    Type_Product = item.Type_Product,
                    ID_Type_Product = item.ID_Type_Product
                });
            }
            
        }
        public void Type_ProductsAdd()
        {
            Type_Products.Clear();
            foreach (var item in App.db.Type_Product)
            {
                Type_Products.Add(item);
            }
        }

        //загрузка профилей пользователя
        public void ProfilesAdd()
        {
            Profiles.Clear();
            foreach (var item in App.db.Profile.ToList().Where(c=> c.Person.ID == Selected_Person.ID).ToList())
            {
                Profiles.Add(item);
            }
        }

        //загрузка журнала пользователя
        public async void RowOfDiaryAdd()
        {
            await Task.Delay(0);
            Diary_Products.Clear();
            RowsOfDiary.Clear();
            if (Selected_Person != null)
            {
                
         
                if (Selected_DateTime.Year < 10)
                { 
                    List<DateTime?> dates = new List<DateTime?>();
                    dates = App.db.Diary_Line.ToList().Where(c => c.GeneralDiary_Person.ID_Person == Selected_Person.ID).ToList().Select(c => c.GeneralDiary_Person.Date).ToList();
                    if (dates.Count == 0)
                    {
                        Selected_DateTime = DateTime.Now.Date;
                        var addGenDiary_Person = App.db.GeneralDiary_Person.Add(new GeneralDiary_Person()
                        { 
                            ID_Person = Selected_Person.ID,
                            Date = DateTime.Now.Date
                        });
                        App.db.SaveChanges();
                        Selected_GenDiary_Person = addGenDiary_Person;
                    }
                    else
                    {
                        Selected_DateTime = dates.Max().Value.Date;
                    }
                }

                Selected_GenDiary_Person.Person = Selected_Person;
                foreach (var item in App.db.Diary_Line.ToList().Where(c => c.GeneralDiary_Person.ID_Person == Selected_Person.ID).ToList())
                {
                    float he = 0;
                    foreach (var item2 in item.Diary_Product.ToList())
                    {
                        Diary_Products.Add(new Diary_Product()
                        {
                            ID = item2.ID,
                            Diary_Line = item2.Diary_Line,
                            Product = item2.Product,
                            Grams = item2.Grams,
                            ID_Product = item2.ID_Product,
                            ID_Diary_Line = item2.ID_Diary_Line
                        });
                        he += (float)Math.Round((double)(item2.Product.Carbohydrates * item2.Grams / 100 / 10), 1);
                    }
                    RowOfDiary row = new RowOfDiary()
                    {
                        
                        Id = item.ID,
                        Time = (TimeSpan)item.Time,
                        Diary_Products = item.Diary_Product.ToList(),
                        Id_diary_person = (int)item.ID_GeneralDiary_Person,
                        Diary_Person = item.GeneralDiary_Person,
                        Glucose = (double)item.Glucose,
                        IsDoseLower = (bool)item.IsDoseLower,

                    };
                    RowsOfDiary.Add(row);
                            //item.Time = row.Time;
                            //item.Glucose = (float?)row.Glucose;

                            //item.Diary_Product = row.Diary_Products.ToList();

                            //item.ID_GeneralDiary_Person = row.Id_diary_person;
                            //item.GeneralDiary_Person = row.Diary_Person;
                            //item.IsDoseLower = row.IsDoseLower;
                }

                //ColumnSeries columnSeries = new ColumnSeries();
                //columnSeries.Values = 

                UpdateGraph();
            }
            else
            {
               
            }
        }

        //обновление журнала по выбранной дате
        public bool UpdateDatetime(object obj)
        {
            RowOfDiary rowOfDiary = (RowOfDiary)obj;
            if (rowOfDiary.Diary_Person.Date == Selected_DateTime)
            {
                return true;
            }
            return false;
        }

        //методы для вывода графиков
        public void UpdateGraph()
        {       
                BasalLoad();
                Carb_CoefLoad();
                GraphGluc();
                GraphCoef();
                GraphBasal();
        }
        public void GraphGluc()
        {
            SeriesPointsGlucose = new SeriesCollection();
            TimesGluc.Clear();
            ChartValues<double> glucose = new ChartValues<double>();
            ChartValues<double> he = new ChartValues<double>();
            ChartValues<double> dose = new ChartValues<double>();
            LineSeries line = new LineSeries() { Title = "Сахар"};
            ColumnSeries columnHe = new ColumnSeries() { Title = "ХЕ"};
            ColumnSeries columnDose = new ColumnSeries() { Title = "Доза"};
            
            foreach (var item in RowsOfDiary.ToList().Where(c => c.Diary_Person.Date == Selected_DateTime).ToList())
            {
                TimesGluc.Add(item.Time.ToString());
                glucose.Add(Math.Round(item.Glucose,1));
                he.Add(Math.Round(item.He,1));
                dose.Add(Math.Round(item.Dose,1));
            }
            line.Values = glucose;
            columnDose.Values = dose;
            columnHe.Values = he;
            SeriesPointsGlucose.Add(line);
            SeriesPointsGlucose.Add(columnHe);
            SeriesPointsGlucose.Add(columnDose);
        }
        public void GraphCoef()
        {
            SeriesPointsCoef = new SeriesCollection();
            LineSeries line1 = new LineSeries() { Title = "Коэф-т"};
            ChartValues<double> coef = new ChartValues<double>();
            TimesCoef.Clear();
            foreach (var item in CarbCoef.ToList())
            {
                coef.Add(Math.Round((double)item.Coefficient,1));
                TimesCoef.Add(item.Time_Begin.ToString());
            }
            line1.Values = coef;
            
            SeriesPointsCoef.Add(line1);
        }
        public void GraphBasal()
        {

            SeriesPointsBasal = new SeriesCollection();
            LineSeries line2 = new LineSeries() { Title = "Коэф-т" };
            ChartValues<double> basal = new ChartValues<double>();
            
            TimesBasal.Clear();
            foreach (var item in App.diary_View.BasalCoef.ToList())
            {
                basal.Add(Math.Round((double)item.Coef,2));
                TimesBasal.Add(item.TimeBegin.ToString());
            }
            line2.Values = basal;
            SeriesPointsBasal.Add(line2);
        }
        public void BasalLoad()
        {
            BasalCoef.Clear();
            SummBasal = 0;
            foreach (var item in App.db.Dose_Profile.ToList().Where(c=> c.ID_Profile == Selected_Profile.ID && c.ID_Type_Coefficient == 1))
            {
                BasalCoef.Add(new Coefficient()
                {
                    Id = item.ID,
                    TimeBegin = (TimeSpan)item.Time_Begin,
                    TimeEnd = (TimeSpan)item.Time_End,
                    Coef = (double)item.Coefficient
                });
                SummBasal += (float)((TimeSpan)item.Time_End - (TimeSpan)item.Time_Begin).TotalMinutes / 15 * (float)item.Coefficient;
            }
        }
        public void Carb_CoefLoad()
        {
            CarbCoef.Clear();
           
            foreach (var item in App.db.Dose_Profile.ToList().Where(c => c.ID_Profile == Selected_Profile.ID && c.ID_Type_Coefficient == 2))
            {
                CarbCoef.Add(new Dose_Profile()
                {
                    ID = item.ID,
                    Time_Begin = (TimeSpan)item.Time_Begin,
                    Time_End = (TimeSpan)item.Time_End,
                    Coefficient = item.Coefficient
                });
            }
            App.diary_View.CalculationSummDose();
        }

        //Конструктор до авторизации
        public Diary_View()
        {
            collectionProduct = CollectionViewSource.GetDefaultView(Products);
            collectionRowOfDiary = CollectionViewSource.GetDefaultView(RowsOfDiary);
            collectionDiary_Product = CollectionViewSource.GetDefaultView(Diary_Products);
            collectionProduct.Filter = FilterProduct;
        }

        //конструктор после авторизации
        public Diary_View(Person person)
        {
            collectionProduct = CollectionViewSource.GetDefaultView(Products);
            collectionRowOfDiary = CollectionViewSource.GetDefaultView(RowsOfDiary);
            collectionDiary_Product = CollectionViewSource.GetDefaultView(Diary_Products);
            collectionProduct.Filter = FilterProduct;
            if (person == null)
            {

            }
            else
            {
                Selected_Person = person;
                ProfilesAdd();
                if (Selected_Profile != null)
                {
                    LoadDiaryView();
                }
                else
                {

                }
                
            }
        }

        //Загрузка данных для пользователя с выбранным профилем
        public void LoadDiaryView()
        {
            Type_ProductsAdd();
            ProductsAdd();
            RowOfDiaryAdd();
            
        }
        private void RowsOfDiary_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            collectionRowOfDiary.Refresh();
            App.diary_View.CalculationSummDose();
        }



        //Фильтрация продуктов по типу
        public string filter_Type_Product;
        public string Filter_Type_Product
        {
            get { return filter_Type_Product; }
            set
            {
                filter_Type_Product = value;
                collectionProduct.Refresh();
                OnPropertyChanged("Filter_Type_Product");
            }
        }
        public bool FilterProduct(object obj)
        {
            Product product = (Product)obj;
            if (string.IsNullOrWhiteSpace(Filter_Type_Product))
            {
                return true;
            }
            else if(product.Type_Product.Name.Contains(Filter_Type_Product))
            {
                return true;
            }
            return false;
        }

    }
}
