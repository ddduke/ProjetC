/*TO DEV : 

 * 
 * 
 * COMBAT :
 * UnitFight :
 * Masterscript a créer avec des fonctions utiles pour fight : check des enemis a proximité, check des enemis qui nous attaquent, puis une fois qu'on a la liste on lance la fight
 * intégrer dans unitfight une fonction  pour lister les différents régiments dans la zone et leur action :
 * -  les régiments qui peuvent être attaqués par l'unité (nourri par la fonction de recherche de unitfight de l'unité en question)
 * -  les régiments qui peuvent attaquer l'unité (nourri par les scripts unitfight de ces régiments)
 *  Identifier les différents régiments à attaquer et les ordonner : quel régiment devant, quel régiment sur les côtés ou derrière, combien ? (cf 17 et 18 de Concept Systeme de combat)
 * Créer une liste avec : 
 * - les régiments qui sont attaqués par le regiment sur ce round, avec optionnellement la possibilité qu'ils soient stuck in fight et qu'ils ne puissent plus bouger (bool à intégrer dans le regiment path get to next position)
 * - les régiments qui vont attaquer ce régiment sur ce round (nourri par la fonction de priorisation de unitfight de l'unité en question)
 * Puis  Fight avec les peoples etc...
 * SI dead alors on les mets au paradis 
 * re check si on est stuck in fight pour le round suivant
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
 * 
 * 
 * 
 * 
 * MAP :
 * Mettre une condition pour empecher le joueur de cliquer sur les autres POI quand l'action Moving est lancée
 * Tester la génération de map : encore des connexions incohérentes = repenser le process ? 
 * 

 Question : 

Display sur un hover d'une unité :  Mettre également le movement capacity individuelle ? Pas forcément utile vu qu'on les dirige pas individuellement
 * 
 */
