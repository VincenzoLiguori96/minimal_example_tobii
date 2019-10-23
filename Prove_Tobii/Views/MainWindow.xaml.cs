using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tobii.Interaction;
using Tobii.EyeX.Framework;
using Tobii.Interaction.Wpf;
using System.Security;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.Win32;
using System.Xml.Linq;
using System.Xml;
using System.Xml.Schema;
using MessageBox = System.Windows.MessageBox;
using Prove_Tobii.Model;
using System.Xml.Serialization;
using Prove_Tobii.Views;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;

namespace Prove_Tobii
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private String configurationFileError;
        private bool correctXMLInserted = false;
        private String path;
        WavList wavList = null;
        public ObservableCollection<ValidationError> ValidationErrors { get; private set; }

        public MainWindow()
        {
            this.ValidationErrors = new ObservableCollection<ValidationError>();
            InitializeComponent();
            comboBoxIstruzione.SelectedIndex = 1;
            User data = new User();
            this.DataContext = data;
            EyeGazeDataSingleton.Instance.User = data;
            //((App)Application.Current).Host.Commands.Input.SendActivationModeOn();
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                MessageBoxResult result = MessageBox.Show("Sei sicuro di voler uscire dall'applicazione?", "Attenzione", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        Application.Current.Shutdown();
                        break;
                    default:
                        break;
                }
            }
        }



        private static void ValidationEventHandler(
    object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Error)
            {
                throw args.Exception;
            }

            Debug.WriteLine(args.Message);
        }

        public void ValidateXmlDocument(
    XmlReader documentToValidate)
        {
            string xsdMarkup =
    @"<xs:schema attributeFormDefault='unqualified' elementFormDefault='qualified' xmlns:xs='http://www.w3.org/2001/XMLSchema'>
  <xs:element name='WavList'>
    <xs:complexType>
      <xs:sequence>
        <xs:element name='AudioFile' maxOccurs='unbounded' minOccurs='1'>
          <xs:complexType>
            <xs:sequence>
              <xs:element type='xs:string' name='Path'/>
              <xs:element type='xs:string' name='CorrectAnswer'/>
              <xs:element type='xs:string' name='CorrectAnswerImagePath' minOccurs='0' maxOccurs='1'/>
              <xs:element type='xs:string' name='WrongAnswer'/>
              <xs:element type='xs:string' name='WrongAnswerImagePath' minOccurs='0' maxOccurs='1'/>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
            XmlSchema schema;
            using (var schemaReader = XmlReader.Create(new StringReader(xsdMarkup)))
            {
                schema = XmlSchema.Read(schemaReader, ValidationEventHandler);
            }

            var schemas = new XmlSchemaSet();
            schemas.Add(schema);

            var settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas = schemas;
            settings.ValidationFlags =
                XmlSchemaValidationFlags.ProcessIdentityConstraints |
                XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += ValidationEventHandler;

            using (var validationReader = XmlReader.Create(documentToValidate, settings))
            {
                while (validationReader.Read())
                {
                }
            }
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            
            EyeGazeDataSingleton.Instance.User.Age = datePicker.SelectedDate.Value;
            EyeGazeDataSingleton.Instance.User.EducationLevel = comboBoxIstruzione.Text;
            EyeGazeDataSingleton.Instance.User.FirstName = textBoxFirstName.Text;
            EyeGazeDataSingleton.Instance.User.LastName = textBoxLastName.Text;
            //TODO: passa alla finestra successiva
            WelcomeScreen ws = new WelcomeScreen(wavList.AudioFiles, 0);
            ws.Show();
            this.Close();
            
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            clearInput();
            WelcomeScreen ws = new WelcomeScreen();
            ws.Show();
            this.Close();
        }

        private bool validateXMLConfigurationFile()
        {
            string xsdMarkup =
   @"<xs:schema attributeFormDefault='unqualified' elementFormDefault='qualified' xmlns:xs='http://www.w3.org/2001/XMLSchema'>
  <xs:element name='WavList'>
    <xs:complexType>
      <xs:sequence>
        <xs:element name='AudioFile' maxOccurs='unbounded' minOccurs='1'>
          <xs:complexType>
            <xs:sequence>
              <xs:element type='xs:string' name='Path'/>
              <xs:element type='xs:string' name='CorrectAnswer'/>
              <xs:element type='xs:string' name='CorrectAnswerImagePath' minOccurs='0' maxOccurs='1'/>
              <xs:element type='xs:string' name='WrongAnswer'/>
              <xs:element type='xs:string' name='WrongAnswerImagePath' minOccurs='0' maxOccurs='1'/>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>

";
            XmlReaderSettings settings = new XmlReaderSettings();
            ValidationEventHandler eventHandler = new ValidationEventHandler(ValidationEventHandler);
            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add("", XmlReader.Create(new StringReader(xsdMarkup)));
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "XML files (*.xml)|*.xml";
            if (openFileDialog.ShowDialog() == true)
            {
                XmlReader reader = XmlReader.Create(openFileDialog.FileName, settings);
                try
                {
                    ValidateXmlDocument(reader);
                    XmlSerializer serializer = new XmlSerializer(typeof(WavList), new XmlRootAttribute("WavList"));
                    var filePath = openFileDialog.FileName;
                    path = openFileDialog.SafeFileName;
                    StreamReader xmlStringReader = new StreamReader(filePath);
                    Debug.WriteLine(xmlStringReader);
                    wavList = (WavList)serializer.Deserialize(xmlStringReader);
                    reader.Close();
                    if (wavList.AudioFiles.Length > 0)
                    {
                        if(ValidationErrors.Count == 0)
                        {
                            Submit.IsEnabled = true;
                            Submit.Background = Brushes.Green;
                            Submit.Foreground = Brushes.White;
                        }
                        correctXMLInserted = true;
                        return true;
                    }
                    else
                    {
                        configurationFileError = "Il file xml inserito non contiene alcun audio.";
                        correctXMLInserted = false;
                        MessageBox.Show("Il file xml inserito non contiene alcun audio.", " XML non valido",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }

                }
                catch (XmlSchemaException excep)
                {
                    correctXMLInserted = false;
                    MessageBox.Show("Il file XML inserito non rispetta la sintassi. Informazioni sull'errore: " + excep.Message, "XML non valido",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    configurationFileError = "Il file XML inserito non rispetta la sintassi. Informazioni sull'errore: " + excep.Message;
                    return false;
                }
                catch (XmlException excep)
                {
                    correctXMLInserted = false;
                    MessageBox.Show("Il file XML inserito non rispetta la sintassi. Informazioni sull'errore: " + excep.Message, "XML non valido",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    configurationFileError = "Il file XML inserito non rispetta la sintassi. Informazioni sull'errore: " + excep.Message;
                    return false;
                }
            }
            configurationFileError = "Impossibile scegliere un file.";
            return false;
        }

        private void clearInput()
        {
            textBoxFirstName.Text = "";
            textBoxLastName.Text = "";
            comboBoxIstruzione.SelectedValue = null;
            datePicker.SelectedDate = null;
            datePicker.DisplayDate = DateTime.Now;
        }
        
        

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if(datePicker.SelectedDate != null)
            {
                datePicker.DisplayDate = datePicker.SelectedDate.Value;
            }
        }

        private void FilePicker_Click(object sender, RoutedEventArgs e)
        {
            var isValid = validateXMLConfigurationFile();
            if (!isValid)
            {
                correctXMLInserted = false;
            }
            else
            {
                filePicker.Content = path;
            }
        }

        private void Window_Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                ValidationErrors.Add(e.Error);
            }
            else
            {
                ValidationErrors.Remove(e.Error);
            }
            if(ValidationErrors.Count == 0 && correctXMLInserted == true)
            {
                Submit.IsEnabled = true;
            }
            else
            {
                if(Submit != null)
                {
                    Submit.IsEnabled = false;
                    Submit.Background = Brushes.Gray;
                    Submit.Foreground = Brushes.DarkGray;
                }
            }
        }

    }   

}

