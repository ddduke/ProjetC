/*TO DEV : 
 * COMBAT :
 * 
 * intégrer un calcul des z min & max de chaque formation et l'appliquer sur la target du formation pivot 
 * exemple : j'ai une formation qui fait 5 de large et que je veux envoyer tout en bas il faut que le formaton pivot 
 * soit positionné de telle manière que mon unité la plus en bas puisse se positionner également
 * 
 *      
 * 
 * 
 * rajouter les unités en obstacle sur le pathfinding
 * 
 * Attention l'attaque doit etre gérée par le régiment de manièer unitaire et pas par une fonction macro (la fonction macro doit activer l'attaque mais la fonction attaque peut etre appelée par d'autres fonctions ex feat) 
 * 
 * pour les déplacements non simultanés (ex. charge) faire une analyse des chemins vers l'unité la plus proche et regarder la meilleure simulation (cas ou la distance moyenne des unités est la plus proche de l'unité enemie)
 * 
 * rajouter un check pour regarder si l'unité a bien les bonus de formation : créer un formation variables lié à l'unité avec les multiplicateurs défense ou attaque ? 
 * rajouter la possibilité de flancker
 * rajouter la capacité de changer de formation = restart la fonction avec la nouvelle formation (systeme de fenetre ou l'on décide de l'emplacement également ?)
 * mettre le pathfinding en A*
 * 
 * MAP :
 * Mettre une condition pour empecher le joueur de cliquer sur les autres POI quand l'action Moving est lancée
 * Tester la génération de map : encore des connexions incohérentes = repenser le process ? 
 * 
 */
