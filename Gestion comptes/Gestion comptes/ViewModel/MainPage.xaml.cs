using System;
using System.Globalization;
using Xamarin.Forms;
using Gestion_comptes.View;
using Type = Gestion_comptes.Model.Type;
using System.Collections.ObjectModel;
using Gestion_comptes.Model;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace Gestion_comptes.ViewModel
{
    /// <summary>
    /// Classe qui gère la page principale de l'appli : écran d'accueil
    /// </summary>
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

        private DateTime _dateToday;

        /// <summary>
        /// Méthode qui permet d'initialiser les dates dans les labels
        /// </summary>
        private void InitializeDate()
        {
            _dateToday = DateTime.Now;
            LabelDate.Text = _dateToday.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            LabelMonth.Text = _dateToday.ToString("MMMM", new CultureInfo("fr")).ToUpper();
        }

        /// <summary>
        /// Evénement levé quand on clique sur le bouton "Ajouter" de la fenêtre Dépenses
        /// méthode qui est asynchrone (car la navigation est asynchrone avec Xamarin)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnClickOutgoingsPage(object sender, EventArgs e)
        {
            // On rajoute de manière asynchrone un objet de type Page en haut de la pile de navigation
            await Navigation.PushAsync(new OutgoingsPage());
        }

        /// <summary>
        /// Evénement levé quand on clique sur le bouton "Ajouter" de la fenêtre Entrées
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnClickEntriesPage(object sender, EventArgs e)
        {
            // On rajoute de manière asynchrone un objet de type Page en haut de la pile de navigation
            await Navigation.PushAsync(new EntriesPage());
        }

        /// <summary>
        /// Evénement levé quand on clique sur le bouton "Ajouter de la fenêtre Frais fixes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnClickFixedCostsPage(object sender, EventArgs e)
        {
            // On rajoute de manière asynchrone un objet de type Page en haut de la pile de navigation
            await Navigation.PushAsync(new FixedCostsPage());
        }

        /// <summary>
        /// Evénement levé quand on clique sur le bouton "Ajouter" de la fenêtre Epargne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnClickSavingsPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SavingsPage());
        }

        /// <summary>
        /// Evénement levé quand on clique sur le bouton "Visualiser" de la fenêtre Total
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnClickVisualizingPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VisualizingPage());
        }

        private string _signActualAmount = string.Empty;
        private decimal _actualAmount = 0;

        /// <summary>
        /// Méthode qui permet de charger des éléments avant que la page soit visible
        /// </summary>
        protected override void OnAppearing()
        {
            ComputeDatas();
        }

        /// <summary>
        /// Méthode qui permet de calculer toutes les données de chaque fenêtre de la page principale
        /// </summary>
        private void ComputeDatas()
        {
            // Calcul des dépenses totales
            Dictionary<decimal, decimal> totalOutgoings = App.DataBase.ComputeTotalAmount(Type.Outgoings, _dateToday);

            decimal totalOutgoingsAmount = totalOutgoings.Keys.FirstOrDefault();
            decimal totalOutgoingsAmountToday = totalOutgoings.Values.FirstOrDefault();

            LabelTotalAmountOutgoings.Text = "- " + totalOutgoingsAmount.ToString("C", new CultureInfo("fr-FR"));

            // Calcul des entrées totales
            Dictionary<decimal, decimal> totalEntries = App.DataBase.ComputeTotalAmount(Type.Entries, _dateToday);

            decimal totalEntriesAmount = totalEntries.Keys.FirstOrDefault();
            decimal totalEntriesAmountToday = totalEntries.Values.FirstOrDefault();

            LabelTotalAmountEntries.Text = "+ " + totalEntriesAmount.ToString("C", new CultureInfo("fr-FR"));

            // Calcul des frais fixes totaux
            Dictionary<decimal, decimal> totalFixedCosts = App.DataBase.ComputeTotalAmount(Type.FixedCosts, _dateToday);

            decimal totalFixedCostsAmount = totalFixedCosts.Keys.FirstOrDefault();
            decimal totalFixedCostsAmountToday = totalFixedCosts.Values.FirstOrDefault();

            LabelTotalAmountFixedCosts.Text = "- " + totalFixedCostsAmount.ToString("C", new CultureInfo("fr-FR"));

            // Calcul des épargnes
            decimal totalSavingsAmount = App.DataBase.ComputeTotalAmount(Type.Savings, _dateToday).Keys.FirstOrDefault();
            LabelTotalAmountSavings.Text = "+ " + totalSavingsAmount.ToString("C", new CultureInfo("fr-FR"));

            // Calcul du montant prévisionnel
            decimal provisionalAmount = totalEntriesAmount - totalOutgoingsAmount - totalFixedCostsAmount;

            // Calcul du montant actuel/ d'aujourd'hui
            decimal actualAmount = totalEntriesAmountToday - totalOutgoingsAmountToday - totalFixedCostsAmountToday;

            // Calcul du montant prévisionnel avec l'épargne
            decimal actualAmountWithSavings = provisionalAmount + totalSavingsAmount;

            string signProvesionalAmount = string.Empty;
            string signActualAmountWithSavings = string.Empty;
            string signActualAmount = string.Empty;

            if (provisionalAmount > 0)
            {
                LabelProvisionalAmount.TextColor = Color.Green;
                signProvesionalAmount = "+ ";
            }
            else
                LabelProvisionalAmount.TextColor = Color.Red;

            if (actualAmountWithSavings > 0)
            {
                LabelActualAmountWithSavings.TextColor = Color.Green;
                signActualAmountWithSavings = "+ ";
            }
            else
                LabelActualAmountWithSavings.TextColor = Color.Red;

            if (actualAmount > 0)
            {
                LabelActualAmount.TextColor = Color.Green;
                signActualAmount = "+ ";
            }
            else
                LabelActualAmount.TextColor = Color.Red;

            _actualAmount = actualAmount;
            _signActualAmount = signActualAmount;

            LabelActualAmount.Text = signActualAmount + actualAmount.ToString("C", new CultureInfo("fr-FR"));
            LabelActualAmountWithSavings.Text = signActualAmountWithSavings + actualAmountWithSavings.ToString("C", new CultureInfo("fr-FR"));
            LabelProvisionalAmount.Text = signProvesionalAmount + provisionalAmount.ToString("C", new CultureInfo("fr-FR"));
        }

        /// <summary>
        /// Evénement levé quand on clique sur le toolbarmenu "Réinitialiser un nouveau mois"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnClickResetMenu(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Réinitialisation", string.Format("Voulez-vous récupérer {0}{1}€ restant du mois dernier ?", _signActualAmount, _actualAmount), "Oui", "Annuler");

            // Si on réinitialise les données avec le restant du mois précédent
            if (answer)
            {
                Type type;
                if (_actualAmount > 0)
                {
                    _signActualAmount = "+ ";
                    type = Type.Entries;
                }
                else
                {
                    _signActualAmount = "- ";
                    type = Type.Outgoings;
                }

                // On crée un nouveau mouvement avec le reste du mois précédent
                Movement movementToAdd = new Movement()
                {
                    Amount = _actualAmount,
                    Sign = _signActualAmount,
                    Category = "Reste mois précédent",
                    Date = DateTime.Now,
                    MovementType = type
                };

                await App.DataBase.SaveMovementAsync(movementToAdd);

                // On crée un réglage avec la date de réinitialisation pour les calculs des montants
                Settings settings = new Settings()
                {
                    ResetDateTime = DateTime.Now
                };

                await App.DataBase.SaveResetDateTime(settings);

                // On relance tous les calculs
                ComputeDatas();
            }
        }
    }
}
