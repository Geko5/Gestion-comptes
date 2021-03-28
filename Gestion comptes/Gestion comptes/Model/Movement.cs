using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using Xamarin.Forms;

/// <summary>
/// prout
/// </summary>
namespace Gestion_comptes.Model
{
    /// <summary>
    /// Enuméré des types de mouvement existant
    /// Outgoings : dépenses
    /// FixedCosts : frais fixes
    /// Entries : entrées
    /// Savings : épargnes
    /// </summary>
    public enum Type { Outgoings, FixedCosts, Entries, Savings }

    /// <summary>
    /// Entité Mouvement
    /// </summary>
    public class Movement
    {
        /// <summary>
        /// Identifiant du mouvement auto incrémenté
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Signe du mouvement (+/-)
        /// </summary>
        public string Sign { get; set; }

        /// <summary>
        /// Quantité lié au mouvement
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Type de mouvement
        /// </summary>
        public Type MovementType { get; set; }

        /// <summary>
        /// Nom de la catégorie associé au mouvement
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Date et heure où a été enregistré le mouvement
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Couleur du texte du mouvement en xaml
        /// </summary>
        public string LabelTextColor { get; set; }

        /// <summary>
        /// Style de l'attribut du mouvement en xaml
        /// </summary>
        public FontAttributes LabelFontAttribute { get; set; }
    }
}
