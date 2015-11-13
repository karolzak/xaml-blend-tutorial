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
    public abstract class MainPageViewModelBase : BindableBase
    {

        bool? _IsLoggedIn = default(bool);
        public bool? IsLoggedIn { get { return _IsLoggedIn; } set { SetProperty(ref _IsLoggedIn, value); } }


        bool _IsPushRegistered = default(bool);
        public bool IsPushRegistered { get { return _IsPushRegistered; } set { SetProperty(ref _IsPushRegistered, value); } }
        public MainPageViewModelBase()
        {
           
        }

        public abstract void LogoutAsync();
        public abstract void LoginAsync();
        public abstract void BroadcasterAsync();
        public abstract void SingleUserPushAsync();
        public abstract void PushRegisterAsync();



        RelayCommand<object> _PushRegisterCommand = null;
            public RelayCommand<object> PushRegisterCommand
        {
            get
            {
                if (_PushRegisterCommand != null)
                    return _PushRegisterCommand;
                _PushRegisterCommand = new RelayCommand<object>
                (
                    o => { PushRegisterAsync(); },
                    o => true
                );
                this.PropertyChanged += (s, e) => _PushRegisterCommand.RaiseCanExecuteChanged();
                return _PushRegisterCommand;
            }
        }

        RelayCommand<object> _LoginCommand = null;
        public RelayCommand<object> LoginCommand
        {
            get
            {
                if (_LoginCommand != null)
                    return _LoginCommand;
                _LoginCommand = new RelayCommand<object>
                (
                    o => { LoginAsync(); },
                    o => true
                );
                this.PropertyChanged += (s, e) => _LoginCommand.RaiseCanExecuteChanged();
                return _LoginCommand;
            }
        }
        RelayCommand<object> _BroadcasterCommand = null;
        public RelayCommand<object> BroadcasterCommand
        {
            get
            {
                if (_BroadcasterCommand != null)
                    return _BroadcasterCommand;
                _BroadcasterCommand = new RelayCommand<object>
                (
                    o => { BroadcasterAsync(); },
                    o => true
                );
                this.PropertyChanged += (s, e) => _BroadcasterCommand.RaiseCanExecuteChanged();
                return _BroadcasterCommand;
            }
        }
        RelayCommand<object> _SingleUserPushCommand = null;
        public RelayCommand<object> SingleUserPushCommand
        {
            get
            {
                if (_SingleUserPushCommand != null)
                    return _SingleUserPushCommand;
                _SingleUserPushCommand = new RelayCommand<object>
                (
                    o => { SingleUserPushAsync(); },
                    o => true
                );
                this.PropertyChanged += (s, e) => _SingleUserPushCommand.RaiseCanExecuteChanged();
                return _SingleUserPushCommand;
            }
        }
        RelayCommand<object> _LogoutCommand = null;
        public RelayCommand<object> LogoutCommand
        {
            get
            {
                if (_LogoutCommand != null)
                    return _LogoutCommand;
                _LogoutCommand = new RelayCommand<object>
                (
                    o => { LogoutAsync(); },
                    o => true
                );
                this.PropertyChanged += (s, e) => _LogoutCommand.RaiseCanExecuteChanged();
                return _LogoutCommand;
            }
        }    
    }
}
