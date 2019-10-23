using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prove_Tobii.Model
{
    /// <summary>
    /// Singleton che mantiene i dati . Si usa cosi: EyeGazeDataSingleton s1 = EyeGazeDataSingleton.Instance;
    /// </summary>
    public sealed class EyeGazeDataSingleton {
        //membro privato che rappresenta l'instanza della classe
        [NonSerialized()] private static EyeGazeDataSingleton _instance;
        private int correctAnswers;
        private int wrongAnswers;
        private int unAnsweredQuestions;
        private User user;

        //costruttore privato senza param non accessibile dall'esterno della classe
        private EyeGazeDataSingleton() { }

    //Entry-Point: proprietà esterna che ritorna l'istanza della classe
        public static EyeGazeDataSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EyeGazeDataSingleton();
                }
                return _instance;
            }
        }

        public int CorrectAnswers { get => correctAnswers; set => correctAnswers = value; }
        public int WrongAnswers { get => wrongAnswers; set => wrongAnswers = value; }
        public int UnAnsweredQuestions { get => unAnsweredQuestions; set => unAnsweredQuestions = value; }
        public User User { get => user; set => user = value; }
        }
}
