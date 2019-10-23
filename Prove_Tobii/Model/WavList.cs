
namespace Prove_Tobii.Model
{


    // 
    // Codice sorgente generato automaticamente da xsd, versione=4.8.3928.0.
    // 


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class WavList
    {

        private WavListAudioFiles[] audioFilesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("AudioFile")]
        public WavListAudioFiles[] AudioFiles
        {
            get
            {
                return this.audioFilesField;
            }
            set
            {
                this.audioFilesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class WavListAudioFiles
    {

        private string pathField;

        private string correctAnswerField;

        private string correctAnswerImagePathField;

        private string wrongAnswerField;

        private string wrongAnswerImagePathField;

        /// <remarks/>
        public string Path
        {
            get
            {
                return this.pathField;
            }
            set
            {
                this.pathField = value;
            }
        }

        /// <remarks/>
        public string CorrectAnswer
        {
            get
            {
                return this.correctAnswerField;
            }
            set
            {
                this.correctAnswerField = value;
            }
        }

        /// <remarks/>
        public string CorrectAnswerImagePath
        {
            get
            {
                return this.correctAnswerImagePathField;
            }
            set
            {
                this.correctAnswerImagePathField = value;
            }
        }

        /// <remarks/>
        public string WrongAnswer
        {
            get
            {
                return this.wrongAnswerField;
            }
            set
            {
                this.wrongAnswerField = value;
            }
        }

        /// <remarks/>
        public string WrongAnswerImagePath
        {
            get
            {
                return this.wrongAnswerImagePathField;
            }
            set
            {
                this.wrongAnswerImagePathField = value;
            }
        }
    }


}

