using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xaml.Blend.Tutorial.Shared.Microsoft;
using Xaml.Blend.Tutorial.Shared.Models;

namespace Xaml.Blend.Tutorial.Shared.ViewModels
{
    public abstract class BehaviorsPageViewModelBase : BindableBase
    {
        public BehaviorsPageViewModelBase()
        {
        }


        #region Properties


        string _LoadDataText = default(string);
        public string LoadDataText { get { return _LoadDataText; } set { SetProperty(ref _LoadDataText, value); } }

        string _ReloadDataText = default(string);
        public string ReloadDataText { get { return _ReloadDataText; } set { SetProperty(ref _ReloadDataText, value); } }

        #endregion

        #region Commands

        RelayCommand<object> _LoadDataCommand = null;
        public RelayCommand<object> LoadDataCommand
        {
            get
            {
                if (_LoadDataCommand != null)
                    return _LoadDataCommand;
                _LoadDataCommand = new RelayCommand<object>
                (
                    o => { LoadDataText = string.Format("Data Loaded {0}", DateTime.Now); },
                    o => true
                );
                this.PropertyChanged += (s, e) => _LoadDataCommand.RaiseCanExecuteChanged();
                return _LoadDataCommand;
            }
        }
        #endregion

        #region Methods
        public void ReloadData() 
        { 
            ReloadDataText = string.Format("Data Reloaded {0}", DateTime.Now); 
        }

        #endregion
                

    }
}
