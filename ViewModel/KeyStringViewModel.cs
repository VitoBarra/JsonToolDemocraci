using MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ToolDemocraci.ViewModel
{
    public class KeyStringViewModel :BaseViewModel
    {
        private int _key ;
        private string _NameValue = "";

        public int Key { get => _key; set { _key = value; Notify(); } }
        public string NameValue { get => _NameValue; set { _NameValue = value; Notify(); } }

    }
}
