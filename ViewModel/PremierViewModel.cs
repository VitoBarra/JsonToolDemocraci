using MVVM.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ToolDemocraci.ViewModel
{
    public class PremierViewModel : BaseViewModel
    {
        [JsonIgnore]
        private string _nome ="";
        [JsonIgnore]
        private string _partito ="";
        [JsonIgnore]
        private StatsTypeViewModel _perk = new StatsTypeViewModel();

        [JsonProperty("nome")]
        public string Nome { get => _nome; set { _nome = value; Notify(); }}
        [JsonProperty("partito")]
        public string Partito { get => _partito; set { _partito = value; Notify(); } }
        [JsonProperty("perk")]
        public StatsTypeViewModel Perk { get => _perk; set { _perk = value; Notify(); } }



    }



    public static class PremierViewModelEx
    {
        public static IList<string>PremierName(this IList<PremierViewModel> Premier)
        {
            IList<string> a = new ObservableCollection<string>();
            foreach(PremierViewModel e in Premier)
                a.Add(e.Nome);

            return a;

        }
        public static IList<string> PartyName(this IList<PremierViewModel> Premier)
        {
            IList<string> a = new ObservableCollection<string>();
            foreach (PremierViewModel e in Premier)
                a.Add(e.Partito);

            return a;
        }
    }
}
