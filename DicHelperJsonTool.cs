using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolDemocraci.ViewModel;
using Helper.Dictionary;


namespace ToolDemocraci
{
    public class DicHelperJsonTool<T> : DictionaryHelper<T>
    {
        private MainViewModel MainVm;
        public override int Index
        {
            get => _index;
            set
            {
                if (value >= 0)
                {
                    if (value <= Keys.Count)
                        _index = value;
                    else if (MainVm.LastEventIsValid && value == Keys.Count)
                        MainVm.NewEvent();
                    else
                        _index = 0;
                }
                else
                    _index = Keys.Count - 1;
            }
        }
        public DicHelperJsonTool(IList<T> _Keys, MainViewModel _mainVm) : base(_Keys)
        {
            MainVm = _mainVm;
        }


    }



}
