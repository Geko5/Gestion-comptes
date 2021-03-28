using Gestion_comptes.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;
using Type = Gestion_comptes.Model.Type;

namespace Gestion_comptes.ViewModel
{
    /// <summary>
    /// Classe qui répertorie les méthodes/règles communes entre les différentes saisies/vues
    /// </summary>
    public class Rules
    {
        /// <summary>
        /// Méthode qui permet de transformer un texte saisie en décimal en prenant en compte les "." et ","
        /// </summary>
        /// <param name="textToTransform">Texte à convertir</param>
        /// <returns>Décimal</returns>
        internal static decimal TransformTextInDecimal(string textToTransform)
        {
            if (string.IsNullOrWhiteSpace(textToTransform))
                return 0;
            else if (textToTransform.Contains(","))
                return Convert.ToDecimal(textToTransform, new CultureInfo("fr-FR", false).NumberFormat);
            else
                return Convert.ToDecimal(textToTransform, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Méthode qui permet de valider les champs passés en paramètre pour pouvoir ajouter un mouvement
        /// </summary>
        /// <param name="amount">Montant à valider</param>
        /// <param name="category">Catégorie à valider</param>
        /// <param name="amountValidated">Message de validation</param>
        /// <param name="addMovementButton">Bouton "Ajouter" mouvement</param>
        internal static void ValidateField(string amount, string category, Label amountValidated, Button addMovementButton)
        {
            if (string.IsNullOrWhiteSpace(amount))
            {
                amountValidated.Text = "Champ obligatoire";
                amountValidated.TextColor = Color.Red;
                amountValidated.FontAttributes = FontAttributes.Italic;
                amountValidated.FontSize = 10;
                addMovementButton.IsEnabled = false;
            }
            else
            {
                amountValidated.Text = string.Empty;

                if (!string.IsNullOrWhiteSpace(category))
                    addMovementButton.IsEnabled = true;
            }
        }

        /// <summary>
        /// Méthode qui permet de vérifier que le champ passé en paramètre et qui est en cours de saisie est bien de type numérique
        /// </summary>
        /// <param name="text">Texte en cours de saisie</param>
        /// <param name="lblAmountValidation">Message de validation</param>
        internal static bool ValidateMontantField(string amount, Label lblAmountValidation)
        {
            Regex regex = new Regex("[^0-9,.]+");
            bool isNotNumber = regex.IsMatch(amount);

            if (isNotNumber)
            {
                lblAmountValidation.Text = "Seul les chiffres sont autorisés";
                lblAmountValidation.TextColor = Color.Red;
                lblAmountValidation.FontAttributes = FontAttributes.Italic;
                lblAmountValidation.FontSize = 10;
            }

            return isNotNumber;
        }

        /// <summary>
        /// Méthode qui permet de récupérer les mouvements en base et de mettre à jour les éléments visuels pour chacun des mouvements
        /// en fonction de la date
        /// </summary>
        /// <returns>Liste de mouvements</returns>
        internal static async Task<List<Movement>> GetAndSetVisualMovements(Type? movementType)
        {
            List<Movement> movements = await App.DataBase.GetMovementsAsync(movementType);

            foreach (Movement movement in movements)
            {
                if (movement.Date < DateTime.Today)
                {
                    movement.LabelTextColor = "Gray";
                    movement.LabelFontAttribute = FontAttributes.Italic;
                }
                else if (movement.Date == DateTime.Today)
                {
                    movement.LabelTextColor = "Blue";
                    movement.LabelFontAttribute = FontAttributes.Bold;
                }
                else if (movement.Date > DateTime.Today)
                {
                    movement.LabelTextColor = "CornflowerBlue";
                    movement.LabelFontAttribute = FontAttributes.None;
                }
            }
            return movements;
        }
    }
}
