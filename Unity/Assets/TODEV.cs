/*TO DEV : 
 * COMBAT :
 * Créer archers à partir d'un blueprint
 * Dev check combat ou non (figé ? regles de déplacement)
 * Fight avec les peoples etc...
 * 
 * 
 * --> Bugs mineurs à gerer derrière : 
 * 
 * --> Carte : info pour chaque carte 
 * 
 * Display les units en update sur le régiment : pour dire que la premiere est devant, la deuxieme derrière etc
 * UI pour sélectionner une formation et la sauvegarder et relative distance from pivot pour chaque régiment de manière à retrouver cette formation a chaque fois qu'elle se reforme
 * 
 * 
 * --> Notes :
 * 
 * Attention l'attaque doit etre gérée par le régiment de manière unitaire et pas par une fonction macro (la fonction macro doit activer l'attaque mais la fonction attaque peut etre appelée par d'autres fonctions ex feat) 
 * rajouter un check pour regarder si l'unité a bien les bonus de formation : créer un formation variables lié à l'unité avec les multiplicateurs défense ou attaque ? 
 * rajouter la possibilité de flancker
 * rajouter la capacité de changer de formation = restart la fonction avec la nouvelle formation (systeme de fenetre ou l'on décide de l'emplacement également ?)
 * 
 * MAP :
 * Mettre une condition pour empecher le joueur de cliquer sur les autres POI quand l'action Moving est lancée
 * Tester la génération de map : encore des connexions incohérentes = repenser le process ? 
 * 
 * 
 */
