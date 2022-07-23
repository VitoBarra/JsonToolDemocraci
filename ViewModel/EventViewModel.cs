using MVVM.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;
using Caliburn.Micro;
using System;

namespace ToolDemocraci.ViewModel
{

    public class AnswereViewModel : BaseViewModel
    {

        [JsonIgnore]
        public RelayCommand e { get; set; }
        [JsonIgnore]
        private StatsTypeViewModel _Dati = new StatsTypeViewModel();
        [JsonIgnore]
        private string _Text = "";
        [JsonIgnore]
        private int _Conseguenza = 0;



        [JsonProperty("dati")]
        public StatsTypeViewModel Dati { get => _Dati; set { _Dati = value; Notify(); e?.RaiseCanExecuteChanged(); } }

        [JsonProperty("text")]
        public string Text { get =>_Text; 
            set { _Text = value; Notify(); e?.RaiseCanExecuteChanged(); } }

        [JsonProperty("cons")]
        public int Conseguenza { get => _Conseguenza; set { _Conseguenza = value; Notify(); } }

        //merda
        [JsonIgnore]
        public IList<KeyStringViewModel> _comboBoxValue = new BindableCollection<KeyStringViewModel>();
        [JsonIgnore]
        public IList<KeyStringViewModel> ComboBoxValue
        {
            get => _comboBoxValue;
            set { _comboBoxValue = value; Notify(); }
        }

        [JsonIgnore]
        private KeyStringViewModel _comboBoxItem;

        [JsonIgnore]
        public KeyStringViewModel ComboBoxItem
        {
            get => _comboBoxItem;
            set
            {
                _comboBoxItem = value; Notify(); SelectionChanged();
            }
        }



        private void SelectionChanged()
        {
            Conseguenza = ComboBoxItem.Key;
        }
    }

    public class EventViewModel : BaseViewModel
    {
        [JsonIgnore]
        public RelayCommand e { get; set; }
        [JsonIgnore]
        private AnswereViewModel _Answere1 = new AnswereViewModel();
        [JsonIgnore]
        private AnswereViewModel _Answere2 = new AnswereViewModel();
        [JsonIgnore]
        private string _Descrizione;
        [JsonIgnore]
        private string _Name;
        [JsonIgnore]
        private string _Persona;
        [JsonIgnore]
        private IList<string> _restrictParty = new List<string>();
        [JsonIgnore]
        private IList<string> _exceptParty = new List<string>();
        [JsonIgnore]
        private IList<string> _restrictPr = new List<string>();      
        [JsonIgnore]
        private IList<string> _exceptPr = new List<string>();
        [JsonIgnore]
        private int? _Time;



        [JsonProperty("ans1")]
        public AnswereViewModel Answere1 { get => _Answere1; set { Answere1 = value; Notify(); e?.RaiseCanExecuteChanged(); } }

        [JsonProperty("ans2")]
        public AnswereViewModel Answere2 { get => _Answere2; set { Answere2 = value; Notify(); e?.RaiseCanExecuteChanged(); } }

        [JsonProperty("desc")]
        public string Descrizione { get => _Descrizione; set 
            { _Descrizione = value; Notify(); e?.RaiseCanExecuteChanged(); } }

        [JsonProperty("name")]
        public string Name { get => _Name; set 
            { _Name = value; Notify(); e?.RaiseCanExecuteChanged(); } }

        [JsonProperty("person")]
        public string Persona { get => _Persona; set { _Persona = value; Notify(); e?.RaiseCanExecuteChanged(); } }

        [JsonProperty("restrictParty")]
        public IList<string> RestrictParty { get => _restrictParty; set { _restrictParty = value; Notify(); e?.RaiseCanExecuteChanged(); } }

        [JsonProperty("restrictPr")]
        public IList<string> RestrictPr { get => _restrictPr; set { _restrictPr = value; Notify(); e?.RaiseCanExecuteChanged(); } }

        [JsonProperty("exceptParty")]
        public IList<string> ExceptParty { get => _exceptParty; set { _exceptParty = value; Notify(); e?.RaiseCanExecuteChanged(); } }

        [JsonProperty("exceptPr")]
        public IList<string> ExceptPr { get => _exceptPr; set { _exceptPr = value; Notify(); e?.RaiseCanExecuteChanged(); } }

        [JsonProperty("time")]
        public int? Time { get => _Time; set { _Time = value; Notify(); e?.RaiseCanExecuteChanged(); } }

        public bool AllFieldAreValid()
        {
            var  a = !(string.IsNullOrEmpty(Descrizione) ||
                   string.IsNullOrEmpty(Name) ||
                   string.IsNullOrEmpty(Persona) ||
                   string.IsNullOrEmpty(Answere1.Text) ||
                   string.IsNullOrEmpty(Answere2.Text));
            return a;
        }
    }

    public class StatsTypeViewModel : BaseViewModel
    {
        [JsonIgnore]
        private int _Consenso = 0;
        [JsonIgnore]
        private int _Economia = 0;
        [JsonIgnore]
        private int _Fede = 0;
        [JsonIgnore]
        private int _Parlamento = 0;

        [JsonProperty("consenso")]
        public int Consenso { get => _Consenso; set { _Consenso = value; Notify(); } } 

        [JsonProperty("economia")]
        public int Economia { get => _Economia; set { _Economia = value; Notify(); } }

        [JsonProperty("fede")]
        public int Fede { get => _Fede; set { _Fede = value; Notify(); } }

        [JsonProperty("parlamento")]
        public int Parlamento { get => _Parlamento; set { _Parlamento = value; Notify(); } }
    }
}