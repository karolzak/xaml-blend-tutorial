using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Xaml.Blend.Tutorial.Shared.Microsoft;
using Xaml.Blend.Tutorial.Shared.Models;

namespace Xaml.Blend.Tutorial.WP8.Models
{
    public class PersonModel:PersonModelBase
    {


        Color _Color = default(Color);
        public Color Color { get { return _Color; } set { SetProperty(ref _Color, value); } }

    }
}
