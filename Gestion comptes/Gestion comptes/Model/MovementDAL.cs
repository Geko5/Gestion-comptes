using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Gestion_comptes.Model
{
    /// <summary>
    /// Classe d'accès aux données des mouvements
    /// </summary>
    public class MovementDAL
    {
        /// <summary>
        /// Va permettre la connexion à la base de donnée
        /// </summary>
        readonly SQLiteAsyncConnection _database;

        /// <summary>
        /// Constructeur publique
        /// </summary>
        /// <param name="dbPath">Chaine de connexion à la base de données</param>
        public MovementDAL(string dbPath)
        {
            // Création d'une nouvelle connexion à la BDD
            if (_database == null)
                _database = new SQLiteAsyncConnection(dbPath);

            // Création de la table Movement en base
            _database.CreateTableAsync<Movement>().Wait();

            // Création de la table Settings en base
            _database.CreateTableAsync<Settings>().Wait();
        }

        /// <summary>
        /// Méthode qui permet de récupéréer
        /// </summary>
        /// <returns></returns>
        private DateTime? GetResetDateTime()
        {
            Settings setting = _database.Table<Settings>().FirstOrDefaultAsync().Result;

            if (setting != null)
                return setting.ResetDateTime;
            else
                return null;
        }

        /// <summary>
        /// Permet de rajouter en base un réglage
        /// </summary>
        /// <param name="Settings">Entité réglage à enregistrer</param>
        /// <returns>Tâche avec l'identifiant du réglage qui a été enregistré</returns>
        public Task<int> SaveResetDateTime(Settings settings)
        {
            return _database.InsertAsync(settings);
        }

        /// <summary>
        /// Méthode qui permet de retourner une liste des 25 premiers mouvements d'un type de mouvement donné
        /// Ou si aucun type n'est donné en paramètre on renvoit les 50 derniers mouvements tout type confondu
        /// </summary>
        /// <param name="movementType">Type de mouvement</param>
        /// <returns>Liste de mouvements</returns>
        public Task<List<Movement>> GetMovementsAsync(Type? movementType)
        {
            if (movementType.HasValue)
            {
                return _database.Table<Movement>()
                                            .Where(x => x.MovementType == movementType.Value)
                                            .OrderByDescending(x => x.Date)
                                            .Take(25)
                                            .ToListAsync();
            }
            else
            {
                return _database.Table<Movement>()
                                                .OrderByDescending(x => x.Date)
                                                .Take(50)
                                                .ToListAsync();
            }
        }

        /// <summary>
        /// Permet de modifier ou rajouter en base un mouvement
        /// </summary>
        /// <param name="movement">Entité mouvement à rajouter</param>
        /// <returns>Tâche avec l'identifiant du mouvement ?</returns>
        public Task<int> SaveMovementAsync(Movement movement)
        {
            // Met à jour le mouvement passé en paramètre si l'Id est différent de 0
            if (movement.Id != 0)
                return _database.UpdateAsync(movement);
            else
                return _database.InsertAsync(movement);
        }

        /// <summary>
        /// Permet de supprimer en base un mouvement
        /// </summary>
        /// <returns>Tâche avec l'identifiant du mouvement ?</returns>
        public Task<int> DeleteMovementAsync(Movement movement)
        {
            _database.DeleteAllAsync<Settings>();

            return _database.DeleteAsync(movement);
        }

        /// <summary>
        /// Méthode qui permet de calculer la somme de tous les mouvements enregistrés pour le type passé en paramètre et la date du mouvement
        /// </summary>
        /// <param name="movementType">Type de mouvement</param>
        /// <param name="dateToday">Date du mouvement</param>
        /// <returns>Dictionnaire avec en clé la somme des quantités et en valeur la somme des quantités en fonction de la date passée en paramètre</returns>
        public Dictionary<decimal, decimal> ComputeTotalAmount(Type movementType, DateTime dateToday)
        {
            Dictionary<decimal, decimal> results = new Dictionary<decimal, decimal>();

            decimal resultTotal = 0;
            List<Movement> totalMovements = _database.Table<Movement>()
                                                     .Where(x => x.MovementType == movementType)
                                                     .ToListAsync().Result;
            foreach (Movement movement in totalMovements)
                resultTotal += movement.Amount;

            decimal resultToday = 0;
            if (movementType != Type.Savings)
            {
                List<Movement> movements = new List<Movement>();

                if (GetResetDateTime().HasValue && GetResetDateTime().Value != null)
                {
                    DateTime resetDateTime = GetResetDateTime().Value;

                    movements = _database.Table<Movement>()
                                         .Where(x => x.MovementType == movementType)
                                         .Where(x => x.Date <= dateToday)
                                         .Where(x => x.Date >= resetDateTime)
                                         .ToListAsync().Result;
                }
                else
                {
                    movements = _database.Table<Movement>()
                                         .Where(x => x.MovementType == movementType)
                                         .Where(x => x.Date <= dateToday)
                                         .ToListAsync().Result;
                }

                foreach (Movement movement in movements)
                    resultToday += movement.Amount;
            }

            results.Add(resultTotal, resultToday);

            return results;
        }
    }
}
