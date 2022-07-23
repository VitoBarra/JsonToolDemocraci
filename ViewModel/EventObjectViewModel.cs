using Caliburn.Micro;
using MVVM.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ToolDemocraci.ViewModel
{
    public class EventObjectViewModel : BaseViewModel
    {
        public enum ListType { InstantList, ShortList, LongList,Cons,Error }
        [JsonIgnore]
        private IList<EventViewModel> _InstantList { get; set; } = new List<EventViewModel>();
        [JsonIgnore]
        private IList<EventViewModel> _ShortList { get; set; } = new List<EventViewModel>();
        [JsonIgnore]
        private IList<EventViewModel> _LongList { get; set; } = new List<EventViewModel>();
        [JsonIgnore]
        private Dictionary<int, EventViewModel> _ConsequencesList { get; set; } = new Dictionary<int, EventViewModel>();




        [JsonProperty("instant")]
        public IList<EventViewModel> InstantList { get => _InstantList; set { _InstantList = value; Notify(); } }

        [JsonProperty("short")]
        public IList<EventViewModel> ShortList { get => _ShortList; set { _ShortList = value; Notify(); } }

        [JsonProperty("long")]
        public IList<EventViewModel> LongList { get => _LongList; set { _LongList = value; Notify(); } }

        [JsonProperty("consequences")]
        public Dictionary<int, EventViewModel> ConsequencesList { get => _ConsequencesList; set { _ConsequencesList = value; Notify(); } }


        public IList<EventViewModel>  GetList(ListType ind)
        {

            switch (ind)
            {
                case ListType.InstantList:
                    return  InstantList;
                case ListType.ShortList:
                    return  ShortList;
                case ListType.LongList:
                    return  LongList;
                case ListType.Cons:
                    return ExtractList();

                default:
                    return  null;
            }



        }

        private IList<EventViewModel>  ExtractList()
        {
            IList<EventViewModel> ris = new List<EventViewModel>();
            foreach (var e in ConsequencesList)
            {
                ris.Add(e.Value);
            }
            return ris;
        }




        public IList<KeyStringViewModel> GetDicName()
        {
            var ris = new BindableCollection<KeyStringViewModel>();
            foreach (var e in ConsequencesList)
                ris.Add(new KeyStringViewModel() {Key= e.Key, NameValue= e.Value.Name });

            return ris;
        }
    }
}