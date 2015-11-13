using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace Xaml.Blend.Tutorial.WP8.Custom
{
    

    public class MessageDialogAction : TriggerAction<DependencyObject>
    {
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(MessageDialogAction), new PropertyMetadata(""));

        public MessageDialogAction()
        {
            // Insert code required for object creation below this point.
        }

        protected override void Invoke(object o)
        {
            MessageBox.Show(Message);
        }
    }

}
