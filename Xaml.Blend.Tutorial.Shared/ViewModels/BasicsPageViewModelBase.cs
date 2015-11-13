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
    public abstract class BasicsPageViewModelBase : BindableBase
    {
        public BasicsPageViewModelBase()
        {
            if (DesignTimeInit())
                return;
            //wykonaj pozostałe zadania
            RealTimeInit();
        }

        public abstract bool DesignTimeInit();
        public abstract bool RealTimeInit();

        #region Properties



        #endregion

                
    }
}
