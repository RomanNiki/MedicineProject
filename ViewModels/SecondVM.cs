using DevExpress.Mvvm;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace PredpriyatieProject.ViewModels
{
    public class Doctype
    {
        public string Name { get; set; }
        public int idDoc { get; set; }
        public string TypeSTR { get; set; }
        public DateTime Date { get; set; }

        public Doctype(string name, string typeSTR, int iddoc, DateTime date)
        {
            Name = name;
            TypeSTR = typeSTR;
            idDoc = iddoc;
            Date = date;
        }
    }

    public class SecondVM : OnPropertyChangedClass
    {
        public List<Doctype> listOfDOcs { get; set; }
        public Doctype selectedItemDataGrid { get; set; }
        public DelegateCommand OpenNewaddWindow { get; set; }
        private MedicineContext medicineContext = FirstVM.MedCont;
        private string _selectedItemScladi;
        public string SelectedItemScladi { get => _selectedItemScladi; set => ComboboxPodrazdeleniyaHelper(value); }
        private string _selecteditemPodrazd;
        public string SelecteditemPodrazd { get => _selecteditemPodrazd; set => SetProperty(ref _selecteditemPodrazd, value); }
        private string _nameOfDoc;
        public string NameOfDoc { get => _nameOfDoc; set => SetProperty(ref _nameOfDoc, value); }
        private DateTime _dateOfDoc;
        public DateTime DateOfDoc { get => _dateOfDoc; set => SetProperty(ref _dateOfDoc, value); }
        private string _kolvoTextbox;     
        public string KolvoTextbox { get => _kolvoTextbox; set => SetProperty(ref _kolvoTextbox, value); }
        public int SelectedIndCennsoti { get; set; } = 0;
        private string nameOfToggleButton = "Приход";
        public string NameOfToggleButton { get => nameOfToggleButton; set => nameOfToggleButton = value; }
        private bool isChechedAddWind;
        public IEnumerable<string> ComboboxPodrazdeleniya { get; set; }
        public List<string> NameOfDOcumentList { get; set; }
        public IEnumerable<string> ComboboxNazvLecarstvХ => medicineContext.НаименованиеЛекарственныхСредствs.ToList().Select(a => a.НаименованиеЦенностей);
        public IEnumerable<string> ComboboxScladi => medicineContext.Складыs.ToList().Select(a => a.Название);
        public bool IsChechedAddWind
        {
            get => isChechedAddWind;
            set
            {
                if (value == true)
                {
                    NameOfToggleButton = "Расход";
                }
                else
                {
                    NameOfToggleButton = "Приход";
                }
                isChechedAddWind = value;
            }
        }

        void ComboboxPodrazdeleniyaHelper(string value)
        {
            switch (value)
            {
                case "Амбулатория":
                    ComboboxPodrazdeleniya = medicineContext.Бригадыs.ToList().Select(a => a.Название);
                    SelecteditemPodrazd = ComboboxPodrazdeleniya.Last();
                    break;
                default:
                    ComboboxPodrazdeleniya = medicineContext.Бригадыs.ToList().Select(a => a.Название).Where(a => a == "Склад");
                    SelecteditemPodrazd = ComboboxPodrazdeleniya.First();
                    break;
            }
            SetProperty(ref _selectedItemScladi, value);
        }

        public ICommand Addbutt
        {
            get { return new RelayCommand(() => { AddButtonMethod(); }); }
        }

        private void AddButtonMethod()
        {
            if (selectedItemDataGrid != null)
            {
                medicineContext.НаименованиеЛекарственныхСредствs.Load();
                medicineContext.ДокументыВещиs.Load();
                medicineContext.ПриходРасходs.Load();
                medicineContext.Rofs.Load();
                medicineContext.ТипДокументацииs.Load();
                medicineContext.Складыs.Load();
                ПриходРасход приходРасходadd = medicineContext.ПриходРасходs.Where(x => x.Id == selectedItemDataGrid.idDoc).Select(x => x).FirstOrDefault();
                ДокументыВещи документыВещиadd = new ДокументыВещи();
                документыВещиadd.НаименованиеЦенностиNavigation = medicineContext.НаименованиеЛекарственныхСредствs.Where(f => f.Id == SelectedIndCennsoti + 1).FirstOrDefault();
                документыВещиadd.Количество = Convert.ToInt32(KolvoTextbox);
                документыВещиadd.ДокументNavigation = приходРасходadd;
                medicineContext.ДокументыВещиs.Add(документыВещиadd);
                medicineContext.SaveChanges();
            }
        }

        public SecondVM()
        {
            listOfDOcs = new List<Doctype>();
            OpenNewaddWindow = new DelegateCommand(() => { AddChangeDocument addWind = new AddChangeDocument(); addWind.ShowDialog(); });
            KolvoTextbox = "1";
            DateOfDoc = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            RefreshTable();
        }

        void RefreshTable()
        {
            DateTime dateTimeStart = new DateTime(DateOfDoc.Year, DateOfDoc.Month, 1);
            var ae = medicineContext.ПриходРасходs.Where(x => x.Дата >= DateOfDoc && x.Склад == 4/*medicineContext.Складыs.Where(x=>x.Название==SelectedItemScladi).Select(x=>x.Id).FirstOrDefault()*/).Select(x => new { id = x.Id, name = x.Название, type = x.ТипДокументаNavigation.Название, date = x.Дата });
            foreach (var item in ae)
                listOfDOcs.Add(new Doctype(item.name, item.type, item.id, (DateTime)item.date));
        }
    }
}
