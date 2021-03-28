using Android.App.Job;
using Gestion_comptes.Model;
using Gestion_comptes.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Type = Gestion_comptes.Model.Type;

namespace Gestion_comptes.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntriesPage : ContentPage
    {
        /// <summary>
        /// Constructeur publique
        /// </summary>
        public EntriesPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Méthode qui permet de charger des éléments avant que la page soit visible
        /// </summary>
        protected async override void OnAppearing()
        {
            ListMovementsOfEntries.ItemsSource = await Rules.GetAndSetVisualMovements(Type.Entries);
            BtnAddMovement.IsEnabled = false;
        }

        /// <summary>
        /// Evènement levé lorsqu'on clique sur le bouton "Ajouter"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnClickButtonAdd(object sender, EventArgs e)
        {
            Movement movementToAdd = new Movement
            {
                Date = TxtDate.Date,
                Sign = "+",
                Amount = Rules.TransformTextInDecimal(TxtAmount.Text),
                Category = TxtCategory.Text,
                MovementType = Type.Entries
            };

            await App.DataBase.SaveMovementAsync(movementToAdd);

            await DisplayAlert("Ajout", "Entrée ajoutée avec succès", "OK");

            ListMovementsOfEntries.ItemsSource = await Rules.GetAndSetVisualMovements(Type.Entries);

            TxtAmount.Text = string.Empty;
            TxtCategory.Text = string.Empty;

            BtnAddMovement.IsEnabled = false;
            LblCategoryValidation.Text = string.Empty;
            TxtDate.Date = DateTime.Today;
        }

        /// <summary>
        /// Evènement levé quand l'utilisateur quitte le champ "Montant"
        /// Si le champ n'est pas rempli on met un message "Champ obligatoire"
        /// Le bouton "Ajouter" est disponible que quand les 2 champs sont remplis
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtAmount_Completed(object sender, EventArgs e)
        {
            Rules.ValidateField(TxtAmount.Text, TxtCategory.Text, LblAmountValidation, BtnAddMovement);
        }

        /// <summary>
        /// Evènement levé quand l'utilisateur quitte le champ "Catégorie"
        /// Si le champ n'est pas rempli on met un message "Champ obligatoire"
        /// Le bouton "Ajouter" est disponible que quand les 2 champs sont remplis
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtCategory_Completed(object sender, EventArgs e)
        {
            Rules.ValidateField(TxtCategory.Text, TxtAmount.Text, LblCategoryValidation, BtnAddMovement);
        }

        /// <summary>
        /// Evènement levé quand l'utilisateur est entrain de saisir le champ "Montant"
        /// Si l'utilisateur écrit autre chose que des chiffres on met un message "Seul les chiffres sont autorisés"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Si ce qui est saisi n'est pas de type numérique, on efface et on met le message d'erreur
            if (Rules.ValidateMontantField(TxtAmount.Text, LblAmountValidation))
                TxtAmount.Text = string.Empty;

            if (string.IsNullOrWhiteSpace(TxtAmount.Text))
                BtnAddMovement.IsEnabled = false;
            else
            {
                if (!string.IsNullOrWhiteSpace(TxtCategory.Text))
                    BtnAddMovement.IsEnabled = true;
            }
        }

        /// <summary>
        /// Evènement levé quand l'utilisateur est entrain de saisir le champ "Catégorie"
        /// Dès qu'il saisit une lettre et que le champ "Montant" est rempli, alors le bouton "Ajouter" est disponible
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtCategory_TextChanged(object sender, TextChangedEventArgs e)
        {
            Rules.ValidateField(TxtCategory.Text, TxtAmount.Text, LblCategoryValidation, BtnAddMovement);
        }

        /// <summary>
        /// Méthode qui permet de mettre à jour un mouvement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void UpdateMovementOnClick(object sender, EventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            Movement movementToUpdate = (Movement)menuItem.CommandParameter;

            // Ouverture d'une modale pour modifier le mouvement
            await Navigation.PushModalAsync(new UpdateMovementModalPage(movementToUpdate), true);
        }

        /// <summary>
        /// Méthode qui permet de supprimer le mouvement sélectionné
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DeleteMovementOnClick(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Suppression", "Voulez-vous vraiment supprimer ce mouvement ?", "Oui", "Annuler");

            if (answer)
            {
                MenuItem menuItem = (MenuItem)sender;
                Movement movementToDelete = (Movement)menuItem.CommandParameter;

                await App.DataBase.DeleteMovementAsync(movementToDelete);

                ListMovementsOfEntries.ItemsSource = await Rules.GetAndSetVisualMovements(Type.Entries);
            }
        }
    }
}