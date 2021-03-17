using DevExpress.Mvvm;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.EntityFrameworkCore;
using PredpriyatieProject.Tabels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

using System.Windows.Input;

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

            public int IntegerData { get; set; }
            public string StringData { get; set; }


        }

        static byte _kostile;
        public byte kostile { get => _kostile; set { _kostile = value; RefreshTable.Execute(RefreshTable); } }
      
        private DateTime _dateTime;
        public DateTime dateTimeStart { get => _dateTime; set { SetProperty(ref _dateTime, value); RefreshTable.Execute(RefreshTable); } }
        private DateTime _dateTimesecond;
        public DateTime dateTimeEnd { get => _dateTimesecond; set { SetProperty(ref _dateTimesecond, value); RefreshTable.Execute(RefreshTable); } }

        public static MedicineContext MedCont = new MedicineContext();
        public List<Приходвсего> PrihodVse { get; set; }
        public List<УходВсе> UhodVse { get; set; }

        static public List<StorageList> GlList { get; set; } = new List<StorageList>();
        public List<StorageList> AmbulatoriList { get; set; }
        public List<StorageList> GorodList { get; set; }
        public List<StorageList> VolnoList { get; set; }
        public DelegateCommand ResultForDate { get; }
        public DelegateCommand OpenNewaddWindow { get; }

        public string TextForSearch { get => _textForSearch; set => SetProperty(ref _textForSearch, value); }
        private string _textForSearch;
        public ICommand RefreshTable
        {
            get
            {
                return new RelayCommand(() =>
                {
                    GlList = new List<StorageList>();
                    MedCont.НаименованиеЛекарственныхСредствs.Load();
                    MedCont.ДокументыВещиs.Load();
                    MedCont.ПриходРасходs.Load();
                    MedCont.Rofs.Load();
                    MedCont.ТипДокументацииs.Load();
                    MedCont.Складыs.Load();

                    var group = MedCont.НаименованиеЛекарственныхСредствs.Local.Join(MedCont.ДокументыВещиs.Local, x => x.Id, x => x.НаименованиеЦенности, (x, y) => new { doc = y.Документ, name = x.НаименованиеЦенностей, kol = y.Количество });
                    var gr2 = MedCont.ПриходРасходs.Local.Join(group, x => x.Id, x => x.doc, (x, y) => new { type = x.ТипДокумента, slad = x.Склад, name = y.name, kol = y.kol, date = x.Дата });



                    var filterstartForOstatok = gr2.Where(x => (x.type == 1) && (x.slad == 4) && x.date <= dateTimeStart).AsEnumerable();
                    var fitterendForOstatok = gr2.Where(x => (x.type == 2) && (x.slad == 4) && x.date <= dateTimeStart).AsEnumerable();


                    var filterstart = gr2.Where(x => (x.type == 1) && (x.slad == 4) && (x.date >= dateTimeStart) && (x.date <= dateTimeEnd)).AsEnumerable();
                    var filterend = gr2.Where(x => (x.type == 2) && (x.slad == 4) && (x.date >= dateTimeStart) && (x.date <= dateTimeEnd)).AsEnumerable();



                    var grupedend = filterend.GroupBy(x => x.name, (x, y) => new { Name = x, Kol = Int32.Parse(y.Select(x => x.kol).Sum().ToString()) }).ToList();
                    var grupedstart = filterstart.GroupBy(x => x.name, (x, y) => new { Name = x, Kol = Int32.Parse(y.Select(x => x.kol).Sum().ToString()) }).ToList();



                    var ostatokGruppedstart = filterstartForOstatok.GroupBy(x => x.name, (x, y) => new { Name = x, Kol = Int32.Parse(y.Select(x => x.kol).Sum().ToString()) }).ToList();
                    var ostaokGruppedEnd = fitterendForOstatok.GroupBy(x => x.name, (x, y) => new { Name = x, Kol = Int32.Parse(y.Select(x => x.kol).Sum().ToString()) }).ToList();



                    List<Data> prihod = new List<Data>();
                    List<Data> ostatokOnStarListt = new List<Data>();

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




                    foreach (var item1 in grupedstart)
                    {
                        prihod.Add(new Data(item1.Name, item1.Kol));
                    }



                    for (int i = 0; i < prihod.Count; i++)
                    {
                        foreach (var item in grupedend)
                        {
                            if (item.Name == prihod[i].StringData)
                            {
                                prihod[i].IntegerData -= item.Kol;
                            }
                        }
                    }



                    foreach (var item in prihod)
                    {
                        int prihoditem = Int32.Parse(grupedstart.Where(x => x.Name == item.StringData).Select(x => x.Kol).FirstOrDefault().ToString());
                        int rashoditem = Int32.Parse(grupedend.Where(x => x.Name == item.StringData).Select(x => x.Kol).FirstOrDefault().ToString());
                        int ostatokonStartLocal = Int32.Parse(ostatokOnStarListt.Where(x => x.StringData == item.StringData).Select(x => x.IntegerData).FirstOrDefault().ToString());
                        GlList.Add(new StorageList(item.StringData, "", "", ostatokonStartLocal, "", prihoditem, rashoditem));
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

                });
            }
        }




        public FirstVM()
        {

            RefreshTable.Execute(RefreshTable);
            OpenNewaddWindow = new DelegateCommand(() => { AddWind addWind = new AddWind();  addWind.ShowDialog();  RefreshTable.Execute(RefreshTable); });
            ResultForDate = new DelegateCommand(() => { MessageBox.Show(dateTimeEnd.ToString()); });

        }

    }
}
