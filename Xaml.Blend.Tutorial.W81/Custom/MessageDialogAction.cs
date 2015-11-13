using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace Xaml.Blend.Tutorial.W81.Custom
{
    class MessageDialogAction : DependencyObject, IAction
    {
        public string Message { get; set; }
        public string Message1 { get; set; }
        public string Message2 { get; set; }
        public object Execute(object sender, object parameter)
        {
            new MessageDialog(this.Message).ShowAsync();
            return null;
        }
    }
}
