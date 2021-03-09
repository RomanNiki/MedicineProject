using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace PredpriyatieProject.ViewModels
{
    public class SecondVM : OnPropertyChangedClass 
    {
        private string _selectedItemScladi;
        private string _nameOfDoc;
        private DateTime _dateOfDoc;
        private string _kolvoTextbox;
        //private readonly Model model;
        private string _selecteditemPodrazd;
        public string SelectedItemScladi { get => _selectedItemScladi; set => ComboboxPodrazdeleniyaHelper(value); }
        public string NameOfDoc { get => _nameOfDoc; set => SetProperty(ref _nameOfDoc, value); }

        public DateTime DateOfDoc { get => _dateOfDoc; set => SetProperty(ref _dateOfDoc, value); }
        public string KolvoTextbox { get => _kolvoTextbox;  set => SetProperty(ref _kolvoTextbox, value); }
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
        public string SelecteditemPodrazd { get => _selecteditemPodrazd;  set => SetProperty(ref _selecteditemPodrazd, value); }
        public int SelectedIndCennsoti { get; set; } = 0;

        MedicineContext medicineContext = new MedicineContext();
        private string nameOfToggleButton = "Приход";
        private bool isChechedAddWind;
        public IEnumerable<string> ComboboxPodrazdeleniya { get; set; } 
        public string NameOfToggleButton { get => nameOfToggleButton; set => nameOfToggleButton = value; } 
        public IEnumerable<string> ComboboxNazvLecarstvХ => medicineContext.НаименованиеЛекарственныхСредствs.ToList().Select(a => a.НаименованиеЦенностей);
        public IEnumerable<string> ComboboxScladi => medicineContext.Складыs.ToList().Select(a => a.Название);
        public bool IsChechedAddWind
        {
            get => isChechedAddWind; 
            set
            {
                if (value == true)
                {
                    NameOfToggleButton = "Уход";
                }
                else
                {
                    NameOfToggleButton = "Приход";
                }
                isChechedAddWind = value;
            } }
        public DelegateCommand AddButton = new DelegateCommand(() => { AddButtonMethod(); });

       static private void AddButtonMethod()
        {

        }
        public SecondVM(/*Model model*/)
        {
            KolvoTextbox = "1";
            DateOfDoc = DateTime.Today;
           
        }

        //private void ModelValueChanged(object sender, string valueName, object oldValue, object newValue)
        //{
        //    switch (valueName)
        //    {
        //        case nameof(Model.StringValue): Text = (string)newValue; break;
        //        case nameof(Model.IntValue): Number = (int)newValue; break;
        //    }
        //}

        //protected override void PropertyNewValue<T>(ref T fieldProperty, T newValue, string propertyName)
        //{
        //    base.PropertyNewValue(ref fieldProperty, newValue, propertyName);

        //    switch (propertyName)
        //    {
        //        case nameof(Text): model.SendValue(nameof(Model.StringValue), Text); break;
        //        case nameof(Number): model.SendValue(nameof(Model.IntValue), Number); break;
        //    }

        //}
    }
}
