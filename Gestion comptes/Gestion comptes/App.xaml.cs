using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;
using Gestion_comptes.ViewModel;
using Gestion_comptes.Model;
using System.IO;

namespace Gestion_comptes
{
    /// <summary>
    /// Classe qui représente l'application mobile
    /// </summary>
    public partial class App : Application
    {
        static MovementDAL _database;

        /// <summary>
        /// Propriété pour accéder à la base de données
        /// </summary>
        public static MovementDAL DataBase
        {
            get
            {
                if (_database == null)
                    _database = new MovementDAL(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "movements.db3"));

                return _database;
            }
        }

        /// <summary>
        /// Constructeur de l'appli
        /// </summary>
        public App()
        {
            InitializeComponent();

            // MainPage est définit comme Page "root" de notre NavigationPage
            MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.FromHex("B5338A"),
                BarTextColor = Color.White
            };
        }

        /// <summary>
        /// Méthode qui va s'exécuter uniquement lorsque l'appli démarre pour la première fois
        /// </summary>
        protected override void OnStart()
        {
            Debug.WriteLine("OnStart");

            if (Current.Properties.ContainsKey("MainPageID"))
            {
                var id = Current.Properties["MainPageID"];
                Debug.WriteLine("OnStart - " + id);
            }
        }

        /// <summary>
        /// Méthode pour effectuer des actions quand l'application rentre en veille (quand elle passe en arrière plan)
        /// </summary>
        protected override void OnSleep()
        {
            Debug.WriteLine("OnSleep");
        }

        /// <summary>
        /// Méthode pour effectuer des actions quand l'appli sort de veille ou quand elle sort de pause
        /// </summary>
        protected override void OnResume()
        {
            Debug.WriteLine("OnResume");

            if (Current.Properties.ContainsKey("MainPageID"))
            {
                var id = Current.Properties["MainPageID"];
                Debug.WriteLine("OnResume - " + id);
            }
        }
    }
}
