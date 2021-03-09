using DevExpress.Mvvm;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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

        private double _Opacity;
        public double Opacity { get { return _Opacity; } set { _Opacity = value; } }
        Page _CurrentPage;
        public Page CurrentPage { get { return _CurrentPage; } set { _CurrentPage = value; } }
        public Navigation()
        { 
            Opacity = 1;
            
            Gorodishe = new Pages.OstatokGorodishe();
            GlSklad = new Pages.OstatokGlSclad();
            Volno = new Pages.OstatokVolono();
            Brigadi = new Pages.OstatokBrigadi();
        
            CurrentPage = GlSklad;
            NameOfWindow = "  Главный склад  ";
        }

        public string NameOfWindow { get; set; }

        public ICommand Gorodishee
        {
            get { return new RelayCommand(() => { SlowOpacity(Gorodishe); NameOfWindow = "  Городище  "; } ) ; }
        }
        public ICommand GlSclad
        {
            get { return new RelayCommand(() => { SlowOpacity(GlSklad); NameOfWindow = "  Главный склад  "; }); }
        }
        public ICommand Voln
        {
            get { return new RelayCommand(() => {SlowOpacity(Volno); NameOfWindow = "  Вольно  "; }); }
        }
        public ICommand Brigad
        {
            get { return new RelayCommand(() => { SlowOpacity(Brigadi); NameOfWindow = "  Бригады  "; }); }
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
