using System.Windows.Input;

namespace EShopNative.CustomViews
{
    public partial class CustomButton : ContentView
    {
        private readonly CustomButtonDrawable _drawable;

        public static readonly BindableProperty CustomBackgroundColorProperty =
            BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(CustomButton), Colors.Yellow, propertyChanged: OnVisualPropertyChanged);

        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(CustomButton), Colors.Black, propertyChanged: OnVisualPropertyChanged);

        public static readonly BindableProperty BorderThicknessProperty =
            BindableProperty.Create(nameof(BorderThickness), typeof(float), typeof(CustomButton), 2f, propertyChanged: OnVisualPropertyChanged);

        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(nameof(CornerRadius), typeof(float), typeof(CustomButton), 8f, propertyChanged: OnVisualPropertyChanged);

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(CustomButton), "Button", propertyChanged: OnVisualPropertyChanged);

        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(CustomButton), Colors.Black, propertyChanged: OnVisualPropertyChanged);

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(CustomButton), null);

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(CustomButton), null);


        public new Color BackgroundColor
        {
            get => (Color)GetValue(CustomBackgroundColorProperty);
            set => SetValue(CustomBackgroundColorProperty, value);
        }

        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }


        public float BorderThickness
        {
            get => (float)GetValue(BorderThicknessProperty);
            set => SetValue(BorderThicknessProperty, value);
        }

        public float CornerRadius
        {
            get => (float)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public event EventHandler? Clicked;

        public CustomButton()
        {
            _drawable = new CustomButtonDrawable();

            var graphicsView = new GraphicsView
            {
                Drawable = _drawable
            };

            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += (s, e) =>
            {
                Clicked?.Invoke(this, EventArgs.Empty);

                if (Command?.CanExecute(CommandParameter) == true)
                    Command.Execute(CommandParameter);
            };
            graphicsView.GestureRecognizers.Add(tapGesture);

            Content = graphicsView;
        }

        private static void OnVisualPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CustomButton button)
            {
                button._drawable.BackgroundColor = button.BackgroundColor;
                button._drawable.BorderColor = button.BorderColor;
                button._drawable.BorderThickness = button.BorderThickness;
                button._drawable.CornerRadius = button.CornerRadius;
                button._drawable.Text = button.Text;
                button._drawable.TextColor = button.TextColor;

                if (button.Content is GraphicsView gv)
                    gv.Invalidate();
            }
        }
    }

    public class CustomButtonDrawable : IDrawable
    {
        public Color BackgroundColor { get; set; } = Colors.Yellow;
        public Color BorderColor { get; set; } = Colors.Black;
        public float CornerRadius { get; set; } = 8f;
        public float BorderThickness { get; set; } = 2f;
        public string Text { get; set; } = "Button";
        public Color TextColor { get; set; } = Colors.Black;

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            var rect = new RectF(dirtyRect.X, dirtyRect.Y, dirtyRect.Width, dirtyRect.Height);

            // Draw background
            canvas.FillColor = BackgroundColor;
            canvas.FillRoundedRectangle(rect, CornerRadius);

            // Draw border
            canvas.StrokeColor = BorderColor;
            canvas.StrokeSize = BorderThickness;
            canvas.DrawRoundedRectangle(rect, CornerRadius);

            // Draw text
            canvas.FontColor = TextColor;
            canvas.FontSize = 16;
            canvas.DrawString(Text, rect, HorizontalAlignment.Center, VerticalAlignment.Center);
        }
    }
}
