using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Xaml.Blend.Tutorial.Shared.Microsoft;
using Xaml.Blend.Tutorial.Shared.Models;
using Xaml.Blend.Tutorial.Shared.ViewModels;
using Xaml.Blend.Tutorial.W81.Models;

namespace Xaml.Blend.Tutorial.W81.ViewModels
{
    public class BasicsPageViewModel : BasicsPageViewModelBase
    {

        public override bool DesignTimeInit()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                this._PersonList = new ObservableCollection<PersonModel>();
                this._PersonList.Add(
                    new PersonModel()
                    {
                        Name = "Karol",
                        IsMale = true,
                        Color = Colors.Blue
                    }
                );
                this._PersonList.Add(
                    new PersonModel()
                    {
                        Name = "Adam",
                        IsMale = true,
                        Color = Colors.Green
                    }
                );
                this._PersonList.Add(
                    new PersonModel()
                    {
                        Name = "Maria",
                        IsMale = false,
                        Color = Colors.Cyan
                    }
                );
                this._PersonList.Add(
                    new PersonModel()
                    {
                        Name = "Ania",
                        IsMale = false,
                        Color = Colors.Red
                    }
                );
                return true;  // design only
            }
            return false;
        }

        public override bool RealTimeInit()
        {
            //zaciąganie danych z serwisu
            this._PersonList = new ObservableCollection<PersonModel>();
            this._PersonList.Add(
                new PersonModel()
                {
                    Name = "Dane z serwisu",
                    IsMale = true,
                    Color = Colors.Blue
                }
            );
            this._PersonList.Add(
                new PersonModel()
                {
                    Name = "Dane z serwisu",
                    IsMale = true,
                    Color = Colors.Green
                }
            );
            this._PersonList.Add(
                new PersonModel()
                {
                    Name = "Dane z serwisu",
                    IsMale = false,
                    Color = Colors.Cyan
                }
            );
            this._PersonList.Add(
                new PersonModel()
                {
                    Name = "Dane z serwisu",
                    IsMale = false,
                    Color = Colors.Red
                }
            );

            return true;
        }


        ObservableCollection<PersonModel> _PersonList = new ObservableCollection<PersonModel>();
        public ObservableCollection<PersonModel> PersonList { get { return _PersonList; } }

        PersonModel _SelectedPerson = default(PersonModel);
        public PersonModel SelectedPerson { get { return _SelectedPerson; } set { SetProperty(ref _SelectedPerson, value); } }
                
    }
}
