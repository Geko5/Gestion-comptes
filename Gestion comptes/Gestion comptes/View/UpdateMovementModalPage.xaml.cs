using Gestion_comptes.Model;
using Gestion_comptes.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gestion_comptes.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateMovementModalPage : ContentPage
    {
        /// <summary>
        /// Mouvement sélectionné pour MAJ
        /// </summary>
        private Movement _currentMovementToUpdate;

        /// <summary>
        /// Constructeur publique
        /// </summary>
        /// <param name="movement"></param>
        public UpdateMovementModalPage(Movement movement)
        {
            InitializeComponent();

            if (_currentMovementToUpdate == null)
                _currentMovementToUpdate = movement;
        }

        /// <summary>
        /// Méthode qui permet de charger des éléments avant que la page soit visible
        /// </summary>
        protected override void OnAppearing()
        {
            TxtAmountUpdated.Text = _currentMovementToUpdate.Amount.ToString(CultureInfo.InvariantCulture);
            TxtCategoryUpdated.Text = _currentMovementToUpdate.Category.ToString(CultureInfo.InvariantCulture);
            TxtDateUpdated.Date = _currentMovementToUpdate.Date;
        }

        /// <summary>
        /// Bouton qui permet de mettre à jour le mouvement selectionné
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnClickUpdateButton(object sender, EventArgs e)
        {
            _currentMovementToUpdate.Amount = Rules.TransformTextInDecimal(TxtAmountUpdated.Text);
            _currentMovementToUpdate.Category = TxtCategoryUpdated.Text;
            _currentMovementToUpdate.Date = TxtDateUpdated.Date;

            await App.DataBase.SaveMovementAsync(_currentMovementToUpdate);

            await DisplayAlert("Modification", "Mouvement modifié avec succès", "OK");

            // Permet de quitter la modale en cours et revenir à la saisie précédente
            await Navigation.PopModalAsync();
        }

        /// <summary>
        /// Evènement levé quand l'utilisateur quitte le champ "Montant"
        /// Si le champ n'est pas rempli on met un message "Champ obligatoire"
        /// Le bouton "Ajouter" est disponible que quand les 2 champs sont remplis
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtAmountUpdated_Completed(object sender, EventArgs e)
        {
            Rules.ValidateField(TxtAmountUpdated.Text, TxtCategoryUpdated.Text, LblAmountUpdatedValidation, BtnUpdateMovement);
        }

        /// <summary>
        /// Evènement levé quand l'utilisateur quitte le champ "Catégorie"
        /// Si le champ n'est pas rempli on met un message "Champ obligatoire"
        /// Le bouton "Ajouter" est disponible que quand les 2 champs sont remplis
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtCategoryUpdated_Completed(object sender, EventArgs e)
        {
            Rules.ValidateField(TxtCategoryUpdated.Text, TxtAmountUpdated.Text, LblCategoryUpdatedValidation, BtnUpdateMovement);
        }

        /// <summary>
        /// Evènement levé quand l'utilisateur est entrain de saisir le champ "Montant"
        /// Si l'utilisateur écrit autre chose que des chiffres on met un message "Seul les chiffres sont autorisés"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtAmountUpdated_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Si ce qui est saisi n'est pas de type numérique, on efface et on met le message d'erreur
            if (Rules.ValidateMontantField(TxtAmountUpdated.Text, LblAmountUpdatedValidation))
                TxtAmountUpdated.Text = string.Empty;

            if (string.IsNullOrWhiteSpace(TxtAmountUpdated.Text))
                BtnUpdateMovement.IsEnabled = false;
            else
            {
                if (!string.IsNullOrWhiteSpace(TxtCategoryUpdated.Text))
                    BtnUpdateMovement.IsEnabled = true;
            }
        }

        /// <summary>
        /// Evènement levé quand l'utilisateur est entrain de saisir le champ "Catégorie"
        /// Dès qu'il saisit une lettre et que le champ "Montant" est rempli, alors le bouton "Ajouter" est disponible
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtCategoryUpdated_TextChanged(object sender, TextChangedEventArgs e)
        {
            Rules.ValidateField(TxtCategoryUpdated.Text, TxtAmountUpdated.Text, LblCategoryUpdatedValidation, BtnUpdateMovement);
        }
    }
}