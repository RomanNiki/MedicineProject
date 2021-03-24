using DevExpress.Mvvm;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.EntityFrameworkCore;
using PredpriyatieProject.Tabels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace PredpriyatieProject.ViewModels
{
    public class FirstVM : OnPropertyChangedClass
    {
        public class Data
        {
            public Data(string strValue, int intValue)
            {
                IntegerData = intValue;
                StringData = strValue;
            }
            public Data(string strValue, int intValue, int id)
            {
                IntegerData = intValue;
                StringData = strValue;
                this.id = id;
            }
            public int IntegerData { get; set; }
            public string StringData { get; set; }
            public int id { get; set; }

        }

        public FirstVM()
        {
            dateTimeStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            dateTimeEnd = DateTime.Today;
            kostile = 4;
            OpenNewaddWindow = new DelegateCommand(() => { AddWind addWind = new AddWind(); addWind.ShowDialog(); RefreshTable.Execute(RefreshTable); });
            ResultForDate = new DelegateCommand(() => { MessageBox.Show(dateTimeEnd.ToString()); });
        }

        static byte _kostile;
        public static byte kostile { get => _kostile; set { _kostile = value; } }
        private DateTime _dateTime;
        public DateTime dateTimeStart { get => _dateTime; set { SetProperty(ref _dateTime, value); RefreshTable.Execute(RefreshTable); } }
        private DateTime _dateTimesecond;
        public DateTime dateTimeEnd { get => _dateTimesecond; set { SetProperty(ref _dateTimesecond, value); RefreshTable.Execute(RefreshTable); } }
        public static MedicineContext MedCont = new MedicineContext();
        public List<Приходвсего> PrihodVse { get; set; }
        public List<УходВсе> UhodVse { get; set; }
        private List<StorageList> _GlList;
        public List<StorageList> GlList { get => _GlList; set { SetProperty(ref _GlList, value); } }
        public List<StorageList> AmbulatoriList { get; set; }
        public List<StorageList> GorodList { get; set; }
        public List<StorageList> VolnoList { get; set; }
        public DelegateCommand ResultForDate { get; }
        public DelegateCommand OpenNewaddWindow { get; }
        public string TextForSearch
        {
            get => _textForSearch; set
            {
                if (_textForSearch == value) return;
                SetProperty(ref _textForSearch, value);
            }
        }
        private string _textForSearch;

        private void UpdateResourse()
        {

            MedCont.НаименованиеЛекарственныхСредствs.Load();
            MedCont.ДокументыВещиs.Load();
            MedCont.ПриходРасходs.Load();
            MedCont.Rofs.Load();
            MedCont.ТипДокументацииs.Load();
            MedCont.Складыs.Load();

        }

        public ICommand RefreshTable
        {
            get
            {
                return new RelayCommand(() =>
                {
                    UpdateResourse();
                    //Загрузка данных 
                    GlList = new List<StorageList>();



                    //Соединение таблиц
                    var group = MedCont.НаименованиеЛекарственныхСредствs.Local.Join(MedCont.ДокументыВещиs.Local, x => x.Id, x => x.НаименованиеЦенности, (x, y) => new { doc = y.Документ, name = x.НаименованиеЦенностей, kol = y.Количество, ide = x.ЕдиницаИзмерения });
                    var gr2 = MedCont.ПриходРасходs.Local.Join(group, x => x.Id, x => x.doc, (x, y) => new { type = x.ТипДокумента, slad = x.Склад, name = y.name, kol = y.kol, date = x.Дата, id = y.ide });


                    //Выборка для определения остатка на начало
                    var filterstartForOstatok = gr2.Where(x => (x.type == 1) && (x.slad == kostile) && x.date <= dateTimeStart).AsEnumerable();
                    var fitterendForOstatok = gr2.Where(x => (x.type == 2) && (x.slad == kostile) && x.date <= dateTimeStart).AsEnumerable();

                    //Выборки для получение текущих приходов расходов
                    var filterstart = gr2.Where(x => (x.type == 1) && (x.slad == 4) && (x.date >= dateTimeStart) && (x.date <= dateTimeEnd)).AsEnumerable();
                    var filterend = gr2.Where(x => (x.type == 2) && (x.slad == 4) && (x.date >= dateTimeStart) && (x.date <= dateTimeEnd)).AsEnumerable();


                    //Группировка по имени ценности
                    var grupedend = filterend.GroupBy(x => x.name, (x, y) => new { Name = x, Kol = Int32.Parse(y.Select(x => x.kol).Sum().ToString()) }).ToList();
                    var grupedstart = filterstart.GroupBy(x => x.name, (x, y) => new { Name = x, Kol = Int32.Parse(y.Select(x => x.kol).Sum().ToString()), id = Int32.Parse(y.Select(x => x.id).FirstOrDefault().ToString()) }).ToList();


                    //тоже самое только для остатка на начало
                    var ostatokGruppedstart = filterstartForOstatok.GroupBy(x => x.name, (x, y) => new { Name = x, Kol = Int32.Parse(y.Select(x => x.kol).Sum().ToString()) }).ToList();

                    var ostaokGruppedEnd = fitterendForOstatok.GroupBy(x => x.name, (x, y) => new { Name = x, Kol = Int32.Parse(y.Select(x => x.kol).Sum().ToString()) }).ToList();


                    //Лист для получение текущих приходов расходов
                    List<Data> prihod = new List<Data>();
                    //Лист для остатка на начало
                    List<Data> ostatokOnStarListt = new List<Data>();


                    //заполнение листа и определ остатка на начало
                    foreach (var start in ostatokGruppedstart)
                    {
                        int k = 0;
                        foreach (var end in ostaokGruppedEnd)
                        {
                            if (start.Name == end.Name) { k += 1; ostatokOnStarListt.Add(new Data(start.Name, start.Kol - end.Kol)); }
                        }
                        if (k == 0)
                        {
                            ostatokOnStarListt.Add(new Data(start.Name, start.Kol));

                        }
                    }



                    //Лист для получение текущих приходов расходов
                    foreach (var item1 in grupedstart)
                    {
                        //  MessageBox.Show(item1.id.ToString());
                        prihod.Add(new Data(item1.Name, item1.Kol, Int32.Parse(item1.id.ToString())));
                    }





                    //Заполнение таблицы 
                    foreach (var item in prihod)
                    {
                        int prihoditem = Int32.Parse(grupedstart.Where(x => x.Name == item.StringData).Select(x => x.Kol).FirstOrDefault().ToString());
                        int rashoditem = Int32.Parse(grupedend.Where(x => x.Name == item.StringData).Select(x => x.Kol).FirstOrDefault().ToString());
                        int ostatokonStartLocal = Int32.Parse(ostatokOnStarListt.Where(x => x.StringData == item.StringData).Select(x => x.IntegerData).FirstOrDefault().ToString());

                        string EdIzm = MedCont.НаименованиеЛекарственныхСредствs.Join(MedCont.ЕдиницыИзмеренияs, x => x.ЕдиницаИзмерения, y => y.Код, (x, y) => new { edizmint = x.ЕдиницаИзмерения, edizmItem = y.ЕдИзмерения }).Where(x => x.edizmint == item.id).Select(x => x.edizmItem).FirstOrDefault().ToString();
                        GlList.Add(new StorageList(item.StringData, EdIzm, "", ostatokonStartLocal, "", prihoditem, rashoditem));
                    }

                });
            }
        }

        public ICommand Addbutte
        {
            get
            {
                return new RelayCommand(() =>
                {
                    PrintDialog printDialog = new PrintDialog();
                    if (printDialog.ShowDialog() == true)
                    {
                        TextBlock visual = new TextBlock();
                        foreach (var a in GlList)
                        {
                            Run run = new Run(a.ToString());
                            // Создать текст


                            // Поместить его в TextBlock

                            visual.Inlines.Add(run);

                            // Использовать поля для получения рамки страницы
                            visual.Margin = new Thickness(5);
                        }
                        // Увеличить TextBlock по обоим измерениям в 5 раз. 
                        // (В этом случае увеличение шрифта дало бы тот же эффект, 
                        // потому что TextBlock — единственный элемент)
                        visual.LayoutTransform = new ScaleTransform(5, 5);

                        // Установить размер элемента
                        Size pageSize = new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight);
                        visual.Measure(pageSize);
                        visual.Arrange(new Rect(0, 0, pageSize.Width, pageSize.Height));

                        // Напечатать элемент
                        printDialog.PrintVisual(visual, "Распечатываем текст");
                      
                    }
                });
            }
        }

   

    }
}
