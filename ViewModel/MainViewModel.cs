using Caliburn.Micro;
using Microsoft.Win32;
using MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Controls;
using IO.JsonFile;
using Helper.Dictionary;
using static ToolDemocraci.ViewModel.EventObjectViewModel;

namespace ToolDemocraci.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private EventObjectViewModel EventObject = new EventObjectViewModel();


        private IList<PremierViewModel> _premier = new ObservableCollection<PremierViewModel>();
        private EventViewModel _currentEvent = new EventViewModel();
        private IList<EventViewModel> _currentEventList = new ObservableCollection<EventViewModel>();
        private IList<string> _premierNameList = new ObservableCollection<string>();
        private IList<string> _partyNameList = new ObservableCollection<string>();



        private int _index = -1;
        private string _indexName = "-1";

        ListType _listType = ListType.Error;

        private string _fileNameEvent = "PlaceHolderEve";
        private string _fileNamePremier = "PlaceHolderPr";

        private DicHelperJsonTool<int> dicHelper;
        private string _ListName = "";
        //combobox
        private int? _comboBoxSelectedItem;
        private IList<int?> _TimeCollectionItem = new BindableCollection<int?>(new int?[] {null, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 });
        private bool _timeComboBoxInEnable = true;
        //combobox

        public bool LastEventIsValid => CurrentEventList.Count>0?CurrentEventList[CurrentEventList.Count - 1].AllFieldAreValid():false;

        #region DataBinding

        public IList<int?> TimeCollection { get => _TimeCollectionItem;set { _TimeCollectionItem = value; Notify(); } }
        public int? ComboBoxSelectedItem 
        { get => _comboBoxSelectedItem; 
            set { if(_TimeCollectionItem.Contains(value)) _comboBoxSelectedItem = value; Notify(); SelectionChanged(); } }

        public bool TimeComboBoxInEnable { get => _timeComboBoxInEnable; set { _timeComboBoxInEnable = value; Notify(); } }
        public IList<EventViewModel> CurrentEventList
        {
            get => _currentEventList; set
            {
                _currentEventList = value; Notify();
                if (_listType != ListType.Cons)
                    Index = 0;
                else
                    dicHelper.Index = 0;
            }
        }
        public IList<PremierViewModel> Premier { get => _premier; set { _premier = value; Notify(); } }
        public IList<string> PremierNameList { get => _premierNameList; private set { _premierNameList = value; } }
        public IList<string> PartyNameList { get => _partyNameList; private set { _partyNameList = value; } }
        public EventViewModel CurrentEvent { get => _currentEvent; set { _currentEvent = value; Notify(); RightElementList.RaiseCanExecuteChanged(); } }

        public ListType ListTypeIndex
        {
            get => _listType;
            set
            {
                //var EnumNum = Enum.GetNames(typeof(ListType)).Length;
                if ((int)value >= 0)
                {
                    if (value != (ListType)4)
                        _listType = value;
                    else
                        _listType = ListType.InstantList;
                }
                else
                    _listType = ListType.Cons;

                ListName = _listType.ToString();
                Notify();
            }
        }
        public int Index
        {
            get => _index;
            set
            {
                if (value >= 0)
                {
                    if (value < CurrentEventList.Count)
                        _index = value;
                    else if (LastEventIsValid && value == CurrentEventList.Count)
                    {
                        NewEvent();
                        _index = value;
                    }
                    else
                        _index = 0;
                }
                else
                    _index = CurrentEventList.Count - 1;

                Notify();
            }
        }

        public string IndexName
        {
            get => _indexName;
            set
            {
                switch (ListTypeIndex)
                {
                    case ListType.InstantList:
                    case ListType.ShortList:
                    case ListType.LongList:
                        Index = int.Parse(value);
                        _indexName = Index.ToString();
                        dicHelper.DefaultErrorIndex();
                        break;
                    case ListType.Cons:
                        Index = -1;
                        dicHelper.CurrentKey = int.Parse(value);
                        _indexName = dicHelper.CurrentKey.ToString();
                        break;
                    case ListType.Error:
                    default:
                        Index = -1;
                        dicHelper.DefaultErrorIndex();
                        _indexName = "-1";
                        break;
                }
                UpdateViewWithCurentEvent();
                Notify();
            }
        }

        public string ListName { get => _ListName; set { _ListName = value; Notify(); } }
        public string FileNameEvent { get => _fileNameEvent; set { _fileNameEvent = value; Notify(); } }
        public string FileNamePremier { get => _fileNamePremier; set { _fileNamePremier = value; Notify(); } }


        //Visual Binding

        //public bool ComboBoxAnswereEnable

        #endregion DataBinding

        #region Command

        public RelayCommand NewFile { get; private set; }
        public RelayCommand LoadFile { get; private set; }
        public RelayCommand CloseFile { get; private set; }
        public RelayCommand LeftElementList { get; private set; }
        public RelayCommand RightElementList { get; private set; }
        public RelayCommand LeftList { get; private set; }
        public RelayCommand RightList { get; private set; }
        public RelayCommand WriteFile { get; private set; }
        public RelayCommand CreateNewEvent { get; private set; }

        #endregion Command

        public MainViewModel()
        {
            
            CloseFile = new RelayCommand((par) =>
            {
                MessageBoxResult result = MessageBox.Show("vuoi salvare prima di uscre", "", MessageBoxButton.YesNoCancel);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        WriteFileF();
                        EventObject = null;
                        break;

                    case MessageBoxResult.No:
                        EventObject = null;
                        break;

                    case MessageBoxResult.Cancel:
                    default:
                        break;
                }

                Global.FileOpned = false;
            });
            LoadFile = new RelayCommand(par =>
             {
                 if (Global.FileOpned)
                     CloseFile?.Execute(this);

                 OpenFileDialog openFileDialog = new OpenFileDialog();
                 if (openFileDialog.ShowDialog() == true)
                 {
                     Global.FileOpned = true;
                     //codice no buono
                     PremierRes.ResetCheckbox();
                     PartyRes.ResetCheckbox();
                     //fine codice no buono 
                     FileNameEvent = openFileDialog.FileName;
                     EventObject = UtilityFileReader.ReadJsonFromFile<EventObjectViewModel>(FileNameEvent);
                     dicHelper = new DicHelperJsonTool<int>(new List<int>(EventObject.ConsequencesList.Keys), this);
                     dicHelper.SetGenerationRule_intDefault();

                     if (openFileDialog.ShowDialog() == true)
                     {
                         FileNamePremier = openFileDialog.FileName;
                         Premier = UtilityFileReader.ReadJsonFromFile<PremierViewModel[]>(FileNamePremier);
                         PremierNameList = Premier.PremierName();
                         PartyNameList = Premier.PartyName();

                         //altra merda
                         PremierRes.AddCheckbox(PremierNameList);
                         PartyRes.AddCheckbox(PartyNameList);
                         //fine
                         SetNewList(EventObject.GetList(ListType.InstantList), ListType.InstantList);
                         UpdateCheckValue();
                         ComboBoxSelectedItem = CurrentEvent.Time;
                     }
                     else
                     {
                         EventObject = new EventObjectViewModel();
                         MessageBox.Show("Seleziona un Premer.json");
                     }
                 }
             });

            //ElementMove
            LeftElementList = new RelayCommand(par =>
            { /*da togliere*/
                SaveCurrentCheckList();
                if (ListTypeIndex != ListType.Cons)
                    Index--;
                else
                    dicHelper.Index--;


                SetNewCurrentEvent();

            });
            RightElementList = new RelayCommand(
                action:
                par =>
                { /*da togliere*/
                    SaveCurrentCheckList();
                    if (ListTypeIndex != ListType.Cons)
                        Index++;
                    else
                        dicHelper.Index++;
                    SetNewCurrentEvent();
                },
                predicate: par => CurrentEvent.AllFieldAreValid()
                );
            //List Move
            LeftList = new RelayCommand(par =>SetNewList(PreviusList));
            RightList = new RelayCommand(par => SetNewList(NextList));


            WriteFile = new RelayCommand(par => WriteFileF());

        }

        public IList<EventViewModel> NextList
        {
            get
            {
                ListTypeIndex++;
                return EventObject.GetList(ListTypeIndex);
            }
        }

        public IList<EventViewModel> PreviusList
        {
            get
            {
                ListTypeIndex--;
                return EventObject.GetList(ListTypeIndex);
            }
        }

        private void SetNewList(IList<EventViewModel> events, ListType listType = ListType.Error)
        {
            if (listType != ListType.Error)
                ListTypeIndex = listType;

            CurrentEventList = events;

            SetNewCurrentEvent();
        }

        private void SetIndexName()
        {
            switch (ListTypeIndex)
            {
                case ListType.InstantList:
                case ListType.ShortList:
                case ListType.LongList:
                    IndexName = Index.ToString();
                    break;
                case ListType.Cons:
                    IndexName = dicHelper.CurrentKey.ToString();
                    break;
                case ListType.Error:
                default:
                    _indexName = "Error";
                    break;
            }
        }

        private void SetNewCurrentEvent()
        {
            SetIndexName();

            UpdateViewWithCurentEvent();
        }

        private void UpdateViewWithCurentEvent()
        {

            if (CurrentEventList.Count == 0) return;



            switch (ListTypeIndex)
            {
                case ListType.InstantList:
                    CurrentEvent = CurrentEventList[Index];

                    TimeComboBoxInEnable = false;
                    ComboBoxSelectedItem = CurrentEvent.Time;


                    Answere2.ConseguenzaFi.IsEnabled = false;
                    Answere1.ConseguenzaFi.IsEnabled = false;
                    break;
                case ListType.ShortList:
                    CurrentEvent = CurrentEventList[Index];
                    CurrentEvent.Answere1.ComboBoxValue = EventObject.GetDicName();
                    CurrentEvent.Answere2.ComboBoxValue = EventObject.GetDicName();

                    TimeComboBoxInEnable = false;
                    ComboBoxSelectedItem = CurrentEvent.Time;


                    Answere2.ConseguenzaFi.IsEnabled = true;
                    Answere1.ConseguenzaFi.IsEnabled = true;
                    break;
                case ListType.LongList:
                    CurrentEvent = CurrentEventList[Index];
                    CurrentEvent.Answere1.ComboBoxValue = EventObject.GetDicName();
                    CurrentEvent.Answere2.ComboBoxValue = EventObject.GetDicName();


                    TimeComboBoxInEnable = true;
                    ComboBoxSelectedItem = CurrentEvent.Time;

                    Answere2.ConseguenzaFi.IsEnabled = true;
                    Answere1.ConseguenzaFi.IsEnabled = true;
                    break;
                case ListType.Cons:
                    CurrentEvent = EventObject.ConsequencesList[dicHelper.CurrentKey];


                    TimeComboBoxInEnable = false;
                    ComboBoxSelectedItem = null;

                    Answere2.ConseguenzaFi.IsEnabled = true;
                    Answere1.ConseguenzaFi.IsEnabled = true;

                    break;
                case ListType.Error:
                default:
                    MessageBox.Show("Cercato di settare male CurrentEvent");
                    break;
            }

            CurrentEvent.e = RightElementList;
            CurrentEvent.Answere1.e = RightElementList;
            CurrentEvent.Answere2.e = RightElementList;


            //codice da togliere
            UpdateCheckValue();
        }



        public void NewEvent()
        {

            CurrentEvent = new EventViewModel();

            switch (ListTypeIndex)
            {
                case ListType.InstantList:
                    ComboBoxSelectedItem = 1;
                    break;
                case ListType.ShortList:
                    ComboBoxSelectedItem = 2;
                    break;
                case ListType.LongList:
                    ComboBoxSelectedItem = 3;
                    break;
                case ListType.Cons:
                case ListType.Error:
                default:
                    ComboBoxSelectedItem = null;
                    break;
            }

            if (ListTypeIndex != ListType.Cons)
                CurrentEventList.Add(CurrentEvent);
            else
                EventObject.ConsequencesList.Add(dicHelper.GenerateNextKey(), CurrentEvent);
        }

        private void WriteFileF()
        {
            SaveCurrentCheckList();
            UtilityFileReader.WriterJsonToFile(_fileNameEvent, EventObject);
        }



        ///TODO:codice da elminare ti prego

        private void SelectionChanged()
        {
            CurrentEvent.Time = ComboBoxSelectedItem;
        }

        private void SaveCurrentCheckList()
        {
            CurrentEvent.RestrictPr = PremierRes.WhiteListToString;
            CurrentEvent.ExceptPr = PremierRes.BlackListToString;
            CurrentEvent.RestrictParty = PartyRes.WhiteListToString;
            CurrentEvent.ExceptParty = PartyRes.BlackListToString;
        }
        private void UpdateCheckValue()
        {
            PremierRes.ClearChackBoxValue();
            PartyRes.ClearChackBoxValue();
            PremierRes.UpdateCheckBlack(CurrentEvent.ExceptPr);
            PartyRes.UpdateCheckBlack(CurrentEvent.ExceptParty);
            PartyRes.UpdateCheckWhite(CurrentEvent.RestrictParty);
            PremierRes.UpdateCheckWhite(CurrentEvent.RestrictPr);
        }





        UC.Restriction PremierRes;
        UC.Restriction PartyRes;
        UC.Answere Answere1;
        UC.Answere Answere2;
        public void GetRestrict(UC.Restriction pr, UC.Restriction par, UC.Answere _answere1, UC.Answere _answere2)
        {
            PremierRes = pr;
            PartyRes = par;


            Answere1 = _answere1;
            Answere2 = _answere2;
        }

    }
}