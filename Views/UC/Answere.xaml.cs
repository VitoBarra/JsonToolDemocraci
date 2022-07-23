using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using ToolDemocraci.ViewModel;

namespace ToolDemocraci.UC
{
    /// <summary>
    /// Logica di interazione per Answere.xaml
    /// </summary>
    public partial class Answere : UserControl
    {
        public AnswereViewModel vm;


        public Answere()
        {
            InitializeComponent();
        }

        public Answere(AnswereViewModel _vm)
        {
            InitializeComponent();
            vm = _vm;
            DataContext = _vm;
        }


        public void AddItemComboBox(IList<(int a,string s)> srt)
        {
            foreach((int a, string s) e in srt)
              ConseguenzaFi.Items.Add($"{e.a}: {e.s}");
        }
   

        private new void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^(-|\+)?[0-9]{1,3}$");
            e.Handled = !regex.IsMatch(e.Text);
        }
    }
}