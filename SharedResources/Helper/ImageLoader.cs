using System.Windows;
using System.Windows.Media;

namespace SharedResources.Helper
{
    public static class ImageLoader
    {
        public static ImageSource GetDefaultImage(DependencyObject obj)
        {
            return (ImageSource)obj.GetValue(DefaultImageProperty);
        }
        public static void SetDefaultImage(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(DefaultImageProperty, value);
        }

        public static readonly DependencyProperty DefaultImageProperty =
            DependencyProperty.RegisterAttached(
            "DefaultImage",
            typeof(ImageSource),
            typeof(ImageLoader),
            new UIPropertyMetadata(null));

        public static ImageSource GetHoverImage(DependencyObject obj)
        {
            return (ImageSource)obj.GetValue(HoverImageProperty);
        }
        public static void SetHoverImage(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(HoverImageProperty, value);
        }

        public static readonly DependencyProperty HoverImageProperty =
            DependencyProperty.RegisterAttached(
            "HoverImage",
            typeof(ImageSource),
            typeof(ImageLoader),
            new UIPropertyMetadata(null));

        public static ImageSource GetClickedImage(DependencyObject obj)
        {
            return (ImageSource)obj.GetValue(ClickedImageProperty);
        }
        public static void SetClickedImage(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(ClickedImageProperty, value);
        }

        public static readonly DependencyProperty ClickedImageProperty =
            DependencyProperty.RegisterAttached(
            "ClickedImage",
            typeof(ImageSource),
            typeof(ImageLoader),
            new UIPropertyMetadata(null));
    }
}
