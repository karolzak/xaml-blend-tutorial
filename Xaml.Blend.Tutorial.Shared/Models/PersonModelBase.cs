using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xaml.Blend.Tutorial.Shared.Microsoft;

namespace Xaml.Blend.Tutorial.Shared.Models
{
    public class PersonModelBase:BindableBase
    {
        string _Name = default(string);
        public string Name { get { return _Name; } set { SetProperty(ref _Name, value); } }

        bool _IsMale = default(bool);
        public bool IsMale { get { return _IsMale; } set { SetProperty(ref _IsMale, value); } }



    }
}
