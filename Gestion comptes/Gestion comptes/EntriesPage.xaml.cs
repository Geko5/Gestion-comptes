using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gestion_comptes
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
        /// Evènement levé lorsqu'on clique sur le bouton "Menu"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnClickMenu(object sender, EventArgs e)
        {
            // On retire de la pile de navigation la page OutgoingsPage
            // en paramètre un boléen à false pour enlever les animations
            await Navigation.PopAsync(false);

            // Ou on peut faire de cette manière qui nous permet de revenir à la page Root dans le cas de navigation de plusieurs pages d'affilés
            //await Navigation.PopToRootAsync();
        }
    }
}