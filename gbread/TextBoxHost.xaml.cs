namespace GBRead
{
    using System.Reflection;
    using System.Windows.Controls;
    using System.Xml;
    using ICSharpCode.AvalonEdit.Highlighting;
    using ICSharpCode.AvalonEdit.Highlighting.Xshd;

    /// <summary>
    /// Interaction logic for TextBoxHost.xaml
    /// </summary>
    public partial class TextBoxHost : UserControl
    {
        public TextBoxHost()
        {
            InitializeComponent();
            using (XmlTextReader xts = new XmlTextReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("GBRead.GB.xshd")))
            {
                mainTextBox.SyntaxHighlighting = HighlightingLoader.Load(xts, HighlightingManager.Instance);
            }
            mainTextBox.Options.ConvertTabsToSpaces = true;
            mainTextBox.Options.EnableTextDragDrop = true;
        }
    }
}