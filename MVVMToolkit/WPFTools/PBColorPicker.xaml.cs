using System;
using System.Windows;
using System.Windows.Media;
using ColorPicker;
using Color = System.Windows.Media.Color;

namespace PB.MVVMToolkit.WPFTools
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class PBColorPicker : Window
    {
        private Color _originalColor;

        /// <summary>
        /// Set to determine whether the Alpha channel should be editable
        /// </summary>
        public bool ShowAlpha { get; set; }

        /// <summary>
        /// Gets or sets the Hex value of the color
        /// </summary>
        public string HexValue => ConvertColorToHex(Picker.SelectedColor);

        /// <summary>
        /// Returns the Color value from the picker
        /// </summary>
        public Color Color => Picker.SelectedColor;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public PBColorPicker()
        {
            _= new StandardColorPicker();
            _originalColor = Color.FromRgb(0, 0, 0);
            InitializeComponent();
        }

        /// <summary>
        /// Constructor that sets the selected color per the hex value
        /// </summary>
        /// <param name="hex"></param>
        public PBColorPicker(string hex)
        {
            InitializeComponent();
            _originalColor = ConvertHexToColor(hex);
            Picker.SelectedColor = _originalColor;
        }

        /// <summary>
        /// Constructor that sets the selected color per a Color value
        /// </summary>
        /// <param name="color"></param>
        public PBColorPicker(Color color)
        {
            InitializeComponent();
            _originalColor = color;
            Picker.SelectedColor = color;
        }


        private Color ConvertHexToColor(string hex)
        {
            try
            {
                if (hex == null)
                    return Color.FromRgb(0, 0, 0);

                var color = ColorConverter.ConvertFromString(hex);
                if (color == null)
                    return Color.FromRgb(0, 0, 0);

                return (Color)color;
            }
            catch
            {
                return Color.FromRgb(0, 0, 0);
            }
        }

        private string ConvertColorToHex(Color color)
        {
            string hex;
            if (ShowAlpha)
                hex = color.ToString();
            else
                hex = string.Format("#{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);

            return hex;
        }


        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Picker.SelectedColor = _originalColor;
            this.Close();
        }
    }
}
