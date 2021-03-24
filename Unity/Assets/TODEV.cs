/*TO DEV : 
 * COMBAT :
 * 
 * 
 * Last Step Formation Charge ligne 180 : Display the path selected putting a path display on each unit , we have to fill the path display gameobject instantiated with the path used in the selected path list
 * see if we can store it directly, but seems to need another function to be called because the seeker won't word in this case
 * 
 * 
 * 
 * 
 * Attention l'attaque doit etre gérée par le régiment de manière unitaire et pas par une fonction macro (la fonction macro doit activer l'attaque mais la fonction attaque peut etre appelée par d'autres fonctions ex feat) 
 * 
 * 
 * rajouter un check pour regarder si l'unité a bien les bonus de formation : créer un formation variables lié à l'unité avec les multiplicateurs défense ou attaque ? 
 * rajouter la possibilité de flancker
 * rajouter la capacité de changer de formation = restart la fonction avec la nouvelle formation (systeme de fenetre ou l'on décide de l'emplacement également ?)
 * 
 * MAP :
 * Mettre une condition pour empecher le joueur de cliquer sur les autres POI quand l'action Moving est lancée
 * Tester la génération de map : encore des connexions incohérentes = repenser le process ? 
 * 
 */
