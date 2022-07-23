using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ToolDemocraci.UC
{
    /// <summary>
    /// Logica di interazione per Restriction.xaml
    /// </summary>
    public partial class Restriction : UserControl
    {
        public Restriction()
        {
            InitializeComponent();
        }



        public void ResetCheckbox()
        {
            WhiteList.Children.Clear();
            BlackList.Children.Clear();
        }

        public void AddCheckbox(string s)
        {
            WhiteList.Children.Add(new CheckBox() { Content = s, Margin = new Thickness(0, 0, 5, 0) });
            BlackList.Children.Add(new CheckBox() { Content = s, Margin = new Thickness(0, 0, 5, 0) });
        }
        public void AddCheckbox(IList<string> s)
        {
            foreach (string e in s)
            {
                CheckBox whietListElement = new CheckBox() { Content = e, Margin = new Thickness(0, 5, 5, 0) };
                whietListElement.Checked += CheckedWhite;

                CheckBox BlackListElement = new CheckBox() { Content = e, Margin = new Thickness(0, 5, 5, 0) };
                BlackListElement.Checked += CheckedBlack;

                WhiteList.Children.Add(whietListElement);
                BlackList.Children.Add(BlackListElement);
            }
        }




        public void UpdateCheckWhite(IList<string> s) => UpdateCheck(s, WhiteList.Children);
        public void UpdateCheckBlack(IList<string> s) => UpdateCheck(s, BlackList.Children);

        private void UpdateCheck(IList<string> s, UIElementCollection CheckBoxList)
        {
            foreach (string e in s)
                foreach (CheckBox item in CheckBoxList)
                    if ((string)item.Content == e)
                        item.IsChecked = true;
        }

        private void CheckedWhite(object sender, RoutedEventArgs e) => MutexCheck((CheckBox)sender, BlackList.Children);

        private void CheckedBlack(object sender, RoutedEventArgs e) => MutexCheck((CheckBox)sender, WhiteList.Children);

        private void MutexCheck(CheckBox Sender, UIElementCollection CheckBoxList)
        {
            foreach (CheckBox e in CheckBoxList)
                if (e.Content == Sender.Content && (bool)e.IsChecked)
                    e.IsChecked = false;
        }




        public IList<string> WhiteListToString => CheckBoxListToString(WhiteList.Children);
        public IList<string> BlackListToString => CheckBoxListToString(BlackList.Children);
        private IList<string> CheckBoxListToString(UIElementCollection CheckBoxList)
        {
            IList<string> s = new List<string>();
            foreach (CheckBox e in CheckBoxList)
                if ((bool)e.IsChecked)
                    s.Add((string)e.Content);
            return s;
        }

        public void ClearChackBoxValue()
        {
            ClearValue(WhiteList.Children);
            ClearValue(BlackList.Children);
        }


        private void ClearValue(UIElementCollection CheckBoxList)
        {
            foreach (CheckBox e in CheckBoxList)
                e.IsChecked = false;
        }

    }
}