using System;
using Android.Content;
using asthanarht.calculator.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Button), typeof(DefaultButtonRenderer))]
namespace asthanarht.calculator.Droid.Renderer
{
    public class DefaultButtonRenderer:ButtonRenderer
    {
        public DefaultButtonRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            // Control is Android.Widget.Button, Element is Xamarin.Forms.Button
            if (Control != null && Element != null)
            {
                // remove default background image
                Control.Background = null;

                // set background color
                Control.SetBackgroundColor(Element.BackgroundColor.ToAndroid());
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == "BackgroundColor")
            {
                // You have to set background color here again, because the background color can be changed later.
                Control.SetBackgroundColor(Element.BackgroundColor.ToAndroid());
            }
        }
    }
}
