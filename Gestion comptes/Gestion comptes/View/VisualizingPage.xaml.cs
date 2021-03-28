using Gestion_comptes.Model;
using Gestion_comptes.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gestion_comptes.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VisualizingPage : ContentPage
    {
        /// <summary>
        /// Constructeur publique
        /// </summary>
        public VisualizingPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Méthode qui permet de charger des éléments avant que la page soit visible
        /// </summary>
        protected async override void OnAppearing()
        {
            ListMovementsOfTotalCosts.ItemsSource = await Rules.GetAndSetVisualMovements(null);
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
            await Navigation.PushModalAsync(new UpdateMovementModalPage(movementToUpdate), false);
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

                ListMovementsOfTotalCosts.ItemsSource = await Rules.GetAndSetVisualMovements(null);
            }
        }
    }
}