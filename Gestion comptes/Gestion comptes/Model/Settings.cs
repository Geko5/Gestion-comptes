using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gestion_comptes.Model
{
    /// <summary>
    /// Entité des réglages/préférences dans l'appli 
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Identifiant du réglage, auto incrémenté
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Date à laquelle on a réinitialisé un nouveau mois
        /// </summary>
        public DateTime ResetDateTime { get; set; }
    }
}
