using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Gestion_comptes
{
    public partial class MainPage : ContentPage
    {
        /// <summary>
        /// Constructeur
        /// </summary>
        public MainPage()
        {
            InitializeComponent();

            InitializeDate();
        }

        /// <summary>
        /// Méthode qui permet d'initialiser les dates dans les labels
        /// </summary>
        private void InitializeDate()
        {
            DateTime dateToday = DateTime.Now;
            LabelDate.Text = dateToday.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            LabelMonth.Text = dateToday.ToString("MMMM", new CultureInfo("fr")).ToUpper();
        }

        /// <summary>
        /// Evénement levé quand on clique sur le bouton "+ Dépenses"
        /// méthode qui est asynchrone (car la navigation est asynchrone avec Xamarin)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnClickOutgoingsPage(object sender, EventArgs e)
        {
            // On rajoute de manière asynchrone un object de type Page en haut de la pile de navigation
            await Navigation.PushAsync(new OutgoingsPage());
        }

        /// <summary>
        /// Evénement levé quand on clique sur le bouton "+ Entrées"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnClickEntriesPage(object sender, EventArgs e)
        {
            // On rajoute de manière asynchrone un object de type Page en haut de la pile de navigation
            await Navigation.PushAsync(new EntriesPage());
        }


        ///// <summary>
        ///// Evènement levé quand l'édition est terminée (quand la personne a terminé sa saisie)
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void Editor_Completed(object sender, EventArgs e)
        //{
        //    var editor = (Editor)sender;
        //    LblCompleted.Text = "TextCompleted: " + editor.Text;
        //}

        ///// <summary>
        ///// Evènement levé quand l'édition est en cours (quand la personne est en cours de saisie)
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void Editor_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    var newText = e.NewTextValue;
        //    LblCompleted.Text = "TextChanged: " + newText;
        //}

        ///// <summary>
        ///// Evénement quand l'entrée (quasiment pareil que l'Editor mais avec qq propriétés différentes) est complétée c'est à dire qu'on a cliqué sur le bouton Entrée du clavier
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void Entry_Completed(object sender, EventArgs e)
        //{
        //    var entry = (Entry)sender;
        //    LblCompleted.Text = "TextCompleted: " + entry.Text;
        //}

        ///// <summary>
        ///// Evénement levé quand l'entrée est en cours de changement
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    var newText = e.NewTextValue;
        //    LblCompleted.Text = "TextChanged: " + newText;
        //}
    }
}
