using DevExpress.Mvvm;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace PredpriyatieProject.ViewModels
{



    class Navigation : ViewModelBase
    {
        private Page Gorodishe;
        private Page GlSklad;
        private Page Volno;
        private Page Brigadi;
        public byte IDbrigad { get => iDbrigad; set { iDbrigad = value; FirstVM.staticSelectedIDBrigad1 = value; } }
        private byte iDbrigad;
       public Visibility cmbvis { get; set; }
        private double _Opacity;
        public double Opacity { get { return _Opacity; } set { _Opacity = value; } }
        Page _CurrentPage;
        public List<string> ComboboxBrigad { get; set; }

        public Page CurrentPage { get { return _CurrentPage; } set { _CurrentPage = value; } }
        public Navigation()
        {
            ComboboxBrigad = FirstVM.MedCont.Бригадыs.Select(x => x.Название).ToList();
            cmbvis = Visibility.Hidden;
            Opacity = 1;
            IDbrigad = 17;
            Gorodishe = new Pages.OstatokGorodishe();
            GlSklad = new Pages.OstatokGlSclad();
            Volno = new Pages.OstatokVolono();
            Brigadi = new Pages.OstatokBrigadi();
            CurrentPage = GlSklad;
            NameOfWindow = "  Главный склад  ";
            FirstVM.staticSelectedIDBrigad1 = 17;
            FirstVM.CurrentPage = 4;
         
        }
        public bool IsLeftDrawerOpen { get; set; }

        public string NameOfWindow { get; set; }

        public ICommand Gorodishee
        {
            get { return new RelayCommand(() => { SlowOpacity(Gorodishe); NameOfWindow = "  Городище  "; FirstVM.CurrentPage = 3; IDbrigad = 17; cmbvis = Visibility.Hidden; });  }
        }
        public ICommand GlSclad
        {
            get { return new RelayCommand(() => { SlowOpacity(GlSklad); NameOfWindow = "  Главный склад  "; FirstVM.CurrentPage = 4; IDbrigad = 17; cmbvis = Visibility.Hidden; }); }
        }
        public ICommand Voln
        {
            get { return new RelayCommand(() => { SlowOpacity(Volno); NameOfWindow = "  Вольно  "; FirstVM.CurrentPage = 2; IDbrigad = 17; cmbvis = Visibility.Hidden; }); }
        }
        public ICommand Brigad
        {
            get { return new RelayCommand(() => { SlowOpacity(Brigadi); NameOfWindow = "  Амбулатория  "; FirstVM.CurrentPage = 1; cmbvis = Visibility.Visible; }); }
        }
        private async void SlowOpacity(Page page)
        {
            await Task.Factory.StartNew(() =>
            {
                for (double i = 1; i > 0; i -= 0.1)
                {
                    Opacity = i;
                }
                CurrentPage = page;
                for (double i = 0; i < 1; i += 0.1)
                {
                    Opacity = i;
                }

            });
        }
    }
}
